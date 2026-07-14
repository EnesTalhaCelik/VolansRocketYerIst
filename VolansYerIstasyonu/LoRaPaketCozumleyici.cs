using System;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using VolansYerIstasyonu.UserControls;

namespace VolansYerIstasyonu
{
    /// <summary>
    /// LoRa üzerinden gelen ham veriyi satırlara ayırır, her satırın
    /// başındaki TEST_ID byte'ına göre uygun handler'a yönlendirir.
    ///
    /// LoRa DataReceived event'inden çağrılması bekleniyor — worker thread güvenli.
    /// </summary>
    public static class LoRaPaketCozumleyici
    {
        // ----- Buffer (paket tam gelmemişse kalan parçayı tutar) -----
        private static readonly StringBuilder buffer = new StringBuilder();
        private static readonly object bufferLock = new object();

        // ----- İstatistikler -----
        public static uint ToplamPaket { get; private set; }
        public static uint BasariliPaket { get; private set; }
        public static uint HataliPaket { get; private set; }
        public static uint BilinmeyenTipPaket { get; private set; }

        // ----- Event'ler (UI subscriber'lar için) -----
        /// <summary>
        /// Başarılı bir basınç testi paketi çözümlendiğinde tetiklenir.
        /// DİKKAT: Event worker thread'de tetiklenir. Subscriber'lar UI
        /// kontrollerine erişmek için Control.BeginInvoke kullanmalıdır.
        /// </summary>
        public static event Action<BasincTestiVerisi> BasincTestiVerisiAlindi;

        /// <summary>
        /// Başarılı bir haberleşme testi paketi çözümlendiğinde tetiklenir.
        /// DİKKAT: Worker thread'de tetiklenir; UI'ya BeginInvoke ile geçilmeli.
        /// </summary>
        public static event Action<HaberlesmeTestiVerisi> HaberlesmeTestiVerisiAlindi;

        /// <summary>
        /// Başarılı bir jiroskop testi paketi çözümlendiğinde tetiklenir.
        /// DİKKAT: Worker thread'de tetiklenir; UI'ya BeginInvoke ile geçilmeli.
        /// </summary>
        public static event Action<JiroskopTestiVerisi> JiroskopTestiVerisiAlindi;

        /// <summary>
        /// Başarılı bir Ayrılma Algoritma testi paketi çözümlendiğinde tetiklenir.
        /// DİKKAT: Worker thread'de tetiklenir; UI'ya BeginInvoke ile geçilmeli.
        /// </summary>
        public static event Action<AyrilmaAlgoritmaTestiVerisi> AyrilmaAlgoritmaTestiVerisiAlindi;

        /// <summary>
        /// LoRa serial portundan okunan ham veriyi parser'a verir.
        /// Veri tam satır olmayabilir; parser buffer'lar ve \n geldikçe işler.
        /// </summary>
        public static void GelenVeriyiIsle(string gelenVeri)
        {
            if (string.IsNullOrEmpty(gelenVeri)) return;

            string tamSatirlar;
            lock (bufferLock)
            {
                buffer.Append(gelenVeri);
                string icerik = buffer.ToString();
                int sonNewline = icerik.LastIndexOf('\n');
                if (sonNewline == -1) return; // Henüz tam satır yok, sonraki paketi bekle

                tamSatirlar = icerik.Substring(0, sonNewline);
                buffer.Clear();
                if (sonNewline + 1 < icerik.Length)
                {
                    buffer.Append(icerik.Substring(sonNewline + 1));
                }
            }

            foreach (string satir in tamSatirlar.Split('\n'))
            {
                string trimmed = satir.Trim();
                if (string.IsNullOrEmpty(trimmed)) continue;
                IslePaket(trimmed);
            }
        }

