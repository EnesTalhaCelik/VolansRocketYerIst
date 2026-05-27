using System.Collections.Generic;
using System.IO;

namespace VolansYerIstasyonu
{
    /// <summary>
    /// LoRa paketlerinin başına eklenen test ID byte'ı.
    /// Arduino'nun gönderdiği CSV satırının ilk alanı bu değerle eşleşir.
    /// 
    /// YENİ TEST EKLEMEK İÇİN:
    ///   1. Burada yeni bir enum değeri ekleyin (sıradaki uint8 değeri).
    ///   2. TestTanimlari.Tanimlar dictionary'sine yeni bir satır ekleyin.
    ///   3. uc_VeriTablo.cs içinde {testAdi}DatabaseHazirla() ve
    ///      {testAdi}VeriEkle() metotlarını yazın (basincTesti örneğini taklit edin).
    ///   4. LoRaPaketCozumleyici.IslePaket() switch'ine case ekleyin.
    /// </summary>
    public enum TestTipi : byte
    {
        Bilinmeyen = 0x00,
        BasincTesti = 0x01,
        HaberlesmeTesti = 0x02,
        JiroskopTesti = 0x03,
        AyrilmaAlgoritmaTesti = 0x04,
        // GELECEKTE EKLENECEK ÖRNEKLER:
        // IrtifaTesti          = 0x05,
        // TamUcus              = 0x06,
    }

    /// <summary>
    /// Tek bir test tipinin metadata'sı.
    /// </summary>
    public class TestTipiTanimi
    {
        /// <summary>Dosya adının başlangıcı, örn. "Test-Basinc"</summary>
        public string DosyaAdiOnEki { get; set; }

        /// <summary>SQLite tablosunun adı, örn. "BasincTestiTablosu"</summary>
        public string TabloAdi { get; set; }

        /// <summary>UI'de gösterilecek okunabilir ad, örn. "Basınç Testi"</summary>
        public string Aciklama { get; set; }
    }

    /// <summary>
    /// Tüm test tiplerinin metadata kayıt defteri.
    /// Yeni test eklerken sadece buraya ekleme yapın.
    /// </summary>
    public static class TestTanimlari
    {
        public static readonly Dictionary<TestTipi, TestTipiTanimi> Tanimlar
            = new Dictionary<TestTipi, TestTipiTanimi>
        {
            {
                TestTipi.BasincTesti, new TestTipiTanimi
                {
                    DosyaAdiOnEki = "Test-Basinc",
                    TabloAdi      = "BasincTestiTablosu",
                    Aciklama      = "Basınç Testi"
                }
            },
            {
                TestTipi.HaberlesmeTesti, new TestTipiTanimi
                {
                    DosyaAdiOnEki = "Test-Haberlesme",
                    TabloAdi      = "HaberlesmeTestiTablosu",
                    Aciklama      = "Haberleşme Testi"
                }
            },
            {
                TestTipi.JiroskopTesti, new TestTipiTanimi
                {
                    DosyaAdiOnEki = "Test-Jiroskop",
                    TabloAdi      = "JiroskopTestiTablosu",
                    Aciklama      = "Jiroskop Testi"
                }
            },
            {
                TestTipi.AyrilmaAlgoritmaTesti, new TestTipiTanimi
                {
                    DosyaAdiOnEki = "Test-AyrilmaAlgoritma",
                    TabloAdi      = "AyrilmaAlgoritmaTestiTablosu",
                    Aciklama      = "Ayrılma Algoritma Testi"
                }
            },
            // {
            //     TestTipi.AciTesti, new TestTipiTanimi
            //     {
            //         DosyaAdiOnEki = "Test-Aci",
            //         TabloAdi      = "AciTestiTablosu",
            //         Aciklama      = "Açı Testi"
            //     }
            // },
        };

