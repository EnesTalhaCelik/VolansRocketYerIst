using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.VariantTypes;

namespace VolansYerIstasyonu.UserControls
{
    public partial class uc_VeriTablo : UserControl
    {
        // -----------------------------------------------------------------
        // Connection string'ler (her aviyonik/test tipi için ayrı)
        // -----------------------------------------------------------------
        private static string anaAviyonikTableConnectionString;
        private static string yedekAviyonikTableConnectionString;
        private static string gorevYukuTableConnectionString;
        private static string basincTestiTableConnectionString;
        private static readonly object basincTestiLock = new object();
        private static string haberlesmeTestiTableConnectionString;
        private static readonly object haberlesmeTestiLock = new object();
        private static string jiroskopTestiTableConnectionString;
        private static readonly object jiroskopTestiLock = new object();
        private static string ayrilmaAlgoritmaTestiTableConnectionString;
        private static readonly object ayrilmaAlgoritmaTestiLock = new object();

        private static DataGridView currentTable;


        public uc_VeriTablo()
        {
            InitializeComponent();
            baslangicCalistir();
            anaAviyonikDatabaseOlustur();
        }

        public void baslangicCalistir()
        {
            comboboxTiklanabilirlik();
        }


        private void cbox_veriTabloUcus_SelectedIndexChanged(object sender, EventArgs e)
        {
            secilenDatabaseYansit();
        }


        private void secilenDatabaseYansit()
        {
            if (cbox_veriTabloUcus.SelectedItem == null)
                return;

            string secilenVeritabaniAdi = cbox_veriTabloUcus.SelectedItem.ToString();
            string secilenVeritabaniYolu = UcusVeriYoneticisi.DosyaYolu(secilenVeritabaniAdi + ".db");
            string secilenVeritabaniBaglantiDizesi = $"Data Source={secilenVeritabaniYolu};Version=3;";

            using (SQLiteConnection connection = new SQLiteConnection(secilenVeritabaniBaglantiDizesi))
            {
                connection.Open();

                // Hangi tablo sorgulanacak? Önce radio butonlara bakıyoruz, sonra
                // (test ise) dosya adından test tipini çıkarıyoruz.
                string query;
                if (rbtn_anaAviyonik.Checked)
                {
                    query = "SELECT * FROM [AnaAviyonikTablosu]";
                }
                else if (rbtn_yedekAviyonik.Checked)
                {
                    query = "SELECT * FROM [YedekAviyonikTablosu]";
                }
                else if (rbtn_gorevYuku.Checked)
                {
                    query = "SELECT * FROM [GorevYukuTablosu]";
                }
                else if (rbtn_testler.Checked)
                {
                    TestTipi tip = TestTanimlari.DosyaAdindanTipiBul(secilenVeritabaniAdi);
                    if (tip == TestTipi.Bilinmeyen)
                    {
                        MessageBox.Show("Bu dosyanın hangi test türüne ait olduğu tespit edilemedi: " + secilenVeritabaniAdi);
                        return;
                    }
                    string tabloAdi = TestTanimlari.Tanimlar[tip].TabloAdi;
                    query = $"SELECT * FROM [{tabloAdi}]";
                }
                else
                {
                    return;
                }

                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, connection))
                {
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    Dictionary<string, string> columnHeaders = new Dictionary<string, string>
                    {
                        // --- Aviyonik / görev yükü kolonları (mevcut) ---
                        { "durum", "Durum" },
                        { "roket_irtifa_basinc", "Roket İrtifa Basınç" },
                        { "dogrulama_kodu", "Doğrulama Kodu" },
                        { "paket_sayaci", "Paket Sayacı" },
                        { "roket_basinc", "Roket Basınç" },
                        { "roket_sicaklik", "Roket Sıcaklık" },
                        { "roket_hiz_x", "Roket Hız X" },
                        { "roket_hiz_y", "Roket Hız Y" },
                        { "roket_hiz_z", "Roket Hız Z" },
                        { "roket_acisal_hiz_x", "Roket Açısal Hız X" },
                        { "roket_acisal_hiz_y", "Roket Açısal Hız Y" },
                        { "roket_acisal_hiz_z", "Roket Açısal Hız Z" },
                        { "jiroskop_x", "Jiroskop X" },
                        { "jiroskop_y", "Jiroskop Y" },
                        { "jiroskop_z", "Jiroskop Z" },
                        { "ivme_x", "İvme X" },
                        { "ivme_y", "İvme Y" },
                        { "ivme_z", "İvme Z" },
                        { "aci", "Açı" },
                        { "roket_gps_enlem", "Roket GPS Enlem" },
                        { "roket_gps_boylam", "Roket GPS Boylam" },
                        { "roket_gps_yukseklik", "Roket GPS Yükseklik" },
                        { "zaman", "Zaman" },
                        { "gps_irtifa", "GPS İrtifa" },
                        { "basinc_irtifa", "Basınç İrtifa" },
                        { "gps_enlem", "GPS Enlem" },
                        { "gps_boylam", "GPS Boylam" },
                        { "geiger_veri", "Geiger Veri" },
                        // --- Test kolonları ---
                        { "arduino_ms", "Arduino Süre (ms)" },
                        { "sicaklik", "Sıcaklık (°C)" },
                        { "basinc", "Basınç (hPa)" },
                        { "nem", "Nem (%)" },
                        { "irtifa", "İrtifa (m)" },
                        { "goreceli_irtifa", "Göreceli İrtifa (m)" },
                        { "ayrilma_durumu", "Ayrılma Durumu" },
                        { "egim_tetiklendi", "Eğim İle Ayrılma" },
                        { "basinc_kosulu", "Basınç Koşulu" },
                        { "egim_kosulu", "Eğim Koşulu" },
                        // Haberleşme testi (gps_enlem/boylam/irtifa zaten yukarıda mapping var)
                    };

                    foreach (DataColumn column in dataTable.Columns)
                    {
                        if (columnHeaders.ContainsKey(column.ColumnName))
                            column.ColumnName = columnHeaders[column.ColumnName];
                    }

                    foreach (DataRow row in dataTable.Rows)
                    {
                        if (dataTable.Columns.Contains("Zaman"))
                        {
                            DateTime zaman;
                            if (DateTime.TryParse(row["Zaman"].ToString(), out zaman))
                            {
                                row["Zaman"] = new DateTime(zaman.Year, zaman.Month, zaman.Day, zaman.Hour, zaman.Minute, zaman.Second);
                            }
                        }
                    }

                    // Hangi DataGridView'a bind edileceği:
                    // Testler için tbl_anaAviyonik'i yeniden kullanıyoruz (geçici).
                    if (rbtn_anaAviyonik.Checked || rbtn_testler.Checked)
                        tbl_anaAviyonik.DataSource = dataTable;
                    else if (rbtn_yedekAviyonik.Checked)
                        tbl_yedekAviyonik.DataSource = dataTable;
                    else if (rbtn_gorevYuku.Checked)
                        tbl_gorevYuku.DataSource = dataTable;

                    foreach (DataGridViewColumn column in tbl_anaAviyonik.Columns)
                        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    foreach (DataGridViewColumn column in tbl_yedekAviyonik.Columns)
                        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    foreach (DataGridViewColumn column in tbl_gorevYuku.Columns)
                        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                }