        /// <summary>
        /// Tek bir CSV satırını parse eder ve doğru test handler'ına yönlendirir.
        /// </summary>
        private static void IslePaket(string satir)
        {
            ToplamPaket++;
            try
            {
                string[] alanlar = satir.Split(',');
                if (alanlar.Length < 1)
                {
                    HataliPaket++;
                    return;
                }

                if (!byte.TryParse(alanlar[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out byte testIdByte))
                {
                    HataliPaket++;
                    Debug.WriteLine($"[LoRa] Paket ID parse hatasi: '{alanlar[0]}'");
                    return;
                }

                TestTipi tip = (TestTipi)testIdByte;

                // -----------------------------------------------------------
                // YÖNLENDİRME: yeni test eklediğinizde buraya case ekleyin
                // -----------------------------------------------------------
                switch (tip)
                {
                    case TestTipi.BasincTesti:
                        BasincTestiPaketiniIsle(alanlar);
                        break;

                    case TestTipi.HaberlesmeTesti:
                        HaberlesmeTestiPaketiniIsle(alanlar);
                        break;

                    case TestTipi.JiroskopTesti:
                        JiroskopTestiPaketiniIsle(alanlar);
                        break;

                    case TestTipi.AyrilmaAlgoritmaTesti:
                        AyrilmaAlgoritmaTestiPaketiniIsle(alanlar);
                        break;

                    // case TestTipi.AciTesti:
                    //     AciTestiPaketiniIsle(alanlar);
                    //     break;

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

        // =====================================================================
        // BASINÇ TESTİ - Tek paket çözümlemesi
        //
        // Beklenen format (CSV, \n ile biten):
        //   [0] testId         = 1
        //   [1] paketSayaci    (uint)
        //   [2] arduinoMs      (uint, Arduino millis())
        //   [3] sicaklik       (float, °C)
        //   [4] basinc         (float, hPa)
        //   [5] nem            (float, %)
        //   [6] irtifa         (float, m, LPF filtreli mutlak)
        //   [7] goreceliIrtifa (float, m, kalibrasyona göre)
        //   [8] ayrilmaFlag    (int, 0 veya 1)
        //
        // Toplam 9 alan beklenir. Az ise paket bozuk sayılır.
        // =====================================================================
        private static void BasincTestiPaketiniIsle(string[] alanlar)
        {
            if (alanlar.Length < 9)
            {
                HataliPaket++;
                Debug.WriteLine($"[LoRa] Basinc testi: eksik alan ({alanlar.Length}/9)");
                return;
            }

            var culture = CultureInfo.InvariantCulture;

            if (!uint.TryParse(alanlar[1], NumberStyles.Integer, culture, out uint paketSayaci)) { HataliPaket++; return; }
            if (!uint.TryParse(alanlar[2], NumberStyles.Integer, culture, out uint arduinoMs)) { HataliPaket++; return; }
            if (!float.TryParse(alanlar[3], NumberStyles.Float, culture, out float sicaklik)) { HataliPaket++; return; }
            if (!float.TryParse(alanlar[4], NumberStyles.Float, culture, out float basinc)) { HataliPaket++; return; }
            if (!float.TryParse(alanlar[5], NumberStyles.Float, culture, out float nem)) { HataliPaket++; return; }
            if (!float.TryParse(alanlar[6], NumberStyles.Float, culture, out float irtifa)) { HataliPaket++; return; }
            if (!float.TryParse(alanlar[7], NumberStyles.Float, culture, out float goreceliIrtifa)) { HataliPaket++; return; }
            if (!int.TryParse(alanlar[8], NumberStyles.Integer, culture, out int ayrilmaRaw)) { HataliPaket++; return; }

            bool ayrilmaDurumu = ayrilmaRaw != 0;

            // 1) DB'ye yaz
            uc_VeriTablo.basincTestiVeriEkle(
                paketSayaci,
                arduinoMs,
                sicaklik,
                basinc,
                nem,
                irtifa,
                goreceliIrtifa,
                ayrilmaDurumu);

            // 2) UI subscriber'lara haber ver (ana ekran label'ları için)
            //    Subscriber yoksa null koşulu sayesinde sessizce geçer.
            var veri = new BasincTestiVerisi
            {
                PaketSayaci = paketSayaci,
                ArduinoMs = arduinoMs,
                Sicaklik = sicaklik,
                Basinc = basinc,
                Nem = nem,
                Irtifa = irtifa,
                GoreceliIrtifa = goreceliIrtifa,
                AyrilmaDurumu = ayrilmaDurumu
            };
            try
            {
                BasincTestiVerisiAlindi?.Invoke(veri);
            }
            catch (Exception ex)
            {
                // Subscriber içinde patlama olursa parser akışı bozulmasın
                Debug.WriteLine("[LoRa] BasincTestiVerisiAlindi subscriber hatasi: " + ex.Message);
            }

            BasariliPaket++;
        }

        // =====================================================================
        // HABERLEŞME TESTİ - Tek paket çözümlemesi
        //
        // Beklenen format (CSV, \n ile biten):
        //   [0] testId      = 2
        //   [1] paketSayaci (uint)
        //   [2] arduinoMs   (uint, Arduino millis())
        //   [3] sicaklik    (float, °C)
        //   [4] basinc      (float, hPa)
        //   [5] gpsEnlem    (double, derece, örn. 40.7166)
        //   [6] gpsBoylam   (double, derece, örn. 31.5247)
        //   [7] gpsIrtifa   (float, m, GPS yüksekliği)
        //
        // Toplam 8 alan beklenir.
        // =====================================================================
        private static void HaberlesmeTestiPaketiniIsle(string[] alanlar)
        {
            if (alanlar.Length < 10)
            {
                HataliPaket++;
                Debug.WriteLine($"[LoRa] Haberlesme testi: eksik alan ({alanlar.Length}/10)");
                return;
            }

            var culture = CultureInfo.InvariantCulture;

            if (!uint.TryParse(alanlar[1], NumberStyles.Integer, culture, out uint paketSayaci)) { HataliPaket++; return; }
            if (!uint.TryParse(alanlar[2], NumberStyles.Integer, culture, out uint arduinoMs)) { HataliPaket++; return; }
            if (!float.TryParse(alanlar[3], NumberStyles.Float, culture, out float gyroX)) { HataliPaket++; return; }
            if (!float.TryParse(alanlar[4], NumberStyles.Float, culture, out float gyroY)) { HataliPaket++; return; }
            if (!float.TryParse(alanlar[5], NumberStyles.Float, culture, out float gyroZ)) { HataliPaket++; return; }
            if (!float.TryParse(alanlar[6], NumberStyles.Float, culture, out float roll)) { HataliPaket++; return; }
            if (!float.TryParse(alanlar[7], NumberStyles.Float, culture, out float pitch)) { HataliPaket++; return; }
            if (!float.TryParse(alanlar[8], NumberStyles.Float, culture, out float toplamAci)) { HataliPaket++; return; }
            if (!int.TryParse(alanlar[9], NumberStyles.Integer, culture, out int ledRaw)) { HataliPaket++; return; }

            bool ledOn = ledRaw != 0;

            // DB'ye yaz
            uc_VeriTablo.haberlesmeTestiVeriEkle(
                paketSayaci, arduinoMs, gyroX, gyroY, gyroZ, roll, pitch, toplamAci, ledOn);

            // UI subscriber'lara haber ver
            var veri = new HaberlesmeTestiVerisi
            {
                PaketSayaci = paketSayaci,
                ArduinoMs = arduinoMs,
                GyroX = gyroX,
                GyroY = gyroY,
                GyroZ = gyroZ,
                Roll = roll,
                Pitch = pitch,
                ToplamAci = toplamAci,
                LedOn = ledOn
            };
            try
            {
                HaberlesmeTestiVerisiAlindi?.Invoke(veri);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[LoRa] HaberlesmeTestiVerisiAlindi subscriber hatasi: " + ex.Message);
            }

            BasariliPaket++;
        }

        // =====================================================================
        // JİROSKOP TESTİ - Tek paket çözümlemesi
        //
        // Beklenen format (CSV, \n ile biten):
        //   [0] testId         = 3
        //   [1] paketSayaci    (uint)
        //   [2] arduinoMs      (uint, Arduino millis())
        //   [3] jiroskopX      (float, °/s)
        //   [4] jiroskopY      (float, °/s)
        //   [5] jiroskopZ      (float, °/s)
        //   [6] aci            (float, °, yer normali ile yapılan açı)
        //   [7] egimTetiklendi (int, 0 veya 1)
        //
        // Toplam 8 alan beklenir.
        //
        // NOT: Eğim eşik değeri ESP32 tarafında karar verilmektedir. Yer
        // istasyonu sadece "tetiklendi mi?" flag'ini okur. Eşiği
        // değiştirmek için yer istasyonu kodu güncellenmesine gerek yoktur.
        // =====================================================================
        private static void JiroskopTestiPaketiniIsle(string[] alanlar)
        {
            if (alanlar.Length < 8)
            {
                HataliPaket++;
                Debug.WriteLine($"[LoRa] Jiroskop testi: eksik alan ({alanlar.Length}/8)");
                return;
            }

            var culture = CultureInfo.InvariantCulture;

            if (!uint.TryParse(alanlar[1], NumberStyles.Integer, culture, out uint paketSayaci)) { HataliPaket++; return; }
            if (!uint.TryParse(alanlar[2], NumberStyles.Integer, culture, out uint arduinoMs)) { HataliPaket++; return; }
            if (!float.TryParse(alanlar[3], NumberStyles.Float, culture, out float jiroskopX)) { HataliPaket++; return; }
            if (!float.TryParse(alanlar[4], NumberStyles.Float, culture, out float jiroskopY)) { HataliPaket++; return; }
            if (!float.TryParse(alanlar[5], NumberStyles.Float, culture, out float jiroskopZ)) { HataliPaket++; return; }
            if (!float.TryParse(alanlar[6], NumberStyles.Float, culture, out float aci)) { HataliPaket++; return; }
            if (!int.TryParse(alanlar[7], NumberStyles.Integer, culture, out int egimRaw)) { HataliPaket++; return; }

            bool egimTetiklendi = egimRaw != 0;

            // 1) DB'ye yaz
            uc_VeriTablo.jiroskopTestiVeriEkle(
                paketSayaci, arduinoMs, jiroskopX, jiroskopY, jiroskopZ, aci, egimTetiklendi);

            // 2) UI subscriber'lara haber ver

            var veri = new JiroskopTestiVerisi
            {
                PaketSayaci = paketSayaci,
                ArduinoMs = arduinoMs,
                JiroskopX = jiroskopX,
                JiroskopY = jiroskopY,
                JiroskopZ = jiroskopZ,
                Aci = aci,
                EgimTetiklendi = egimTetiklendi
            };
            try
            {
                JiroskopTestiVerisiAlindi?.Invoke(veri);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[LoRa] JiroskopTestiVerisiAlindi subscriber hatasi: " + ex.Message);
            }

            BasariliPaket++;
        }

        // =====================================================================
        // AYRILMA ALGORİTMA TESTİ - Tek paket çözümlemesi
        //
        // BME280 + jiroskop verilerini bir arada gönderen test paketi.
        // "Eğim VE irtifa kaybı" tabanlı iki-kriterli ayrılma algoritmasının
        // test edilmesi için kullanılır (şartname: "Özgün UKB Algoritma Testi").
        //
        // Beklenen format (CSV, \n ile biten) - 14 alan:
        //   [0]  testId         = 4
        //   [1]  paketSayaci    (uint)
        //   [2]  arduinoMs      (uint)
        //   [3]  sicaklik       (float, °C)
        //   [4]  basinc         (float, hPa)
        //   [5]  nem            (float, %)
        //   [6]  irtifa         (float, m - göreceli)
        //   [7]  jiroskopX      (float, °/s)
        //   [8]  jiroskopY      (float, °/s)
        //   [9]  jiroskopZ      (float, °/s)
        //   [10] aci            (float, °)
        //   [11] basincKosulu   (int, 0/1)
        //   [12] egimKosulu     (int, 0/1)
        //   [13] ayrilmaDurumu  (int, 0/1) - ESP32'nin nihai kararı
        // =====================================================================
        private static void AyrilmaAlgoritmaTestiPaketiniIsle(string[] alanlar)
        {
            if (alanlar.Length < 14)
            {
                HataliPaket++;
                Debug.WriteLine($"[LoRa] Ayrilma algoritma testi: eksik alan ({alanlar.Length}/14)");
                return;
            }

            var culture = CultureInfo.InvariantCulture;

            if (!uint.TryParse(alanlar[1], NumberStyles.Integer, culture, out uint paketSayaci)) { HataliPaket++; return; }
            if (!uint.TryParse(alanlar[2], NumberStyles.Integer, culture, out uint arduinoMs)) { HataliPaket++; return; }
            if (!float.TryParse(alanlar[3], NumberStyles.Float, culture, out float sicaklik)) { HataliPaket++; return; }
            if (!float.TryParse(alanlar[4], NumberStyles.Float, culture, out float basinc)) { HataliPaket++; return; }
            if (!float.TryParse(alanlar[5], NumberStyles.Float, culture, out float nem)) { HataliPaket++; return; }
            if (!float.TryParse(alanlar[6], NumberStyles.Float, culture, out float irtifa)) { HataliPaket++; return; }
            if (!float.TryParse(alanlar[7], NumberStyles.Float, culture, out float jiroskopX)) { HataliPaket++; return; }
            if (!float.TryParse(alanlar[8], NumberStyles.Float, culture, out float jiroskopY)) { HataliPaket++; return; }
            if (!float.TryParse(alanlar[9], NumberStyles.Float, culture, out float jiroskopZ)) { HataliPaket++; return; }
            if (!float.TryParse(alanlar[10], NumberStyles.Float, culture, out float aci)) { HataliPaket++; return; }
            if (!int.TryParse(alanlar[11], NumberStyles.Integer, culture, out int basincKosRaw)) { HataliPaket++; return; }
            if (!int.TryParse(alanlar[12], NumberStyles.Integer, culture, out int egimKosRaw)) { HataliPaket++; return; }
            if (!int.TryParse(alanlar[13], NumberStyles.Integer, culture, out int ayrilmaRaw)) { HataliPaket++; return; }

            bool basincKosulu = basincKosRaw != 0;
            bool egimKosulu = egimKosRaw != 0;
            bool ayrilmaDurumu = ayrilmaRaw != 0;

            // 1) DB'ye yaz
            uc_VeriTablo.ayrilmaAlgoritmaTestiVeriEkle(
                paketSayaci, arduinoMs,
                sicaklik, basinc, nem, irtifa,
                jiroskopX, jiroskopY, jiroskopZ, aci,
                basincKosulu, egimKosulu, ayrilmaDurumu);

            // 2) UI subscriber'lara haber ver
            var veri = new AyrilmaAlgoritmaTestiVerisi
            {
                PaketSayaci = paketSayaci,
                ArduinoMs = arduinoMs,
                Sicaklik = sicaklik,
                Basinc = basinc,
                Nem = nem,
                Irtifa = irtifa,
                JiroskopX = jiroskopX,
                JiroskopY = jiroskopY,
                JiroskopZ = jiroskopZ,
                Aci = aci,
                BasincKosulu = basincKosulu,
                EgimKosulu = egimKosulu,
                AyrilmaDurumu = ayrilmaDurumu
            };
            try
            {
                AyrilmaAlgoritmaTestiVerisiAlindi?.Invoke(veri);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[LoRa] AyrilmaAlgoritmaTestiVerisiAlindi subscriber hatasi: " + ex.Message);
            }

            BasariliPaket++;
        }
    }
}