        /// <summary>
        /// Bir .db dosyasının adından test tipini çıkarır.
        /// Örn: "Test-Basinc-20260526_193015.db" -> TestTipi.BasincTesti
        /// </summary>
        public static TestTipi DosyaAdindanTipiBul(string dosyaAdiYadaYolu)
        {
            string dosyaAdi = Path.GetFileNameWithoutExtension(dosyaAdiYadaYolu);
            foreach (var pair in Tanimlar)
            {
                if (dosyaAdi.StartsWith(pair.Value.DosyaAdiOnEki))
                    return pair.Key;
            }
            return TestTipi.Bilinmeyen;
        }

        /// <summary>
        /// Test verisi dosyalarını bulmak için kullanılacak glob pattern'i.
        /// "Test-*.db" gibi - tüm test türlerini kapsar.
        /// </summary>
        public const string TumTestDosyalariPattern = "Test-*.db";
    }

    /// <summary>
    /// Basınç testi paketinden çözümlenmiş tek bir veri kaydı.
    /// LoRaPaketCozumleyici, parse ettiği her başarılı basınç testi paketi
    /// için bir instance üretir ve event ile yayınlar.
    /// </summary>
    public class BasincTestiVerisi
    {
        public uint PaketSayaci { get; set; }
        public uint ArduinoMs { get; set; }
        public float Sicaklik { get; set; }
        public float Basinc { get; set; }
        public float Nem { get; set; }
        public float Irtifa { get; set; }
        public float GoreceliIrtifa { get; set; }
        public bool AyrilmaDurumu { get; set; }
    }

    /// <summary>
    /// Haberleşme testi paketinden çözümlenmiş tek bir veri kaydı.
    /// GPS, sıcaklık ve basınç verilerini içerir.
    /// </summary>
    public class HaberlesmeTestiVerisi
    {
        public uint PaketSayaci { get; set; }
        public uint ArduinoMs { get; set; }
        public float Sicaklik { get; set; }
        public float Basinc { get; set; }
        public double GpsEnlem { get; set; }
        public double GpsBoylam { get; set; }
        public float GpsIrtifa { get; set; }
    }

    /// <summary>
    /// Jiroskop testi paketinden çözümlenmiş tek bir veri kaydı.
    /// Jiroskop ham verileri (açısal hız, °/s) + hesaplanmış yer normali açısı +
    /// eğim eşiği ile ayrılma tetikleme durumunu içerir.
    /// </summary>
    public class JiroskopTestiVerisi
    {
        public uint PaketSayaci { get; set; }
        public uint ArduinoMs { get; set; }
        public float JiroskopX { get; set; }  // °/s
        public float JiroskopY { get; set; }  // °/s
        public float JiroskopZ { get; set; }  // °/s
        public float Aci { get; set; }  // ° (yer normali ile)
        public bool EgimTetiklendi { get; set; }
    }

    /// <summary>
    /// Ayrılma Algoritma testi paketinden çözümlenmiş tek bir veri kaydı.
    /// 
    /// BME280 + jiroskop verilerini bir arada içerir ve "iki kritere bağlı"
    /// ayrılma algoritmasının test edilmesini sağlar:
    /// - BasincKosulu: irtifa kaybı eşiği geçildi mi
    /// - EgimKosulu: açı eşiği geçildi mi
    /// - AyrilmaDurumu: ESP32'deki algoritmanın nihai kararı (genelde
    ///   iki koşulun AND'i, ama daha karmaşık olabilir)
    /// </summary>
    public class AyrilmaAlgoritmaTestiVerisi
    {
        public uint PaketSayaci { get; set; }
        public uint ArduinoMs { get; set; }
        // BME280
        public float Sicaklik { get; set; }  // °C
        public float Basinc { get; set; }  // hPa
        public float Nem { get; set; }  // %
        public float Irtifa { get; set; }  // m (göreceli)
        // Jiroskop + hesaplanmış açı
        public float JiroskopX { get; set; }  // °/s
        public float JiroskopY { get; set; }  // °/s
        public float JiroskopZ { get; set; }  // °/s
        public float Aci { get; set; }  // °
        // Karar flag'leri
        public bool BasincKosulu { get; set; }
        public bool EgimKosulu { get; set; }
        public bool AyrilmaDurumu { get; set; }
    }
}