                connection.Close();
            }
        }


        // ====================================================================
        // ANA AVİYONİK
        // ====================================================================
        private void btn_anaAvDbOlustur_Click(object sender, EventArgs e)
        {
            anaAviyonikDatabaseOlustur();
        }

        public string anaAviyonikDatabaseOlustur()
        {
            string connectionString = string.Empty;
            try
            {
                string databaseAdi = $"VeriAnaAv-{DateTime.Now:yyyyMMdd_HHmmss}.db";
                string veriPath = UcusVeriYoneticisi.DosyaYolu(databaseAdi);
                connectionString = $"Data Source={veriPath};Version=3;";

                if (!File.Exists(veriPath))
                {
                    SQLiteConnection.CreateFile(veriPath);
                    using (var connection = new SQLiteConnection(connectionString))
                    {
                        connection.Open();
                        string table = @"
                            CREATE TABLE IF NOT EXISTS [AnaAviyonikTablosu] (
                                durum INTEGER, roket_irtifa_basinc REAL, dogrulama_kodu INTEGER,
                                paket_sayaci INTEGER, roket_basinc REAL, roket_sicaklik REAL,
                                roket_hiz_x REAL, roket_hiz_y REAL, roket_hiz_z REAL,
                                roket_acisal_hiz_x REAL, roket_acisal_hiz_y REAL, roket_acisal_hiz_z REAL,
                                jiroskop_x REAL, jiroskop_y REAL, jiroskop_z REAL,
                                ivme_x REAL, ivme_y REAL, ivme_z REAL, aci REAL,
                                roket_gps_enlem REAL, roket_gps_boylam REAL, roket_gps_yukseklik REAL,
                                zaman TEXT
                            )";
                        new SQLiteCommand(table, connection).ExecuteNonQuery();
                    }
                    comboboxGuncelle();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ana Aviyonik veritabanı oluşturulurken hata: " + ex.Message);
            }
            anaAviyonikTableConnectionString = connectionString;
            return connectionString;
        }

        public static void anaAviyonikVeriEkle(int durum, float roket_irtifa_basinc, int dogrulama_kodu, int paket_sayaci,
            float roket_basinc, float roket_sicaklik, float roket_hiz_x, float roket_hiz_y, float roket_hiz_z, float roket_acisal_hiz_x,
            float roket_acisal_hiz_y, float roket_acisal_hiz_z, float jiroskop_x, float jiroskop_y, float jiroskop_z, float ivme_x,
            float ivme_y, float ivme_z, float aci, float roket_gps_enlem, float roket_gps_boylam, float roket_gps_yukseklik, DateTime zaman)
        {
            try
            {
                using (var connection = new SQLiteConnection(anaAviyonikTableConnectionString))
                {
                    connection.Open();
                    string insertQuery = "INSERT INTO [AnaAviyonikTablosu] (durum, roket_irtifa_basinc, dogrulama_kodu, paket_sayaci," +
                        " roket_basinc, roket_sicaklik, roket_hiz_x, roket_hiz_y, roket_hiz_z, roket_acisal_hiz_x, roket_acisal_hiz_y," +
                        " roket_acisal_hiz_z, jiroskop_x, jiroskop_y, jiroskop_z, ivme_x, ivme_y, ivme_z, aci, roket_gps_enlem," +
                        " roket_gps_boylam, roket_gps_yukseklik, zaman) VALUES (@durum, @roket_irtifa_basinc, @dogrulama_kodu, @paket_sayaci," +
                        " @roket_basinc, @roket_sicaklik, @roket_hiz_x, @roket_hiz_y, @roket_hiz_z, @roket_acisal_hiz_x, @roket_acisal_hiz_y," +
                        " @roket_acisal_hiz_z, @jiroskop_x, @jiroskop_y, @jiroskop_z, @ivme_x, @ivme_y, @ivme_z, @aci, @roket_gps_enlem," +
                        " @roket_gps_boylam, @roket_gps_yukseklik, @zaman)";
                    var cmd = new SQLiteCommand(insertQuery, connection);
                    cmd.Parameters.AddWithValue("@durum", durum);
                    cmd.Parameters.AddWithValue("@roket_irtifa_basinc", roket_irtifa_basinc);
                    cmd.Parameters.AddWithValue("@dogrulama_kodu", dogrulama_kodu);
                    cmd.Parameters.AddWithValue("@paket_sayaci", paket_sayaci);
                    cmd.Parameters.AddWithValue("@roket_basinc", roket_basinc);
                    cmd.Parameters.AddWithValue("@roket_sicaklik", roket_sicaklik);
                    cmd.Parameters.AddWithValue("@roket_hiz_x", roket_hiz_x);
                    cmd.Parameters.AddWithValue("@roket_hiz_y", roket_hiz_y);
                    cmd.Parameters.AddWithValue("@roket_hiz_z", roket_hiz_z);
                    cmd.Parameters.AddWithValue("@roket_acisal_hiz_x", roket_acisal_hiz_x);
                    cmd.Parameters.AddWithValue("@roket_acisal_hiz_y", roket_acisal_hiz_y);
                    cmd.Parameters.AddWithValue("@roket_acisal_hiz_z", roket_acisal_hiz_z);
                    cmd.Parameters.AddWithValue("@jiroskop_x", jiroskop_x);
                    cmd.Parameters.AddWithValue("@jiroskop_y", jiroskop_y);
                    cmd.Parameters.AddWithValue("@jiroskop_z", jiroskop_z);
                    cmd.Parameters.AddWithValue("@ivme_x", ivme_x);
                    cmd.Parameters.AddWithValue("@ivme_y", ivme_y);
                    cmd.Parameters.AddWithValue("@ivme_z", ivme_z);
                    cmd.Parameters.AddWithValue("@aci", aci);
                    cmd.Parameters.AddWithValue("@roket_gps_enlem", roket_gps_enlem);
                    cmd.Parameters.AddWithValue("@roket_gps_boylam", roket_gps_boylam);
                    cmd.Parameters.AddWithValue("@roket_gps_yukseklik", roket_gps_yukseklik);
                    cmd.Parameters.AddWithValue("@zaman", zaman);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ana Aviyonik veri eklemesinde hata: " + ex.Message);
            }
        }

        public static void anaAviyonikVeriEkle(int durum, float roket_irtifa_basinc, int dogrulama_kodu, int paket_sayaci,
            float roket_basinc, float roket_sicaklik, float roket_hiz_x, float roket_hiz_y, float roket_hiz_z, float roket_acisal_hiz_x,
            float roket_acisal_hiz_y, float roket_acisal_hiz_z, float jiroskop_x, float jiroskop_y, float jiroskop_z, float ivme_x,
            float ivme_y, float ivme_z, float aci, float roket_gps_enlem, float roket_gps_boylam, float roket_gps_yukseklik)
        {
            anaAviyonikVeriEkle(durum, roket_irtifa_basinc, dogrulama_kodu, paket_sayaci,
                roket_basinc, roket_sicaklik, roket_hiz_x, roket_hiz_y, roket_hiz_z, roket_acisal_hiz_x,
                roket_acisal_hiz_y, roket_acisal_hiz_z, jiroskop_x, jiroskop_y, jiroskop_z, ivme_x,
                ivme_y, ivme_z, aci, roket_gps_enlem, roket_gps_boylam, roket_gps_yukseklik, DateTime.Now);
        }


        // ====================================================================
        // YEDEK AVİYONİK
        // ====================================================================
        private void btn_yedekAvDbOlustur_Click(object sender, EventArgs e)
        {
            yedekAviyonikDatabaseOlustur();
        }

        public string yedekAviyonikDatabaseOlustur()
        {
            string connectionString = string.Empty;
            try
            {
                string databaseAdi = $"VeriYedekAv-{DateTime.Now:yyyyMMdd_HHmmss}.db";
                string veriPath = UcusVeriYoneticisi.DosyaYolu(databaseAdi);
                connectionString = $"Data Source={veriPath};Version=3;";

                if (!File.Exists(veriPath))
                {
                    SQLiteConnection.CreateFile(veriPath);
                    using (var connection = new SQLiteConnection(connectionString))
                    {
                        connection.Open();
                        string table = @"
                            CREATE TABLE IF NOT EXISTS [YedekAviyonikTablosu] (
                                durum INTEGER, roket_irtifa_basinc REAL, dogrulama_kodu INTEGER,
                                paket_sayaci INTEGER, roket_basinc REAL, roket_sicaklik REAL,
                                roket_hiz_x REAL, roket_hiz_y REAL, roket_hiz_z REAL,
                                roket_acisal_hiz_x REAL, roket_acisal_hiz_y REAL, roket_acisal_hiz_z REAL,
                                jiroskop_x REAL, jiroskop_y REAL, jiroskop_z REAL,
                                ivme_x REAL, ivme_y REAL, ivme_z REAL, aci REAL,
                                roket_gps_enlem REAL, roket_gps_boylam REAL, roket_gps_yukseklik REAL,
                                zaman TEXT
                            )";
                        new SQLiteCommand(table, connection).ExecuteNonQuery();
                    }
                    comboboxGuncelle();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Yedek Aviyonik veritabanı oluşturulurken hata: " + ex.Message);
            }
            yedekAviyonikTableConnectionString = connectionString;
            return connectionString;
        }

        public static void yedekAviyonikVeriEkle(int durum, float roket_irtifa_basinc, int dogrulama_kodu, int paket_sayaci,
            float roket_basinc, float roket_sicaklik, float roket_hiz_x, float roket_hiz_y, float roket_hiz_z, float roket_acisal_hiz_x,
            float roket_acisal_hiz_y, float roket_acisal_hiz_z, float jiroskop_x, float jiroskop_y, float jiroskop_z, float ivme_x,
            float ivme_y, float ivme_z, float aci, float roket_gps_enlem, float roket_gps_boylam, float roket_gps_yukseklik, DateTime zaman)
        {
            try
            {
                using (var connection = new SQLiteConnection(yedekAviyonikTableConnectionString))
                {
                    connection.Open();
                    string insertQuery = "INSERT INTO [YedekAviyonikTablosu] (durum, roket_irtifa_basinc, dogrulama_kodu, paket_sayaci," +
                        " roket_basinc, roket_sicaklik, roket_hiz_x, roket_hiz_y, roket_hiz_z, roket_acisal_hiz_x, roket_acisal_hiz_y," +
                        " roket_acisal_hiz_z, jiroskop_x, jiroskop_y, jiroskop_z, ivme_x, ivme_y, ivme_z, aci, roket_gps_enlem," +
                        " roket_gps_boylam, roket_gps_yukseklik, zaman) VALUES (@durum, @roket_irtifa_basinc, @dogrulama_kodu, @paket_sayaci," +
                        " @roket_basinc, @roket_sicaklik, @roket_hiz_x, @roket_hiz_y, @roket_hiz_z, @roket_acisal_hiz_x, @roket_acisal_hiz_y," +
                        " @roket_acisal_hiz_z, @jiroskop_x, @jiroskop_y, @jiroskop_z, @ivme_x, @ivme_y, @ivme_z, @aci, @roket_gps_enlem," +
                        " @roket_gps_boylam, @roket_gps_yukseklik, @zaman)";
                    var cmd = new SQLiteCommand(insertQuery, connection);
                    cmd.Parameters.AddWithValue("@durum", durum);
                    cmd.Parameters.AddWithValue("@roket_irtifa_basinc", roket_irtifa_basinc);
                    cmd.Parameters.AddWithValue("@dogrulama_kodu", dogrulama_kodu);
                    cmd.Parameters.AddWithValue("@paket_sayaci", paket_sayaci);
                    cmd.Parameters.AddWithValue("@roket_basinc", roket_basinc);
                    cmd.Parameters.AddWithValue("@roket_sicaklik", roket_sicaklik);
                    cmd.Parameters.AddWithValue("@roket_hiz_x", roket_hiz_x);
                    cmd.Parameters.AddWithValue("@roket_hiz_y", roket_hiz_y);
                    cmd.Parameters.AddWithValue("@roket_hiz_z", roket_hiz_z);
                    cmd.Parameters.AddWithValue("@roket_acisal_hiz_x", roket_acisal_hiz_x);
                    cmd.Parameters.AddWithValue("@roket_acisal_hiz_y", roket_acisal_hiz_y);
                    cmd.Parameters.AddWithValue("@roket_acisal_hiz_z", roket_acisal_hiz_z);
                    cmd.Parameters.AddWithValue("@jiroskop_x", jiroskop_x);
                    cmd.Parameters.AddWithValue("@jiroskop_y", jiroskop_y);
                    cmd.Parameters.AddWithValue("@jiroskop_z", jiroskop_z);
                    cmd.Parameters.AddWithValue("@ivme_x", ivme_x);
                    cmd.Parameters.AddWithValue("@ivme_y", ivme_y);
                    cmd.Parameters.AddWithValue("@ivme_z", ivme_z);
                    cmd.Parameters.AddWithValue("@aci", aci);
                    cmd.Parameters.AddWithValue("@roket_gps_enlem", roket_gps_enlem);
                    cmd.Parameters.AddWithValue("@roket_gps_boylam", roket_gps_boylam);
                    cmd.Parameters.AddWithValue("@roket_gps_yukseklik", roket_gps_yukseklik);
                    cmd.Parameters.AddWithValue("@zaman", zaman);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Yedek Aviyonik veri eklemesinde hata: " + ex.Message);
            }
        }

        public static void yedekAviyonikVeriEkle(int durum, float roket_irtifa_basinc, int dogrulama_kodu, int paket_sayaci,
            float roket_basinc, float roket_sicaklik, float roket_hiz_x, float roket_hiz_y, float roket_hiz_z, float roket_acisal_hiz_x,
            float roket_acisal_hiz_y, float roket_acisal_hiz_z, float jiroskop_x, float jiroskop_y, float jiroskop_z, float ivme_x,
            float ivme_y, float ivme_z, float aci, float roket_gps_enlem, float roket_gps_boylam, float roket_gps_yukseklik)
        {
            yedekAviyonikVeriEkle(durum, roket_irtifa_basinc, dogrulama_kodu, paket_sayaci,
                roket_basinc, roket_sicaklik, roket_hiz_x, roket_hiz_y, roket_hiz_z, roket_acisal_hiz_x,
                roket_acisal_hiz_y, roket_acisal_hiz_z, jiroskop_x, jiroskop_y, jiroskop_z, ivme_x,
                ivme_y, ivme_z, aci, roket_gps_enlem, roket_gps_boylam, roket_gps_yukseklik, DateTime.Now);
        }


        // ====================================================================
        // GÖREV YÜKÜ
        // ====================================================================
        private void btn_gorevYukuDbOlustur_Click(object sender, EventArgs e)
        {
            gorevYukuDatabaseOlustur();
        }

        public string gorevYukuDatabaseOlustur()
        {
            string connectionString = string.Empty;
            try
            {
                string databaseAdi = $"VeriGorevYuku-{DateTime.Now:yyyyMMdd_HHmmss}.db";
                string veriPath = UcusVeriYoneticisi.DosyaYolu(databaseAdi);
                connectionString = $"Data Source={veriPath};Version=3;";

                if (!File.Exists(veriPath))
                {
                    SQLiteConnection.CreateFile(veriPath);
                    using (var connection = new SQLiteConnection(connectionString))
                    {
                        connection.Open();
                        string table = @"
                            CREATE TABLE IF NOT EXISTS [GorevYukuTablosu] (
                                gps_irtifa REAL, basinc_irtifa REAL, gps_enlem REAL, gps_boylam REAL,
                                geiger_veri REAL, paket_sayaci INTEGER, dogrulama_kodu INTEGER, zaman TEXT
                            )";
                        new SQLiteCommand(table, connection).ExecuteNonQuery();
                    }
                    comboboxGuncelle();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Görev Yükü veritabanı oluşturulurken hata: " + ex.Message);
            }
            gorevYukuTableConnectionString = connectionString;
            return connectionString;
        }

        public static void gorevYukuVeriEkle(float gps_irtifa, float basinc_irtifa, float gps_enlem, float gps_boylam,
            float geiger_veri, int paket_sayaci, int dogrulama_kodu, DateTime zaman)
        {
            try
            {
                using (var connection = new SQLiteConnection(gorevYukuTableConnectionString))
                {
                    connection.Open();
                    string insertQuery = "INSERT INTO [GorevYukuTablosu] (gps_irtifa, basinc_irtifa, gps_enlem, gps_boylam," +
                        " geiger_veri, paket_sayaci, dogrulama_kodu, zaman) VALUES (@gps_irtifa, @basinc_irtifa, @gps_enlem," +
                        " @gps_boylam, @geiger_veri, @paket_sayaci, @dogrulama_kodu, @zaman)";
                    var cmd = new SQLiteCommand(insertQuery, connection);
                    cmd.Parameters.AddWithValue("@gps_irtifa", gps_irtifa);
                    cmd.Parameters.AddWithValue("@basinc_irtifa", basinc_irtifa);
                    cmd.Parameters.AddWithValue("@gps_enlem", gps_enlem);
                    cmd.Parameters.AddWithValue("@gps_boylam", gps_boylam);
                    cmd.Parameters.AddWithValue("@geiger_veri", geiger_veri);
                    cmd.Parameters.AddWithValue("@paket_sayaci", paket_sayaci);
                    cmd.Parameters.AddWithValue("@dogrulama_kodu", dogrulama_kodu);
                    cmd.Parameters.AddWithValue("@zaman", zaman);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Görev Yükü veri eklemesinde hata: " + ex.Message);
            }
        }

        public static void gorevYukuVeriEkle(float gps_irtifa, float basinc_irtifa, float gps_enlem, float gps_boylam,
            float geiger_veri, int paket_sayaci, int dogrulama_kodu)
        {
            gorevYukuVeriEkle(gps_irtifa, basinc_irtifa, gps_enlem, gps_boylam, geiger_veri,
                paket_sayaci, dogrulama_kodu, DateTime.Now);
        }


        // ====================================================================
        // BASINÇ TESTİ (YENİ)
        //
        // Database otomatik oluşturulur: program açıldıktan sonra ilk basınç
        // testi paketi geldiğinde tetiklenir. Her program oturumu için ayrı
        // bir Test-Basinc-{tarih}.db dosyası yaratılır.
        // ====================================================================

        /// <summary>
        /// Aktif bir basınç testi DB'si yoksa yenisini oluşturur.
        /// Aktif varsa onun connection string'ini döner. Thread-safe.
        /// </summary>
        public static string basincTestiDatabaseHazirla(bool yeniOlustur)
        {
            lock (basincTestiLock)
            {
                if (!yeniOlustur && !string.IsNullOrEmpty(basincTestiTableConnectionString))
                    return basincTestiTableConnectionString;

                try
                {
                    var tanim = TestTanimlari.Tanimlar[TestTipi.BasincTesti];
                    string databaseAdi = $"{tanim.DosyaAdiOnEki}-{DateTime.Now:yyyyMMdd_HHmmss}.db";
                    string veriPath = UcusVeriYoneticisi.DosyaYolu(databaseAdi);
                    string connectionString = $"Data Source={veriPath};Version=3;";

                    if (!File.Exists(veriPath))
                    {
                        SQLiteConnection.CreateFile(veriPath);
                        using (var connection = new SQLiteConnection(connectionString))
                        {
                            connection.Open();
                            string table = $@"
                                CREATE TABLE IF NOT EXISTS [{tanim.TabloAdi}] (
                                    paket_sayaci INTEGER,
                                    arduino_ms INTEGER,
                                    sicaklik REAL,
                                    basinc REAL,
                                    nem REAL,
                                    irtifa REAL,
                                    goreceli_irtifa REAL,
                                    ayrilma_durumu INTEGER,
                                    zaman TEXT
                                )";
                            new SQLiteCommand(table, connection).ExecuteNonQuery();
                        }
                    }

                    basincTestiTableConnectionString = connectionString;
                    Debug.WriteLine("[BasincTesti] Yeni DB oluşturuldu: " + databaseAdi);
                    return connectionString;
                }
                catch (Exception ex)
                {
                    // Worker thread'deyiz, MessageBox kullanmıyoruz.
                    Debug.WriteLine("[BasincTesti] DB oluşturma hatası: " + ex.Message);
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// Basınç testi paketinden çözümlenen veriyi DB'ye yazar.
        /// DB yoksa otomatik oluşturur. LoRa worker thread'inden çağrılır.
        /// </summary>
        public static void basincTestiVeriEkle(
            uint paketSayaci, uint arduinoMs, float sicaklik, float basinc, float nem,
            float irtifa, float goreceliIrtifa, bool ayrilmaDurumu)
        {
            string connectionString = basincTestiDatabaseHazirla(false);
            if (string.IsNullOrEmpty(connectionString))
            {
                Debug.WriteLine("[BasincTesti] DB hazır değil, veri atlandı.");
                return;
            }

            try
            {
                var tanim = TestTanimlari.Tanimlar[TestTipi.BasincTesti];
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string insertQuery = $@"
                        INSERT INTO [{tanim.TabloAdi}]
                            (paket_sayaci, arduino_ms, sicaklik, basinc, nem,
                             irtifa, goreceli_irtifa, ayrilma_durumu, zaman)
                        VALUES
                            (@paket_sayaci, @arduino_ms, @sicaklik, @basinc, @nem,
                             @irtifa, @goreceli_irtifa, @ayrilma_durumu, @zaman)";
                    var cmd = new SQLiteCommand(insertQuery, connection);
                    cmd.Parameters.AddWithValue("@paket_sayaci", paketSayaci);
                    cmd.Parameters.AddWithValue("@arduino_ms", arduinoMs);
                    cmd.Parameters.AddWithValue("@sicaklik", sicaklik);
                    cmd.Parameters.AddWithValue("@basinc", basinc);
                    cmd.Parameters.AddWithValue("@nem", nem);
                    cmd.Parameters.AddWithValue("@irtifa", irtifa);
                    cmd.Parameters.AddWithValue("@goreceli_irtifa", goreceliIrtifa);
                    cmd.Parameters.AddWithValue("@ayrilma_durumu", ayrilmaDurumu ? 1 : 0);
                    cmd.Parameters.AddWithValue("@zaman", DateTime.Now);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[BasincTesti] Veri ekleme hatası: " + ex.Message);
            }
        }


        // ====================================================================
        // HABERLEŞME TESTİ (YENİ)
        //
        // Paket: testId, paketSayaci, arduinoMs, sicaklik, basinc,
        //        gpsEnlem, gpsBoylam, gpsIrtifa
        //
        // Database otomatik oluşturulur: program açıldıktan sonra ilk
        // haberleşme testi paketi geldiğinde tetiklenir.
        // ====================================================================

        public static string haberlesmeTestiDatabaseHazirla(bool yeniOlustur)
        {
            lock (haberlesmeTestiLock)
            {
                if (!yeniOlustur && !string.IsNullOrEmpty(haberlesmeTestiTableConnectionString))
                    return haberlesmeTestiTableConnectionString;

                try
                {
                    var tanim = TestTanimlari.Tanimlar[TestTipi.HaberlesmeTesti];
                    string databaseAdi = $"{tanim.DosyaAdiOnEki}-{DateTime.Now:yyyyMMdd_HHmmss}.db";
                    string veriPath = UcusVeriYoneticisi.DosyaYolu(databaseAdi);
                    string connectionString = $"Data Source={veriPath};Version=3;";

                    if (!File.Exists(veriPath))
                    {
                        SQLiteConnection.CreateFile(veriPath);
                        using (var connection = new SQLiteConnection(connectionString))
                        {
                            connection.Open();
                            string table = $@"
                                CREATE TABLE IF NOT EXISTS [{tanim.TabloAdi}] (
                                    paket_sayaci INTEGER,
                                    arduino_ms INTEGER,
                                    sicaklik REAL,
                                    basinc REAL,
                                    gps_enlem REAL,
                                    gps_boylam REAL,
                                    gps_irtifa REAL,
                                    zaman TEXT
                                )";
                            new SQLiteCommand(table, connection).ExecuteNonQuery();
                        }
                    }

                    haberlesmeTestiTableConnectionString = connectionString;
                    Debug.WriteLine("[HaberlesmeTesti] Yeni DB oluşturuldu: " + databaseAdi);
                    return connectionString;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("[HaberlesmeTesti] DB oluşturma hatası: " + ex.Message);
                    return string.Empty;
                }
            }
        }

        public static void haberlesmeTestiVeriEkle(
            uint paketSayaci, uint arduinoMs, float sicaklik, float basinc,
            double gpsEnlem, double gpsBoylam, float gpsIrtifa)
        {
            string connectionString = haberlesmeTestiDatabaseHazirla(false);
            if (string.IsNullOrEmpty(connectionString))
            {
                Debug.WriteLine("[HaberlesmeTesti] DB hazır değil, veri atlandı.");
                return;
            }

            try
            {
                var tanim = TestTanimlari.Tanimlar[TestTipi.HaberlesmeTesti];
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string insertQuery = $@"
                        INSERT INTO [{tanim.TabloAdi}]
                            (paket_sayaci, arduino_ms, sicaklik, basinc,
                             gps_enlem, gps_boylam, gps_irtifa, zaman)
                        VALUES
                            (@paket_sayaci, @arduino_ms, @sicaklik, @basinc,
                             @gps_enlem, @gps_boylam, @gps_irtifa, @zaman)";
                    var cmd = new SQLiteCommand(insertQuery, connection);
                    cmd.Parameters.AddWithValue("@paket_sayaci", paketSayaci);
                    cmd.Parameters.AddWithValue("@arduino_ms", arduinoMs);
                    cmd.Parameters.AddWithValue("@sicaklik", sicaklik);
                    cmd.Parameters.AddWithValue("@basinc", basinc);
                    cmd.Parameters.AddWithValue("@gps_enlem", gpsEnlem);
                    cmd.Parameters.AddWithValue("@gps_boylam", gpsBoylam);
                    cmd.Parameters.AddWithValue("@gps_irtifa", gpsIrtifa);
                    cmd.Parameters.AddWithValue("@zaman", DateTime.Now);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[HaberlesmeTesti] Veri ekleme hatası: " + ex.Message);
            }
        }


        // ====================================================================
        // JİROSKOP TESTİ (YENİ)
        //
        // Paket: testId, paketSayaci, arduinoMs, jiroskopX, jiroskopY, jiroskopZ,
        //        aci, egimTetiklendi
        //
        // - jiroskopX/Y/Z: açısal hız (°/s) - ham jiroskop verisi
        // - aci: yer normali ile yapılan açı (°) - işlenmiş veri
        //   (jiroskop + ivmeölçer'den filter ile çıkarılan değer)
        // - egimTetiklendi: ESP32'de açı eşiği geçilince 1 olur
        //
        // Database otomatik oluşturulur: program açıldıktan sonra ilk
        // jiroskop testi paketi geldiğinde tetiklenir.
        // ====================================================================

        public static string jiroskopTestiDatabaseHazirla(bool yeniOlustur)
        {
            lock (jiroskopTestiLock)
            {
                if (!yeniOlustur && !string.IsNullOrEmpty(jiroskopTestiTableConnectionString))
                    return jiroskopTestiTableConnectionString;

                try
                {
                    var tanim = TestTanimlari.Tanimlar[TestTipi.JiroskopTesti];
                    string databaseAdi = $"{tanim.DosyaAdiOnEki}-{DateTime.Now:yyyyMMdd_HHmmss}.db";
                    string veriPath = UcusVeriYoneticisi.DosyaYolu(databaseAdi);
                    string connectionString = $"Data Source={veriPath};Version=3;";

                    if (!File.Exists(veriPath))
                    {
                        SQLiteConnection.CreateFile(veriPath);
                        using (var connection = new SQLiteConnection(connectionString))
                        {
                            connection.Open();
                            string table = $@"
                                CREATE TABLE IF NOT EXISTS [{tanim.TabloAdi}] (
                                    paket_sayaci INTEGER,
                                    arduino_ms INTEGER,
                                    jiroskop_x REAL,
                                    jiroskop_y REAL,
                                    jiroskop_z REAL,
                                    aci REAL,
                                    egim_tetiklendi INTEGER,
                                    zaman TEXT
                                )";
                            new SQLiteCommand(table, connection).ExecuteNonQuery();
                        }
                    }

                    jiroskopTestiTableConnectionString = connectionString;
                    Debug.WriteLine("[JiroskopTesti] Yeni DB oluşturuldu: " + databaseAdi);
                    return connectionString;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("[JiroskopTesti] DB oluşturma hatası: " + ex.Message);
                    return string.Empty;
                }
            }
        }

        public static void jiroskopTestiVeriEkle(
            uint paketSayaci, uint arduinoMs,
            float jiroskopX, float jiroskopY, float jiroskopZ,
            float aci, bool egimTetiklendi)
        {
            string connectionString = jiroskopTestiDatabaseHazirla(false);
            if (string.IsNullOrEmpty(connectionString))
            {
                Debug.WriteLine("[JiroskopTesti] DB hazır değil, veri atlandı.");
                return;
            }

            try
            {
                var tanim = TestTanimlari.Tanimlar[TestTipi.JiroskopTesti];
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string insertQuery = $@"
                        INSERT INTO [{tanim.TabloAdi}]
                            (paket_sayaci, arduino_ms,
                             jiroskop_x, jiroskop_y, jiroskop_z,
                             aci, egim_tetiklendi, zaman)
                        VALUES
                            (@paket_sayaci, @arduino_ms,
                             @jiroskop_x, @jiroskop_y, @jiroskop_z,
                             @aci, @egim_tetiklendi, @zaman)";
                    var cmd = new SQLiteCommand(insertQuery, connection);
                    cmd.Parameters.AddWithValue("@paket_sayaci", paketSayaci);
                    cmd.Parameters.AddWithValue("@arduino_ms", arduinoMs);
                    cmd.Parameters.AddWithValue("@jiroskop_x", jiroskopX);
                    cmd.Parameters.AddWithValue("@jiroskop_y", jiroskopY);
                    cmd.Parameters.AddWithValue("@jiroskop_z", jiroskopZ);
                    cmd.Parameters.AddWithValue("@aci", aci);
                    cmd.Parameters.AddWithValue("@egim_tetiklendi", egimTetiklendi ? 1 : 0);
                    cmd.Parameters.AddWithValue("@zaman", DateTime.Now);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[JiroskopTesti] Veri ekleme hatası: " + ex.Message);
            }
        }


        // ====================================================================
        // AYRILMA ALGORİTMA TESTİ (YENİ)
        //
        // BME280 + jiroskop verilerini bir arada içeren, iki-kriterli ayrılma
        // algoritmasının test edildiği paket türü.
        //
        // Şartname karşılığı: "Özgün UKB Algoritma Testi" (KTR şablonu)
        //
        // Üç ayrı flag kaydedilir:
        // - basinc_kosulu:  irtifa kaybı eşiği geçildi mi
        // - egim_kosulu:    açı eşiği geçildi mi
        // - ayrilma_durumu: ESP32'nin nihai kararı (genelde AND, kullanıcı
        //   algoritmasına göre değişebilir)
        // ====================================================================

        public static string ayrilmaAlgoritmaTestiDatabaseHazirla(bool yeniOlustur)
        {
            lock (ayrilmaAlgoritmaTestiLock)
            {
                if (!yeniOlustur && !string.IsNullOrEmpty(ayrilmaAlgoritmaTestiTableConnectionString))
                    return ayrilmaAlgoritmaTestiTableConnectionString;

                try
                {
                    var tanim = TestTanimlari.Tanimlar[TestTipi.AyrilmaAlgoritmaTesti];
                    string databaseAdi = $"{tanim.DosyaAdiOnEki}-{DateTime.Now:yyyyMMdd_HHmmss}.db";
                    string veriPath = UcusVeriYoneticisi.DosyaYolu(databaseAdi);
                    string connectionString = $"Data Source={veriPath};Version=3;";

                    if (!File.Exists(veriPath))
                    {
                        SQLiteConnection.CreateFile(veriPath);
                        using (var connection = new SQLiteConnection(connectionString))
                        {
                            connection.Open();
                            string table = $@"
                                CREATE TABLE IF NOT EXISTS [{tanim.TabloAdi}] (
                                    paket_sayaci INTEGER,
                                    arduino_ms INTEGER,
                                    sicaklik REAL,
                                    basinc REAL,
                                    nem REAL,
                                    irtifa REAL,
                                    jiroskop_x REAL,
                                    jiroskop_y REAL,
                                    jiroskop_z REAL,
                                    aci REAL,
                                    basinc_kosulu INTEGER,
                                    egim_kosulu INTEGER,
                                    ayrilma_durumu INTEGER,
                                    zaman TEXT
                                )";
                            new SQLiteCommand(table, connection).ExecuteNonQuery();
                        }
                    }

                    ayrilmaAlgoritmaTestiTableConnectionString = connectionString;
                    Debug.WriteLine("[AyrilmaAlgoritmaTesti] Yeni DB oluşturuldu: " + databaseAdi);
                    return connectionString;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("[AyrilmaAlgoritmaTesti] DB oluşturma hatası: " + ex.Message);
                    return string.Empty;
                }
            }
        }

        public static void ayrilmaAlgoritmaTestiVeriEkle(
            uint paketSayaci, uint arduinoMs,
            float sicaklik, float basinc, float nem, float irtifa,
            float jiroskopX, float jiroskopY, float jiroskopZ, float aci,
            bool basincKosulu, bool egimKosulu, bool ayrilmaDurumu)
        {
            string connectionString = ayrilmaAlgoritmaTestiDatabaseHazirla(false);
            if (string.IsNullOrEmpty(connectionString))
            {
                Debug.WriteLine("[AyrilmaAlgoritmaTesti] DB hazır değil, veri atlandı.");
                return;
            }

            try
            {
                var tanim = TestTanimlari.Tanimlar[TestTipi.AyrilmaAlgoritmaTesti];
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string insertQuery = $@"
                        INSERT INTO [{tanim.TabloAdi}]
                            (paket_sayaci, arduino_ms,
                             sicaklik, basinc, nem, irtifa,
                             jiroskop_x, jiroskop_y, jiroskop_z, aci,
                             basinc_kosulu, egim_kosulu, ayrilma_durumu, zaman)
                        VALUES
                            (@paket_sayaci, @arduino_ms,
                             @sicaklik, @basinc, @nem, @irtifa,
                             @jiroskop_x, @jiroskop_y, @jiroskop_z, @aci,
                             @basinc_kosulu, @egim_kosulu, @ayrilma_durumu, @zaman)";
                    var cmd = new SQLiteCommand(insertQuery, connection);
                    cmd.Parameters.AddWithValue("@paket_sayaci", paketSayaci);
                    cmd.Parameters.AddWithValue("@arduino_ms", arduinoMs);
                    cmd.Parameters.AddWithValue("@sicaklik", sicaklik);
                    cmd.Parameters.AddWithValue("@basinc", basinc);
                    cmd.Parameters.AddWithValue("@nem", nem);
                    cmd.Parameters.AddWithValue("@irtifa", irtifa);
                    cmd.Parameters.AddWithValue("@jiroskop_x", jiroskopX);
                    cmd.Parameters.AddWithValue("@jiroskop_y", jiroskopY);
                    cmd.Parameters.AddWithValue("@jiroskop_z", jiroskopZ);
                    cmd.Parameters.AddWithValue("@aci", aci);
                    cmd.Parameters.AddWithValue("@basinc_kosulu", basincKosulu ? 1 : 0);
                    cmd.Parameters.AddWithValue("@egim_kosulu", egimKosulu ? 1 : 0);
                    cmd.Parameters.AddWithValue("@ayrilma_durumu", ayrilmaDurumu ? 1 : 0);
                    cmd.Parameters.AddWithValue("@zaman", DateTime.Now);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[AyrilmaAlgoritmaTesti] Veri ekleme hatası: " + ex.Message);
            }
        }


        // ====================================================================
        // EXCEL EXPORT
        // ====================================================================
        private void btn_verileriExceleAktar_Click(object sender, EventArgs e)
        {
            if (rbtn_anaAviyonik.Checked)
                verileriExceleAktar(tbl_anaAviyonik, "AnaAviyonikVerileri");
            else if (rbtn_yedekAviyonik.Checked)
                verileriExceleAktar(tbl_yedekAviyonik, "YedekAviyonikVerileri");
            else if (rbtn_gorevYuku.Checked)
                verileriExceleAktar(tbl_gorevYuku, "GorevYukuVerileri");
            else if (rbtn_testler.Checked)
            {
                // Seçili dosyadan test tipini bul, worksheet adını ona göre yaz
                string secili = cbox_veriTabloUcus.SelectedItem?.ToString() ?? "";
                TestTipi tip = TestTanimlari.DosyaAdindanTipiBul(secili);
                string wsAdi = (tip != TestTipi.Bilinmeyen)
                    ? TestTanimlari.Tanimlar[tip].Aciklama.Replace(" ", "") + "Verileri"
                    : "TestVerileri";
                verileriExceleAktar(tbl_anaAviyonik, wsAdi);
            }
            else
                MessageBox.Show("Lütfen bir veri tablosu seçin.");
        }

        private void verileriExceleAktar(DataGridView dataGridView, string worksheetName)
        {
            try
            {
                using (SaveFileDialog sfd = new SaveFileDialog() { Filter = "Excel Workbook|*.xlsx", ValidateNames = true })
                {
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        using (XLWorkbook wb = new XLWorkbook())
                        {
                            var worksheet = wb.Worksheets.Add(worksheetName);

                            for (int j = 0; j < dataGridView.Columns.Count; j++)
                                worksheet.Cell(1, j + 1).Value = dataGridView.Columns[j].HeaderText;

                            if (dataGridView.Visible && dataGridView.DataSource != null && dataGridView.Rows.Count > 0)
                            {
                                for (int i = 0; i < dataGridView.Rows.Count; i++)
                                    for (int j = 0; j < dataGridView.Columns.Count; j++)
                                        worksheet.Cell(i + 2, j + 1).Value = dataGridView.Rows[i].Cells[j].Value?.ToString();
                            }

                            worksheet.Columns().AdjustToContents();
                            wb.SaveAs(sfd.FileName);
                        }
                        MessageBox.Show("Veriler Excel'e aktarıldı.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Excel'e aktarma hatası: " + ex.Message);
            }
        }


        // ====================================================================
        // COMBOBOX (radio buton + Testler desteği)
        // ====================================================================
        private void comboboxGuncelle()
        {
            cbox_veriTabloUcus.Items.Clear();

            string[] veriDosyalar;
            if (rbtn_anaAviyonik.Checked)
                veriDosyalar = UcusVeriYoneticisi.DosyalariListele("VeriAnaAv*.db");
            else if (rbtn_yedekAviyonik.Checked)
                veriDosyalar = UcusVeriYoneticisi.DosyalariListele("VeriYedekAv*.db");
            else if (rbtn_gorevYuku.Checked)
                veriDosyalar = UcusVeriYoneticisi.DosyalariListele("VeriGorevYuku*.db");
            else if (rbtn_testler.Checked)
                veriDosyalar = UcusVeriYoneticisi.DosyalariListele(TestTanimlari.TumTestDosyalariPattern);
            else
                veriDosyalar = new string[0];

            foreach (var veriDosya in veriDosyalar)
                cbox_veriTabloUcus.Items.Add(Path.GetFileNameWithoutExtension(veriDosya));

            cbox_veriTabloUcus.SelectedIndex = -1;
        }

        private void comboboxTiklanabilirlik()
        {
            bool herhangiSecili = rbtn_anaAviyonik.Checked || rbtn_yedekAviyonik.Checked
                                 || rbtn_gorevYuku.Checked || rbtn_testler.Checked;
            cbox_veriTabloUcus.Enabled = herhangiSecili;
        }


        // ====================================================================
        // RADIO BUTTON EVENT HANDLERS
        // ====================================================================
        private void rbtn_anaAviyonik_CheckedChanged(object sender, EventArgs e)
        {
            tbl_gorevYuku.Visible = false;
            tbl_anaAviyonik.Visible = true;
            tbl_yedekAviyonik.Visible = false;
            tbl_anaAviyonik.DataSource = null;
            comboboxGuncelle();
            comboboxTiklanabilirlik();
            currentTable = tbl_anaAviyonik;
        }

        private void rbtn_yedekAviyonik_CheckedChanged(object sender, EventArgs e)
        {
            tbl_gorevYuku.Visible = false;
            tbl_anaAviyonik.Visible = false;
            tbl_yedekAviyonik.Visible = true;
            tbl_yedekAviyonik.DataSource = null;
            comboboxGuncelle();
            comboboxTiklanabilirlik();
            currentTable = tbl_yedekAviyonik;
        }

        private void rbtn_gorevYuku_CheckedChanged(object sender, EventArgs e)
        {
            tbl_gorevYuku.Visible = true;
            tbl_anaAviyonik.Visible = false;
            tbl_yedekAviyonik.Visible = false;
            tbl_gorevYuku.DataSource = null;
            comboboxGuncelle();
            comboboxTiklanabilirlik();
            currentTable = tbl_gorevYuku;
        }

        // YENİ: Designer'da rbtn_testler oluşturulduktan sonra
        // CheckedChanged event'i bu handler'a bağlanmalı.
        private void rbtn_testler_CheckedChanged(object sender, EventArgs e)
        {
            // Test verileri için tbl_anaAviyonik'i geçici olarak yeniden kullanıyoruz.
            tbl_gorevYuku.Visible = false;
            tbl_yedekAviyonik.Visible = false;
            tbl_anaAviyonik.Visible = true;
            tbl_anaAviyonik.DataSource = null;
            comboboxGuncelle();
            comboboxTiklanabilirlik();
            currentTable = tbl_anaAviyonik;
        }

        private void tbl_yedekAviyonik_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void tbl_anaAviyonik_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void tbl_gorevYuku_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void uc_VeriTablo_Load(object sender, EventArgs e) { }
    }
}