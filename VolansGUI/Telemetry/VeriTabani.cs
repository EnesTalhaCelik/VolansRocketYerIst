using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;

namespace VolansGUI.Telemetry
{
    /// <summary>
    /// SQLite veri katmanı. Her aviyonik/test tipi için ayrı .db dosyası tutar.
    /// Insert metotları LoRa worker thread'inden çağrılabilir; bu yüzden burada
    /// MessageBox YOKTUR (hatalar Debug çıktısına yazılır). Her tip kendi lock'una sahip.
    /// </summary>
    public static class VeriTabani
    {
        private static string anaAviyonikConn;
        private static string yedekAviyonikConn;
        private static string gorevYukuConn;

        private static string basincTestiConn;
        private static readonly object basincTestiLock = new object();
        private static string haberlesmeTestiConn;
        private static readonly object haberlesmeTestiLock = new object();
        private static string jiroskopTestiConn;
        private static readonly object jiroskopTestiLock = new object();
        private static string ayrilmaAlgoritmaTestiConn;
        private static readonly object ayrilmaAlgoritmaTestiLock = new object();

        // =====================================================================
        // ANA AVİYONİK
        // =====================================================================
        public static string AnaAviyonikDatabaseOlustur()
        {
            string conn = string.Empty;
            try
            {
                string ad = $"VeriAnaAv-{DateTime.Now:yyyyMMdd_HHmmss}.db";
                string path = UcusVeriYoneticisi.DosyaYolu(ad);
                conn = $"Data Source={path};Version=3;";
                if (!File.Exists(path))
                {
                    SQLiteConnection.CreateFile(path);
                    using (var c = new SQLiteConnection(conn))
                    {
                        c.Open();
                        new SQLiteCommand(@"
                            CREATE TABLE IF NOT EXISTS [AnaAviyonikTablosu] (
                                durum INTEGER, roket_irtifa_basinc REAL, dogrulama_kodu INTEGER,
                                paket_sayaci INTEGER, roket_basinc REAL, roket_sicaklik REAL,
                                roket_hiz_x REAL, roket_hiz_y REAL, roket_hiz_z REAL,
                                roket_acisal_hiz_x REAL, roket_acisal_hiz_y REAL, roket_acisal_hiz_z REAL,
                                jiroskop_x REAL, jiroskop_y REAL, jiroskop_z REAL,
                                ivme_x REAL, ivme_y REAL, ivme_z REAL, aci REAL,
                                roket_gps_enlem REAL, roket_gps_boylam REAL, roket_gps_yukseklik REAL,
                                zaman TEXT)", c).ExecuteNonQuery();
                    }
                }
                anaAviyonikConn = conn;
            }
            catch (Exception ex) { Debug.WriteLine("[AnaAviyonik] DB olusturma hatasi: " + ex.Message); }
            return conn;
        }

        public static void AnaAviyonikVeriEkle(int durum, float roket_irtifa_basinc, int dogrulama_kodu, int paket_sayaci,
            float roket_basinc, float roket_sicaklik, float roket_hiz_x, float roket_hiz_y, float roket_hiz_z, float roket_acisal_hiz_x,
            float roket_acisal_hiz_y, float roket_acisal_hiz_z, float jiroskop_x, float jiroskop_y, float jiroskop_z, float ivme_x,
            float ivme_y, float ivme_z, float aci, double roket_gps_enlem, double roket_gps_boylam, float roket_gps_yukseklik)
        {
            if (string.IsNullOrEmpty(anaAviyonikConn)) return;
            try
            {
                using (var c = new SQLiteConnection(anaAviyonikConn))
                {
                    c.Open();
                    var cmd = new SQLiteCommand(@"INSERT INTO [AnaAviyonikTablosu]
                        (durum, roket_irtifa_basinc, dogrulama_kodu, paket_sayaci, roket_basinc, roket_sicaklik,
                         roket_hiz_x, roket_hiz_y, roket_hiz_z, roket_acisal_hiz_x, roket_acisal_hiz_y, roket_acisal_hiz_z,
                         jiroskop_x, jiroskop_y, jiroskop_z, ivme_x, ivme_y, ivme_z, aci,
                         roket_gps_enlem, roket_gps_boylam, roket_gps_yukseklik, zaman)
                        VALUES (@durum,@rib,@dk,@ps,@rb,@rs,@rhx,@rhy,@rhz,@rahx,@rahy,@rahz,
                         @jx,@jy,@jz,@ix,@iy,@iz,@aci,@enlem,@boylam,@yuk,@zaman)", c);
                    cmd.Parameters.AddWithValue("@durum", durum);
                    cmd.Parameters.AddWithValue("@rib", roket_irtifa_basinc);
                    cmd.Parameters.AddWithValue("@dk", dogrulama_kodu);
                    cmd.Parameters.AddWithValue("@ps", paket_sayaci);
                    cmd.Parameters.AddWithValue("@rb", roket_basinc);
                    cmd.Parameters.AddWithValue("@rs", roket_sicaklik);
                    cmd.Parameters.AddWithValue("@rhx", roket_hiz_x);
                    cmd.Parameters.AddWithValue("@rhy", roket_hiz_y);
                    cmd.Parameters.AddWithValue("@rhz", roket_hiz_z);
                    cmd.Parameters.AddWithValue("@rahx", roket_acisal_hiz_x);
                    cmd.Parameters.AddWithValue("@rahy", roket_acisal_hiz_y);
                    cmd.Parameters.AddWithValue("@rahz", roket_acisal_hiz_z);
                    cmd.Parameters.AddWithValue("@jx", jiroskop_x);
                    cmd.Parameters.AddWithValue("@jy", jiroskop_y);
                    cmd.Parameters.AddWithValue("@jz", jiroskop_z);
                    cmd.Parameters.AddWithValue("@ix", ivme_x);
                    cmd.Parameters.AddWithValue("@iy", ivme_y);
                    cmd.Parameters.AddWithValue("@iz", ivme_z);
                    cmd.Parameters.AddWithValue("@aci", aci);
                    cmd.Parameters.AddWithValue("@enlem", roket_gps_enlem);
                    cmd.Parameters.AddWithValue("@boylam", roket_gps_boylam);
                    cmd.Parameters.AddWithValue("@yuk", roket_gps_yukseklik);
                    cmd.Parameters.AddWithValue("@zaman", DateTime.Now);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex) { Debug.WriteLine("[AnaAviyonik] Veri ekleme hatasi: " + ex.Message); }
        }

        // =====================================================================
        // YEDEK AVİYONİK
        // =====================================================================
        public static string YedekAviyonikDatabaseOlustur()
        {
            string conn = string.Empty;
            try
            {
                string ad = $"VeriYedekAv-{DateTime.Now:yyyyMMdd_HHmmss}.db";
                string path = UcusVeriYoneticisi.DosyaYolu(ad);
                conn = $"Data Source={path};Version=3;";
                if (!File.Exists(path))
                {
                    SQLiteConnection.CreateFile(path);
                    using (var c = new SQLiteConnection(conn))
                    {
                        c.Open();
                        new SQLiteCommand(@"
                            CREATE TABLE IF NOT EXISTS [YedekAviyonikTablosu] (
                                durum INTEGER, roket_irtifa_basinc REAL, dogrulama_kodu INTEGER,
                                paket_sayaci INTEGER, roket_basinc REAL, roket_sicaklik REAL,
                                roket_hiz_x REAL, roket_hiz_y REAL, roket_hiz_z REAL,
                                roket_acisal_hiz_x REAL, roket_acisal_hiz_y REAL, roket_acisal_hiz_z REAL,
                                jiroskop_x REAL, jiroskop_y REAL, jiroskop_z REAL,
                                ivme_x REAL, ivme_y REAL, ivme_z REAL, aci REAL,
                                roket_gps_enlem REAL, roket_gps_boylam REAL, roket_gps_yukseklik REAL,
                                zaman TEXT)", c).ExecuteNonQuery();
                    }
                }
                yedekAviyonikConn = conn;
            }
            catch (Exception ex) { Debug.WriteLine("[YedekAviyonik] DB olusturma hatasi: " + ex.Message); }
            return conn;
        }

        // =====================================================================
        // GÖREV YÜKÜ
        // =====================================================================
        public static string GorevYukuDatabaseOlustur()
        {
            string conn = string.Empty;
            try
            {
                string ad = $"VeriGorevYuku-{DateTime.Now:yyyyMMdd_HHmmss}.db";
                string path = UcusVeriYoneticisi.DosyaYolu(ad);
                conn = $"Data Source={path};Version=3;";
                if (!File.Exists(path))
                {
                    SQLiteConnection.CreateFile(path);
                    using (var c = new SQLiteConnection(conn))
                    {
                        c.Open();
                        new SQLiteCommand(@"
                            CREATE TABLE IF NOT EXISTS [GorevYukuTablosu] (
                                gps_irtifa REAL, basinc_irtifa REAL, gps_enlem REAL, gps_boylam REAL,
                                geiger_veri REAL, paket_sayaci INTEGER, dogrulama_kodu INTEGER, zaman TEXT)", c).ExecuteNonQuery();
                    }
                }
                gorevYukuConn = conn;
            }
            catch (Exception ex) { Debug.WriteLine("[GorevYuku] DB olusturma hatasi: " + ex.Message); }
            return conn;
        }

        // =====================================================================
        // BASINÇ TESTİ
        // =====================================================================
        public static string BasincTestiDatabaseHazirla(bool yeniOlustur)
        {
            lock (basincTestiLock)
            {
                if (!yeniOlustur && !string.IsNullOrEmpty(basincTestiConn)) return basincTestiConn;
                try
                {
                    var tanim = TestTanimlari.Tanimlar[TestTipi.BasincTesti];
                    string path = UcusVeriYoneticisi.DosyaYolu($"{tanim.DosyaAdiOnEki}-{DateTime.Now:yyyyMMdd_HHmmss}.db");
                    string conn = $"Data Source={path};Version=3;";
                    if (!File.Exists(path))
                    {
                        SQLiteConnection.CreateFile(path);
                        using (var c = new SQLiteConnection(conn))
                        {
                            c.Open();
                            new SQLiteCommand($@"
                                CREATE TABLE IF NOT EXISTS [{tanim.TabloAdi}] (
                                    paket_sayaci INTEGER, arduino_ms INTEGER, sicaklik REAL, basinc REAL,
                                    nem REAL, irtifa REAL, goreceli_irtifa REAL, ayrilma_durumu INTEGER, zaman TEXT)", c).ExecuteNonQuery();
                        }
                    }
                    basincTestiConn = conn;
                    return conn;
                }
                catch (Exception ex) { Debug.WriteLine("[BasincTesti] DB hatasi: " + ex.Message); return string.Empty; }
            }
        }

        public static void BasincTestiVeriEkle(uint paketSayaci, uint arduinoMs, float sicaklik, float basinc,
            float nem, float irtifa, float goreceliIrtifa, bool ayrilmaDurumu)
        {
            string conn = BasincTestiDatabaseHazirla(false);
            if (string.IsNullOrEmpty(conn)) return;
            try
            {
                var tanim = TestTanimlari.Tanimlar[TestTipi.BasincTesti];
                using (var c = new SQLiteConnection(conn))
                {
                    c.Open();
                    var cmd = new SQLiteCommand($@"INSERT INTO [{tanim.TabloAdi}]
                        (paket_sayaci, arduino_ms, sicaklik, basinc, nem, irtifa, goreceli_irtifa, ayrilma_durumu, zaman)
                        VALUES (@ps,@ms,@s,@b,@n,@i,@gi,@a,@z)", c);
                    cmd.Parameters.AddWithValue("@ps", paketSayaci);
                    cmd.Parameters.AddWithValue("@ms", arduinoMs);
                    cmd.Parameters.AddWithValue("@s", sicaklik);
                    cmd.Parameters.AddWithValue("@b", basinc);
                    cmd.Parameters.AddWithValue("@n", nem);
                    cmd.Parameters.AddWithValue("@i", irtifa);
                    cmd.Parameters.AddWithValue("@gi", goreceliIrtifa);
                    cmd.Parameters.AddWithValue("@a", ayrilmaDurumu ? 1 : 0);
                    cmd.Parameters.AddWithValue("@z", DateTime.Now);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex) { Debug.WriteLine("[BasincTesti] Veri ekleme hatasi: " + ex.Message); }
        }

        // =====================================================================
        // HABERLEŞME TESTİ  (schema DÜZELTİLDİ: gyro/roll/pitch/led kolonları)
        // =====================================================================
        public static string HaberlesmeTestiDatabaseHazirla(bool yeniOlustur)
        {
            lock (haberlesmeTestiLock)
            {
                if (!yeniOlustur && !string.IsNullOrEmpty(haberlesmeTestiConn)) return haberlesmeTestiConn;
                try
                {
                    var tanim = TestTanimlari.Tanimlar[TestTipi.HaberlesmeTesti];
                    string path = UcusVeriYoneticisi.DosyaYolu($"{tanim.DosyaAdiOnEki}-{DateTime.Now:yyyyMMdd_HHmmss}.db");
                    string conn = $"Data Source={path};Version=3;";
                    if (!File.Exists(path))
                    {
                        SQLiteConnection.CreateFile(path);
                        using (var c = new SQLiteConnection(conn))
                        {
                            c.Open();
                            // ESKİ HATA: tablo gps_* kolonlarıyla oluşturuluyor ama INSERT gyro_* yazıyordu.
                            // Artık tablo INSERT ile eşleşiyor.
                            new SQLiteCommand($@"
                                CREATE TABLE IF NOT EXISTS [{tanim.TabloAdi}] (
                                    paket_sayaci INTEGER, arduino_ms INTEGER,
                                    gyro_x REAL, gyro_y REAL, gyro_z REAL,
                                    roll REAL, pitch REAL, toplam_aci REAL, led_on INTEGER, zaman TEXT)", c).ExecuteNonQuery();
                        }
                    }
                    haberlesmeTestiConn = conn;
                    return conn;
                }
                catch (Exception ex) { Debug.WriteLine("[HaberlesmeTesti] DB hatasi: " + ex.Message); return string.Empty; }
            }
        }

        public static void HaberlesmeTestiVeriEkle(uint paketSayaci, uint arduinoMs,
            float gyroX, float gyroY, float gyroZ, float roll, float pitch, float toplamAci, bool ledOn)
        {
            string conn = HaberlesmeTestiDatabaseHazirla(false);
            if (string.IsNullOrEmpty(conn)) return;
            try
            {
                var tanim = TestTanimlari.Tanimlar[TestTipi.HaberlesmeTesti];
                using (var c = new SQLiteConnection(conn))
                {
                    c.Open();
                    var cmd = new SQLiteCommand($@"INSERT INTO [{tanim.TabloAdi}]
                        (paket_sayaci, arduino_ms, gyro_x, gyro_y, gyro_z, roll, pitch, toplam_aci, led_on, zaman)
                        VALUES (@ps,@ms,@gx,@gy,@gz,@roll,@pitch,@ta,@led,@z)", c);
                    cmd.Parameters.AddWithValue("@ps", paketSayaci);
                    cmd.Parameters.AddWithValue("@ms", arduinoMs);
                    cmd.Parameters.AddWithValue("@gx", gyroX);
                    cmd.Parameters.AddWithValue("@gy", gyroY);
                    cmd.Parameters.AddWithValue("@gz", gyroZ);
                    cmd.Parameters.AddWithValue("@roll", roll);
                    cmd.Parameters.AddWithValue("@pitch", pitch);
                    cmd.Parameters.AddWithValue("@ta", toplamAci);
                    cmd.Parameters.AddWithValue("@led", ledOn ? 1 : 0);
                    cmd.Parameters.AddWithValue("@z", DateTime.Now);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex) { Debug.WriteLine("[HaberlesmeTesti] Veri ekleme hatasi: " + ex.Message); }
        }

        // =====================================================================
        // JİROSKOP TESTİ
        // =====================================================================
        public static string JiroskopTestiDatabaseHazirla(bool yeniOlustur)
        {
            lock (jiroskopTestiLock)
            {
                if (!yeniOlustur && !string.IsNullOrEmpty(jiroskopTestiConn)) return jiroskopTestiConn;
                try
                {
                    var tanim = TestTanimlari.Tanimlar[TestTipi.JiroskopTesti];
                    string path = UcusVeriYoneticisi.DosyaYolu($"{tanim.DosyaAdiOnEki}-{DateTime.Now:yyyyMMdd_HHmmss}.db");
                    string conn = $"Data Source={path};Version=3;";
                    if (!File.Exists(path))
                    {
                        SQLiteConnection.CreateFile(path);
                        using (var c = new SQLiteConnection(conn))
                        {
                            c.Open();
                            new SQLiteCommand($@"
                                CREATE TABLE IF NOT EXISTS [{tanim.TabloAdi}] (
                                    paket_sayaci INTEGER, arduino_ms INTEGER,
                                    jiroskop_x REAL, jiroskop_y REAL, jiroskop_z REAL,
                                    aci REAL, egim_tetiklendi INTEGER, zaman TEXT)", c).ExecuteNonQuery();
                        }
                    }
                    jiroskopTestiConn = conn;
                    return conn;
                }
                catch (Exception ex) { Debug.WriteLine("[JiroskopTesti] DB hatasi: " + ex.Message); return string.Empty; }
            }
        }

        public static void JiroskopTestiVeriEkle(uint paketSayaci, uint arduinoMs,
            float jiroskopX, float jiroskopY, float jiroskopZ, float aci, bool egimTetiklendi)
        {
            string conn = JiroskopTestiDatabaseHazirla(false);
            if (string.IsNullOrEmpty(conn)) return;
            try
            {
                var tanim = TestTanimlari.Tanimlar[TestTipi.JiroskopTesti];
                using (var c = new SQLiteConnection(conn))
                {
                    c.Open();
                    var cmd = new SQLiteCommand($@"INSERT INTO [{tanim.TabloAdi}]
                        (paket_sayaci, arduino_ms, jiroskop_x, jiroskop_y, jiroskop_z, aci, egim_tetiklendi, zaman)
                        VALUES (@ps,@ms,@jx,@jy,@jz,@aci,@egim,@z)", c);
                    cmd.Parameters.AddWithValue("@ps", paketSayaci);
                    cmd.Parameters.AddWithValue("@ms", arduinoMs);
                    cmd.Parameters.AddWithValue("@jx", jiroskopX);
                    cmd.Parameters.AddWithValue("@jy", jiroskopY);
                    cmd.Parameters.AddWithValue("@jz", jiroskopZ);
                    cmd.Parameters.AddWithValue("@aci", aci);
                    cmd.Parameters.AddWithValue("@egim", egimTetiklendi ? 1 : 0);
                    cmd.Parameters.AddWithValue("@z", DateTime.Now);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex) { Debug.WriteLine("[JiroskopTesti] Veri ekleme hatasi: " + ex.Message); }
        }

        // =====================================================================
        // AYRILMA ALGORİTMA TESTİ
        // =====================================================================
        public static string AyrilmaAlgoritmaTestiDatabaseHazirla(bool yeniOlustur)
        {
            lock (ayrilmaAlgoritmaTestiLock)
            {
                if (!yeniOlustur && !string.IsNullOrEmpty(ayrilmaAlgoritmaTestiConn)) return ayrilmaAlgoritmaTestiConn;
                try
                {
                    var tanim = TestTanimlari.Tanimlar[TestTipi.AyrilmaAlgoritmaTesti];
                    string path = UcusVeriYoneticisi.DosyaYolu($"{tanim.DosyaAdiOnEki}-{DateTime.Now:yyyyMMdd_HHmmss}.db");
                    string conn = $"Data Source={path};Version=3;";
                    if (!File.Exists(path))
                    {
                        SQLiteConnection.CreateFile(path);
                        using (var c = new SQLiteConnection(conn))
                        {
                            c.Open();
                            new SQLiteCommand($@"
                                CREATE TABLE IF NOT EXISTS [{tanim.TabloAdi}] (
                                    paket_sayaci INTEGER, arduino_ms INTEGER,
                                    sicaklik REAL, basinc REAL, nem REAL, irtifa REAL,
                                    jiroskop_x REAL, jiroskop_y REAL, jiroskop_z REAL, aci REAL,
                                    basinc_kosulu INTEGER, egim_kosulu INTEGER, ayrilma_durumu INTEGER, zaman TEXT)", c).ExecuteNonQuery();
                        }
                    }
                    ayrilmaAlgoritmaTestiConn = conn;
                    return conn;
                }
                catch (Exception ex) { Debug.WriteLine("[AyrilmaAlgoritmaTesti] DB hatasi: " + ex.Message); return string.Empty; }
            }
        }

        public static void AyrilmaAlgoritmaTestiVeriEkle(uint paketSayaci, uint arduinoMs,
            float sicaklik, float basinc, float nem, float irtifa,
            float jiroskopX, float jiroskopY, float jiroskopZ, float aci,
            bool basincKosulu, bool egimKosulu, bool ayrilmaDurumu)
        {
            string conn = AyrilmaAlgoritmaTestiDatabaseHazirla(false);
            if (string.IsNullOrEmpty(conn)) return;
            try
            {
                var tanim = TestTanimlari.Tanimlar[TestTipi.AyrilmaAlgoritmaTesti];
                using (var c = new SQLiteConnection(conn))
                {
                    c.Open();
                    var cmd = new SQLiteCommand($@"INSERT INTO [{tanim.TabloAdi}]
                        (paket_sayaci, arduino_ms, sicaklik, basinc, nem, irtifa,
                         jiroskop_x, jiroskop_y, jiroskop_z, aci, basinc_kosulu, egim_kosulu, ayrilma_durumu, zaman)
                        VALUES (@ps,@ms,@s,@b,@n,@i,@jx,@jy,@jz,@aci,@bk,@ek,@ad,@z)", c);
                    cmd.Parameters.AddWithValue("@ps", paketSayaci);
                    cmd.Parameters.AddWithValue("@ms", arduinoMs);
                    cmd.Parameters.AddWithValue("@s", sicaklik);
                    cmd.Parameters.AddWithValue("@b", basinc);
                    cmd.Parameters.AddWithValue("@n", nem);
                    cmd.Parameters.AddWithValue("@i", irtifa);
                    cmd.Parameters.AddWithValue("@jx", jiroskopX);
                    cmd.Parameters.AddWithValue("@jy", jiroskopY);
                    cmd.Parameters.AddWithValue("@jz", jiroskopZ);
                    cmd.Parameters.AddWithValue("@aci", aci);
                    cmd.Parameters.AddWithValue("@bk", basincKosulu ? 1 : 0);
                    cmd.Parameters.AddWithValue("@ek", egimKosulu ? 1 : 0);
                    cmd.Parameters.AddWithValue("@ad", ayrilmaDurumu ? 1 : 0);
                    cmd.Parameters.AddWithValue("@z", DateTime.Now);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex) { Debug.WriteLine("[AyrilmaAlgoritmaTesti] Veri ekleme hatasi: " + ex.Message); }
        }

        // =====================================================================
        // OKUMA / GÖRÜNTÜLEME
        // =====================================================================
        /// <summary>
        /// Seçilen .db dosyasından verilen tabloyu okuyup, kolon başlıkları
        /// Türkçeleştirilmiş bir DataTable döner. (DataGridView'e bind edilebilir.)
        /// </summary>
        public static DataTable TabloyuOku(string dbYolu, string tabloAdi)
        {
            string conn = $"Data Source={dbYolu};Version=3;";
            var dataTable = new DataTable();
            using (var c = new SQLiteConnection(conn))
            {
                c.Open();
                using (var adapter = new SQLiteDataAdapter($"SELECT * FROM [{tabloAdi}]", c))
                    adapter.Fill(dataTable);
            }
            BasliklariTurkcelestir(dataTable);
            return dataTable;
        }

        private static void BasliklariTurkcelestir(DataTable dataTable)
        {
            var basliklar = new Dictionary<string, string>
            {
                { "durum", "Durum" }, { "roket_irtifa_basinc", "Roket İrtifa Basınç" },
                { "dogrulama_kodu", "Doğrulama Kodu" }, { "paket_sayaci", "Paket Sayacı" },
                { "roket_basinc", "Roket Basınç" }, { "roket_sicaklik", "Roket Sıcaklık" },
                { "roket_hiz_x", "Roket Hız X" }, { "roket_hiz_y", "Roket Hız Y" }, { "roket_hiz_z", "Roket Hız Z" },
                { "roket_acisal_hiz_x", "Açısal Hız X" }, { "roket_acisal_hiz_y", "Açısal Hız Y" }, { "roket_acisal_hiz_z", "Açısal Hız Z" },
                { "jiroskop_x", "Jiroskop X" }, { "jiroskop_y", "Jiroskop Y" }, { "jiroskop_z", "Jiroskop Z" },
                { "ivme_x", "İvme X" }, { "ivme_y", "İvme Y" }, { "ivme_z", "İvme Z" }, { "aci", "Açı" },
                { "roket_gps_enlem", "Roket GPS Enlem" }, { "roket_gps_boylam", "Roket GPS Boylam" }, { "roket_gps_yukseklik", "Roket GPS Yükseklik" },
                { "zaman", "Zaman" }, { "gps_irtifa", "GPS İrtifa" }, { "basinc_irtifa", "Basınç İrtifa" },
                { "gps_enlem", "GPS Enlem" }, { "gps_boylam", "GPS Boylam" }, { "geiger_veri", "Geiger Veri" },
                { "arduino_ms", "Arduino Süre (ms)" }, { "sicaklik", "Sıcaklık (°C)" }, { "basinc", "Basınç (hPa)" },
                { "nem", "Nem (%)" }, { "irtifa", "İrtifa (m)" }, { "goreceli_irtifa", "Göreceli İrtifa (m)" },
                { "ayrilma_durumu", "Ayrılma Durumu" }, { "egim_tetiklendi", "Eğim İle Ayrılma" },
                { "basinc_kosulu", "Basınç Koşulu" }, { "egim_kosulu", "Eğim Koşulu" },
                { "gyro_x", "Jiroskop X" }, { "gyro_y", "Jiroskop Y" }, { "gyro_z", "Jiroskop Z" },
                { "roll", "Roll" }, { "pitch", "Pitch" }, { "toplam_aci", "Toplam Açı" }, { "led_on", "LED" },
            };
            foreach (DataColumn column in dataTable.Columns)
                if (basliklar.ContainsKey(column.ColumnName))
                    column.ColumnName = basliklar[column.ColumnName];
        }
    }
}
