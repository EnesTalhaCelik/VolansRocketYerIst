using System;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace VolansGUI.Telemetry
{
    /// <summary>
    /// LoRa üzerinden gelen ham CSV veriyi satırlara ayırır, her satırın
    /// başındaki TEST_ID byte'ına göre uygun handler'a yönlendirir.
    /// Serial DataReceived event'inden çağrılır — worker thread güvenli.
    /// UI subscriber'ları event'lere Control.BeginInvoke ile erişmelidir.
    /// </summary>
    public static class LoRaPaketCozumleyici
    {
        private static readonly StringBuilder buffer = new StringBuilder();
        private static readonly object bufferLock = new object();

        public static uint ToplamPaket { get; private set; }
        public static uint BasariliPaket { get; private set; }
        public static uint HataliPaket { get; private set; }
        public static uint BilinmeyenTipPaket { get; private set; }

        public static event Action<BasincTestiVerisi> BasincTestiVerisiAlindi;
        public static event Action<HaberlesmeTestiVerisi> HaberlesmeTestiVerisiAlindi;
        public static event Action<JiroskopTestiVerisi> JiroskopTestiVerisiAlindi;
        public static event Action<AyrilmaAlgoritmaTestiVerisi> AyrilmaAlgoritmaTestiVerisiAlindi;

        /// <summary>Serial porttan okunan ham veriyi parser'a verir (partial satırları buffer'lar).</summary>
        public static void GelenVeriyiIsle(string gelenVeri)
        {
            if (string.IsNullOrEmpty(gelenVeri)) return;

            string tamSatirlar;
            lock (bufferLock)
            {
                buffer.Append(gelenVeri);
                string icerik = buffer.ToString();
                int sonNewline = icerik.LastIndexOf('\n');
                if (sonNewline == -1) return;

                tamSatirlar = icerik.Substring(0, sonNewline);
                buffer.Clear();
                if (sonNewline + 1 < icerik.Length)
                    buffer.Append(icerik.Substring(sonNewline + 1));
            }

            foreach (string satir in tamSatirlar.Split('\n'))
            {
                string trimmed = satir.Trim();
                if (string.IsNullOrEmpty(trimmed)) continue;
                IslePaket(trimmed);
            }
        }

        private static void IslePaket(string satir)
        {
            ToplamPaket++;
            try
            {
                string[] alanlar = satir.Split(',');
                if (alanlar.Length < 1) { HataliPaket++; return; }

                if (!byte.TryParse(alanlar[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out byte testIdByte))
                {
                    HataliPaket++;
                    Debug.WriteLine($"[LoRa] Paket ID parse hatasi: '{alanlar[0]}'");
                    return;
                }

                switch ((TestTipi)testIdByte)
                {
                    case TestTipi.BasincTesti: BasincTestiPaketiniIsle(alanlar); break;
                    case TestTipi.HaberlesmeTesti: HaberlesmeTestiPaketiniIsle(alanlar); break;
                    case TestTipi.JiroskopTesti: JiroskopTestiPaketiniIsle(alanlar); break;
                    case TestTipi.AyrilmaAlgoritmaTesti: AyrilmaAlgoritmaTestiPaketiniIsle(alanlar); break;
                    default:
                        BilinmeyenTipPaket++;
                        Debug.WriteLine($"[LoRa] Bilinmeyen test tipi: 0x{testIdByte:X2}");
                        break;
                }
            }
            catch (Exception ex)
            {
                HataliPaket++;
                Debug.WriteLine("[LoRa] Paket islem hatasi: " + ex.Message);
            }
        }

        // Basınç: [0]=1 [1]paketSayaci [2]arduinoMs [3]sicaklik [4]basinc [5]nem [6]irtifa [7]goreceliIrtifa [8]ayrilmaFlag  (9 alan)
        private static void BasincTestiPaketiniIsle(string[] alanlar)
        {
            if (alanlar.Length < 9) { HataliPaket++; Debug.WriteLine($"[LoRa] Basinc: eksik alan ({alanlar.Length}/9)"); return; }
            var ci = CultureInfo.InvariantCulture;
            if (!uint.TryParse(alanlar[1], NumberStyles.Integer, ci, out uint paketSayaci)) { HataliPaket++; return; }
            if (!uint.TryParse(alanlar[2], NumberStyles.Integer, ci, out uint arduinoMs)) { HataliPaket++; return; }
            if (!float.TryParse(alanlar[3], NumberStyles.Float, ci, out float sicaklik)) { HataliPaket++; return; }
            if (!float.TryParse(alanlar[4], NumberStyles.Float, ci, out float basinc)) { HataliPaket++; return; }
            if (!float.TryParse(alanlar[5], NumberStyles.Float, ci, out float nem)) { HataliPaket++; return; }
            if (!float.TryParse(alanlar[6], NumberStyles.Float, ci, out float irtifa)) { HataliPaket++; return; }
            if (!float.TryParse(alanlar[7], NumberStyles.Float, ci, out float goreceliIrtifa)) { HataliPaket++; return; }
            if (!int.TryParse(alanlar[8], NumberStyles.Integer, ci, out int ayrilmaRaw)) { HataliPaket++; return; }

            bool ayrilma = ayrilmaRaw != 0;
            VeriTabani.BasincTestiVeriEkle(paketSayaci, arduinoMs, sicaklik, basinc, nem, irtifa, goreceliIrtifa, ayrilma);
            TetikleGuvenli(BasincTestiVerisiAlindi, new BasincTestiVerisi
            {
                PaketSayaci = paketSayaci, ArduinoMs = arduinoMs, Sicaklik = sicaklik, Basinc = basinc,
                Nem = nem, Irtifa = irtifa, GoreceliIrtifa = goreceliIrtifa, AyrilmaDurumu = ayrilma
            });
            BasariliPaket++;
        }

        // Haberleşme: [0]=2 [1]paketSayaci [2]arduinoMs [3..5]gyroXYZ [6]roll [7]pitch [8]toplamAci [9]ledFlag  (10 alan)
        private static void HaberlesmeTestiPaketiniIsle(string[] alanlar)
        {
            if (alanlar.Length < 10) { HataliPaket++; Debug.WriteLine($"[LoRa] Haberlesme: eksik alan ({alanlar.Length}/10)"); return; }
            var ci = CultureInfo.InvariantCulture;
            if (!uint.TryParse(alanlar[1], NumberStyles.Integer, ci, out uint paketSayaci)) { HataliPaket++; return; }
            if (!uint.TryParse(alanlar[2], NumberStyles.Integer, ci, out uint arduinoMs)) { HataliPaket++; return; }
            if (!float.TryParse(alanlar[3], NumberStyles.Float, ci, out float gyroX)) { HataliPaket++; return; }
            if (!float.TryParse(alanlar[4], NumberStyles.Float, ci, out float gyroY)) { HataliPaket++; return; }
            if (!float.TryParse(alanlar[5], NumberStyles.Float, ci, out float gyroZ)) { HataliPaket++; return; }
            if (!float.TryParse(alanlar[6], NumberStyles.Float, ci, out float roll)) { HataliPaket++; return; }
            if (!float.TryParse(alanlar[7], NumberStyles.Float, ci, out float pitch)) { HataliPaket++; return; }
            if (!float.TryParse(alanlar[8], NumberStyles.Float, ci, out float toplamAci)) { HataliPaket++; return; }
            if (!int.TryParse(alanlar[9], NumberStyles.Integer, ci, out int ledRaw)) { HataliPaket++; return; }

            bool ledOn = ledRaw != 0;
            VeriTabani.HaberlesmeTestiVeriEkle(paketSayaci, arduinoMs, gyroX, gyroY, gyroZ, roll, pitch, toplamAci, ledOn);
            TetikleGuvenli(HaberlesmeTestiVerisiAlindi, new HaberlesmeTestiVerisi
            {
                PaketSayaci = paketSayaci, ArduinoMs = arduinoMs, GyroX = gyroX, GyroY = gyroY, GyroZ = gyroZ,
                Roll = roll, Pitch = pitch, ToplamAci = toplamAci, LedOn = ledOn
            });
            BasariliPaket++;
        }

        // Jiroskop: [0]=3 [1]paketSayaci [2]arduinoMs [3..5]jiroskopXYZ [6]aci [7]egimFlag  (8 alan)
        private static void JiroskopTestiPaketiniIsle(string[] alanlar)
        {
            if (alanlar.Length < 8) { HataliPaket++; Debug.WriteLine($"[LoRa] Jiroskop: eksik alan ({alanlar.Length}/8)"); return; }
            var ci = CultureInfo.InvariantCulture;
            if (!uint.TryParse(alanlar[1], NumberStyles.Integer, ci, out uint paketSayaci)) { HataliPaket++; return; }
            if (!uint.TryParse(alanlar[2], NumberStyles.Integer, ci, out uint arduinoMs)) { HataliPaket++; return; }
            if (!float.TryParse(alanlar[3], NumberStyles.Float, ci, out float jiroskopX)) { HataliPaket++; return; }
            if (!float.TryParse(alanlar[4], NumberStyles.Float, ci, out float jiroskopY)) { HataliPaket++; return; }
            if (!float.TryParse(alanlar[5], NumberStyles.Float, ci, out float jiroskopZ)) { HataliPaket++; return; }
            if (!float.TryParse(alanlar[6], NumberStyles.Float, ci, out float aci)) { HataliPaket++; return; }
            if (!int.TryParse(alanlar[7], NumberStyles.Integer, ci, out int egimRaw)) { HataliPaket++; return; }

            bool egim = egimRaw != 0;
            VeriTabani.JiroskopTestiVeriEkle(paketSayaci, arduinoMs, jiroskopX, jiroskopY, jiroskopZ, aci, egim);
            TetikleGuvenli(JiroskopTestiVerisiAlindi, new JiroskopTestiVerisi
            {
                PaketSayaci = paketSayaci, ArduinoMs = arduinoMs,
                JiroskopX = jiroskopX, JiroskopY = jiroskopY, JiroskopZ = jiroskopZ, Aci = aci, EgimTetiklendi = egim
            });
            BasariliPaket++;
        }

        // Ayrılma: [0]=4 [1]ps [2]ms [3]sicaklik [4]basinc [5]nem [6]irtifa [7..9]jiroskopXYZ [10]aci [11]basincKosulu [12]egimKosulu [13]ayrilma  (14 alan)
        private static void AyrilmaAlgoritmaTestiPaketiniIsle(string[] alanlar)
        {
            if (alanlar.Length < 14) { HataliPaket++; Debug.WriteLine($"[LoRa] Ayrilma: eksik alan ({alanlar.Length}/14)"); return; }
            var ci = CultureInfo.InvariantCulture;
            if (!uint.TryParse(alanlar[1], NumberStyles.Integer, ci, out uint paketSayaci)) { HataliPaket++; return; }
            if (!uint.TryParse(alanlar[2], NumberStyles.Integer, ci, out uint arduinoMs)) { HataliPaket++; return; }
            if (!float.TryParse(alanlar[3], NumberStyles.Float, ci, out float sicaklik)) { HataliPaket++; return; }
            if (!float.TryParse(alanlar[4], NumberStyles.Float, ci, out float basinc)) { HataliPaket++; return; }
            if (!float.TryParse(alanlar[5], NumberStyles.Float, ci, out float nem)) { HataliPaket++; return; }
            if (!float.TryParse(alanlar[6], NumberStyles.Float, ci, out float irtifa)) { HataliPaket++; return; }
            if (!float.TryParse(alanlar[7], NumberStyles.Float, ci, out float jiroskopX)) { HataliPaket++; return; }
            if (!float.TryParse(alanlar[8], NumberStyles.Float, ci, out float jiroskopY)) { HataliPaket++; return; }
            if (!float.TryParse(alanlar[9], NumberStyles.Float, ci, out float jiroskopZ)) { HataliPaket++; return; }
            if (!float.TryParse(alanlar[10], NumberStyles.Float, ci, out float aci)) { HataliPaket++; return; }
            if (!int.TryParse(alanlar[11], NumberStyles.Integer, ci, out int basincKosRaw)) { HataliPaket++; return; }
            if (!int.TryParse(alanlar[12], NumberStyles.Integer, ci, out int egimKosRaw)) { HataliPaket++; return; }
            if (!int.TryParse(alanlar[13], NumberStyles.Integer, ci, out int ayrilmaRaw)) { HataliPaket++; return; }

            bool basincKosulu = basincKosRaw != 0, egimKosulu = egimKosRaw != 0, ayrilma = ayrilmaRaw != 0;
            VeriTabani.AyrilmaAlgoritmaTestiVeriEkle(paketSayaci, arduinoMs, sicaklik, basinc, nem, irtifa,
                jiroskopX, jiroskopY, jiroskopZ, aci, basincKosulu, egimKosulu, ayrilma);
            TetikleGuvenli(AyrilmaAlgoritmaTestiVerisiAlindi, new AyrilmaAlgoritmaTestiVerisi
            {
                PaketSayaci = paketSayaci, ArduinoMs = arduinoMs, Sicaklik = sicaklik, Basinc = basinc, Nem = nem, Irtifa = irtifa,
                JiroskopX = jiroskopX, JiroskopY = jiroskopY, JiroskopZ = jiroskopZ, Aci = aci,
                BasincKosulu = basincKosulu, EgimKosulu = egimKosulu, AyrilmaDurumu = ayrilma
            });
            BasariliPaket++;
        }

        private static void TetikleGuvenli<T>(Action<T> handler, T veri)
        {
            try { handler?.Invoke(veri); }
            catch (Exception ex) { Debug.WriteLine("[LoRa] Subscriber hatasi: " + ex.Message); }
        }
    }
}
