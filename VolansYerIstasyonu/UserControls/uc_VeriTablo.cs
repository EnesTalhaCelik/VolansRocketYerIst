using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
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


        private static string anaAviyonikDatabaseDosyaYolu = "UcusVeri/veriAnaAv.db";
        private static string yedekAviyonikDatabaseDosyaYolu = "UcusVeri/veriYedekAv.db";
        private static string gorevYukuDatabaseDosyaYolu = "UcusVeri/veriGorevYuku.db";
        private static string anaAviyonikTableConnectionString;
        private static string yedekAviyonikTableConnectionString;
        private static string gorevYukuTableConnectionString;
        private static DataGridView currentTable;



        public uc_VeriTablo()
        {
            InitializeComponent();
            baslangicCalistir();
            
        }


        public void baslangicCalistir()
        {

            //databaseComboboxDoldur();
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
            string secilenVeritabaniYolu = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "UcusVeri", secilenVeritabaniAdi + ".db");
            string secilenVeritabaniBaglantiDizesi = $"Data Source={secilenVeritabaniYolu};Version=3;";

            using (SQLiteConnection connection = new SQLiteConnection(secilenVeritabaniBaglantiDizesi))
            {
                connection.Open();

                string query = "";
                if (rbtn_anaAviyonik.Checked)
                {
                    query = "SELECT * FROM [AnaAviyonikTablosu]";
                }
                else if (rbtn_yedekAviyonik.Checked)
                {
                    query = "SELECT * FROM [YedekAviyonikTablosu]";
                }
                else
                {
                    query = "SELECT * FROM [GorevYukuTablosu]";
                }

                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, connection))
                {
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    
                    Dictionary<string, string> columnHeaders = new Dictionary<string, string>
            {
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
                { "geiger_veri", "Geiger Veri" }
            };

                    
                    foreach (DataColumn column in dataTable.Columns)
                    {
                        if (columnHeaders.ContainsKey(column.ColumnName))
                        {
                            column.ColumnName = columnHeaders[column.ColumnName];
                        }
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

                    if (rbtn_anaAviyonik.Checked)
                    {
                        tbl_anaAviyonik.DataSource = dataTable;
                    }
                    else if (rbtn_yedekAviyonik.Checked)
                    {
                        tbl_yedekAviyonik.DataSource = dataTable;
                    }
                    else
                    {
                        tbl_gorevYuku.DataSource = dataTable;
                    }

                    
                    foreach (DataGridViewColumn column in tbl_anaAviyonik.Columns)
                    {
                        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    }
                    foreach (DataGridViewColumn column in tbl_yedekAviyonik.Columns)
                    {
                        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    }
                    foreach (DataGridViewColumn column in tbl_gorevYuku.Columns)
                    {
                        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    }
                }

                connection.Close();
            }
        }





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
                string veriPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "UcusVeri", databaseAdi);

                if (!Directory.Exists(Path.GetDirectoryName(anaAviyonikDatabaseDosyaYolu)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(anaAviyonikDatabaseDosyaYolu));
                }

                connectionString = $"Data Source={veriPath};Version=3;";

                if (!File.Exists(veriPath))
                {
                    SQLiteConnection.CreateFile(veriPath);

                    using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                    {
                        connection.Open();

                        string table = @"
                            CREATE TABLE IF NOT EXISTS [AnaAviyonikTablosu] (
                                durum INTEGER, 
                                roket_irtifa_basinc REAL, 
                                dogrulama_kodu INTEGER, 
                                paket_sayaci INTEGER, 
                                roket_basinc REAL, 
                                roket_sicaklik REAL, 
                                roket_hiz_x REAL, 
                                roket_hiz_y REAL, 
                                roket_hiz_z REAL, 
                                roket_acisal_hiz_x REAL, 
                                roket_acisal_hiz_y REAL, 
                                roket_acisal_hiz_z REAL, 
                                jiroskop_x REAL, 
                                jiroskop_y REAL, 
                                jiroskop_z REAL, 
                                ivme_x REAL, 
                                ivme_y REAL, 
                                ivme_z REAL, 
                                aci REAL, 
                                roket_gps_enlem REAL, 
                                roket_gps_boylam REAL, 
                                roket_gps_yukseklik REAL,
                                zaman TEXT
                            )";
                        SQLiteCommand command = new SQLiteCommand(table, connection);
                        command.ExecuteNonQuery();

                        connection.Close();
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
            string connectionString = anaAviyonikTableConnectionString;

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    string insertQuery = "INSERT INTO [AnaAviyonikTablosu] (durum, roket_irtifa_basinc, dogrulama_kodu, paket_sayaci," +
                        " roket_basinc, roket_sicaklik, roket_hiz_x, roket_hiz_y, roket_hiz_z, roket_acisal_hiz_x, roket_acisal_hiz_y," +
                        " roket_acisal_hiz_z, jiroskop_x, jiroskop_y, jiroskop_z, ivme_x, ivme_y, ivme_z, aci, roket_gps_enlem," +
                        " roket_gps_boylam, roket_gps_yukseklik, zaman) VALUES (@durum, @roket_irtifa_basinc, @dogrulama_kodu, @paket_sayaci," +
                        " @roket_basinc, @roket_sicaklik, @roket_hiz_x, @roket_hiz_y, @roket_hiz_z, @roket_acisal_hiz_x, @roket_acisal_hiz_y," +
                        " @roket_acisal_hiz_z, @jiroskop_x, @jiroskop_y, @jiroskop_z, @ivme_x, @ivme_y, @ivme_z, @aci, @roket_gps_enlem," +
                        " @roket_gps_boylam, @roket_gps_yukseklik, @zaman)";
                    SQLiteCommand anaAviyonikVeriEkleme = new SQLiteCommand(insertQuery, connection);
                    anaAviyonikVeriEkleme.Parameters.AddWithValue("@durum", durum);
                    anaAviyonikVeriEkleme.Parameters.AddWithValue("@roket_irtifa_basinc", roket_irtifa_basinc);
                    anaAviyonikVeriEkleme.Parameters.AddWithValue("@dogrulama_kodu", dogrulama_kodu);
                    anaAviyonikVeriEkleme.Parameters.AddWithValue("@paket_sayaci", paket_sayaci);
                    anaAviyonikVeriEkleme.Parameters.AddWithValue("@roket_basinc", roket_basinc);
                    anaAviyonikVeriEkleme.Parameters.AddWithValue("@roket_sicaklik", roket_sicaklik);
                    anaAviyonikVeriEkleme.Parameters.AddWithValue("@roket_hiz_x", roket_hiz_x);
                    anaAviyonikVeriEkleme.Parameters.AddWithValue("@roket_hiz_y", roket_hiz_y);
                    anaAviyonikVeriEkleme.Parameters.AddWithValue("@roket_hiz_z", roket_hiz_z);
                    anaAviyonikVeriEkleme.Parameters.AddWithValue("@roket_acisal_hiz_x", roket_acisal_hiz_x);
                    anaAviyonikVeriEkleme.Parameters.AddWithValue("@roket_acisal_hiz_y", roket_acisal_hiz_y);
                    anaAviyonikVeriEkleme.Parameters.AddWithValue("@roket_acisal_hiz_z", roket_acisal_hiz_z);
                    anaAviyonikVeriEkleme.Parameters.AddWithValue("@jiroskop_x", jiroskop_x);
                    anaAviyonikVeriEkleme.Parameters.AddWithValue("@jiroskop_y", jiroskop_y);
                    anaAviyonikVeriEkleme.Parameters.AddWithValue("@jiroskop_z", jiroskop_z);
                    anaAviyonikVeriEkleme.Parameters.AddWithValue("@ivme_x", ivme_x);
                    anaAviyonikVeriEkleme.Parameters.AddWithValue("@ivme_y", ivme_y);
                    anaAviyonikVeriEkleme.Parameters.AddWithValue("@ivme_z", ivme_z);
                    anaAviyonikVeriEkleme.Parameters.AddWithValue("@aci", aci);
                    anaAviyonikVeriEkleme.Parameters.AddWithValue("@roket_gps_enlem", roket_gps_enlem);
                    anaAviyonikVeriEkleme.Parameters.AddWithValue("@roket_gps_boylam", roket_gps_boylam);
                    anaAviyonikVeriEkleme.Parameters.AddWithValue("@roket_gps_yukseklik", roket_gps_yukseklik);
                    anaAviyonikVeriEkleme.Parameters.AddWithValue("@zaman", zaman);

                    anaAviyonikVeriEkleme.ExecuteNonQuery();

                    connection.Close();
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
            string connectionString = anaAviyonikTableConnectionString;

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    string insertQuery = "INSERT INTO [AnaAviyonikTablosu] (durum, roket_irtifa_basinc, dogrulama_kodu, paket_sayaci," +
                        " roket_basinc, roket_sicaklik, roket_hiz_x, roket_hiz_y, roket_hiz_z, roket_acisal_hiz_x, roket_acisal_hiz_y," +
                        " roket_acisal_hiz_z, jiroskop_x, jiroskop_y, jiroskop_z, ivme_x, ivme_y, ivme_z, aci, roket_gps_enlem," +
                        " roket_gps_boylam, roket_gps_yukseklik) VALUES (@durum, @roket_irtifa_basinc, @dogrulama_kodu, @paket_sayaci," +
                        " @roket_basinc, @roket_sicaklik, @roket_hiz_x, @roket_hiz_y, @roket_hiz_z, @roket_acisal_hiz_x, @roket_acisal_hiz_y," +
                        " @roket_acisal_hiz_z, @jiroskop_x, @jiroskop_y, @jiroskop_z, @ivme_x, @ivme_y, @ivme_z, @aci, @roket_gps_enlem," +
                        " @roket_gps_boylam, @roket_gps_yukseklik)";
                    SQLiteCommand anaAviyonikVeriEkleme = new SQLiteCommand(insertQuery, connection);
                    anaAviyonikVeriEkleme.Parameters.AddWithValue("@durum", durum);
                    anaAviyonikVeriEkleme.Parameters.AddWithValue("@roket_irtifa_basinc", roket_irtifa_basinc);
                    anaAviyonikVeriEkleme.Parameters.AddWithValue("@dogrulama_kodu", dogrulama_kodu);
                    anaAviyonikVeriEkleme.Parameters.AddWithValue("@paket_sayaci", paket_sayaci);
                    anaAviyonikVeriEkleme.Parameters.AddWithValue("@roket_basinc", roket_basinc);
                    anaAviyonikVeriEkleme.Parameters.AddWithValue("@roket_sicaklik", roket_sicaklik);
                    anaAviyonikVeriEkleme.Parameters.AddWithValue("@roket_hiz_x", roket_hiz_x);
                    anaAviyonikVeriEkleme.Parameters.AddWithValue("@roket_hiz_y", roket_hiz_y);
                    anaAviyonikVeriEkleme.Parameters.AddWithValue("@roket_hiz_z", roket_hiz_z);
                    anaAviyonikVeriEkleme.Parameters.AddWithValue("@roket_acisal_hiz_x", roket_acisal_hiz_x);
                    anaAviyonikVeriEkleme.Parameters.AddWithValue("@roket_acisal_hiz_y", roket_acisal_hiz_y);
                    anaAviyonikVeriEkleme.Parameters.AddWithValue("@roket_acisal_hiz_z", roket_acisal_hiz_z);
                    anaAviyonikVeriEkleme.Parameters.AddWithValue("@jiroskop_x", jiroskop_x);
                    anaAviyonikVeriEkleme.Parameters.AddWithValue("@jiroskop_y", jiroskop_y);
                    anaAviyonikVeriEkleme.Parameters.AddWithValue("@jiroskop_z", jiroskop_z);
                    anaAviyonikVeriEkleme.Parameters.AddWithValue("@ivme_x", ivme_x);
                    anaAviyonikVeriEkleme.Parameters.AddWithValue("@ivme_y", ivme_y);
                    anaAviyonikVeriEkleme.Parameters.AddWithValue("@ivme_z", ivme_z);
                    anaAviyonikVeriEkleme.Parameters.AddWithValue("@aci", aci);
                    anaAviyonikVeriEkleme.Parameters.AddWithValue("@roket_gps_enlem", roket_gps_enlem);
                    anaAviyonikVeriEkleme.Parameters.AddWithValue("@roket_gps_boylam", roket_gps_boylam);
                    anaAviyonikVeriEkleme.Parameters.AddWithValue("@roket_gps_yukseklik", roket_gps_yukseklik);
                    anaAviyonikVeriEkleme.Parameters.AddWithValue("@zaman", DateTime.Now);

                    anaAviyonikVeriEkleme.ExecuteNonQuery();

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ana Aviyonik veri eklemesinde hata: " + ex.Message);
            }
        }



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
                string veriPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "UcusVeri", databaseAdi);

                if (!Directory.Exists(Path.GetDirectoryName(yedekAviyonikDatabaseDosyaYolu)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(yedekAviyonikDatabaseDosyaYolu));
                }

                connectionString = $"Data Source={veriPath};Version=3;";

                if (!File.Exists(veriPath))
                {
                    SQLiteConnection.CreateFile(veriPath);

                    using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                    {
                        connection.Open();

                        string table = @"
                            CREATE TABLE IF NOT EXISTS [YedekAviyonikTablosu] (
                                durum INTEGER, 
                                roket_irtifa_basinc REAL, 
                                dogrulama_kodu INTEGER, 
                                paket_sayaci INTEGER, 
                                roket_basinc REAL, 
                                roket_sicaklik REAL, 
                                roket_hiz_x REAL, 
                                roket_hiz_y REAL, 
                                roket_hiz_z REAL, 
                                roket_acisal_hiz_x REAL, 
                                roket_acisal_hiz_y REAL, 
                                roket_acisal_hiz_z REAL, 
                                jiroskop_x REAL, 
                                jiroskop_y REAL, 
                                jiroskop_z REAL, 
                                ivme_x REAL, 
                                ivme_y REAL, 
                                ivme_z REAL, 
                                aci REAL, 
                                roket_gps_enlem REAL, 
                                roket_gps_boylam REAL, 
                                roket_gps_yukseklik REAL,
                                zaman TEXT
                            )";
                        SQLiteCommand command = new SQLiteCommand(table, connection);
                        command.ExecuteNonQuery();

                        connection.Close();
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
            string connectionString = yedekAviyonikTableConnectionString;

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    string insertQuery = "INSERT INTO [YedekAviyonikTablosu] (durum, roket_irtifa_basinc, dogrulama_kodu, paket_sayaci," +
                        " roket_basinc, roket_sicaklik, roket_hiz_x, roket_hiz_y, roket_hiz_z, roket_acisal_hiz_x, roket_acisal_hiz_y," +
                        " roket_acisal_hiz_z, jiroskop_x, jiroskop_y, jiroskop_z, ivme_x, ivme_y, ivme_z, aci, roket_gps_enlem," +
                        " roket_gps_boylam, roket_gps_yukseklik, zaman) VALUES (@durum, @roket_irtifa_basinc, @dogrulama_kodu, @paket_sayaci," +
                        " @roket_basinc, @roket_sicaklik, @roket_hiz_x, @roket_hiz_y, @roket_hiz_z, @roket_acisal_hiz_x, @roket_acisal_hiz_y," +
                        " @roket_acisal_hiz_z, @jiroskop_x, @jiroskop_y, @jiroskop_z, @ivme_x, @ivme_y, @ivme_z, @aci, @roket_gps_enlem," +
                        " @roket_gps_boylam, @roket_gps_yukseklik, @zaman)";
                    SQLiteCommand yedekAviyonikVeriEkleme = new SQLiteCommand(insertQuery, connection);
                    yedekAviyonikVeriEkleme.Parameters.AddWithValue("@durum", durum);
                    yedekAviyonikVeriEkleme.Parameters.AddWithValue("@roket_irtifa_basinc", roket_irtifa_basinc);
                    yedekAviyonikVeriEkleme.Parameters.AddWithValue("@dogrulama_kodu", dogrulama_kodu);
                    yedekAviyonikVeriEkleme.Parameters.AddWithValue("@paket_sayaci", paket_sayaci);
                    yedekAviyonikVeriEkleme.Parameters.AddWithValue("@roket_basinc", roket_basinc);
                    yedekAviyonikVeriEkleme.Parameters.AddWithValue("@roket_sicaklik", roket_sicaklik);
                    yedekAviyonikVeriEkleme.Parameters.AddWithValue("@roket_hiz_x", roket_hiz_x);
                    yedekAviyonikVeriEkleme.Parameters.AddWithValue("@roket_hiz_y", roket_hiz_y);
                    yedekAviyonikVeriEkleme.Parameters.AddWithValue("@roket_hiz_z", roket_hiz_z);
                    yedekAviyonikVeriEkleme.Parameters.AddWithValue("@roket_acisal_hiz_x", roket_acisal_hiz_x);
                    yedekAviyonikVeriEkleme.Parameters.AddWithValue("@roket_acisal_hiz_y", roket_acisal_hiz_y);
                    yedekAviyonikVeriEkleme.Parameters.AddWithValue("@roket_acisal_hiz_z", roket_acisal_hiz_z);
                    yedekAviyonikVeriEkleme.Parameters.AddWithValue("@jiroskop_x", jiroskop_x);
                    yedekAviyonikVeriEkleme.Parameters.AddWithValue("@jiroskop_y", jiroskop_y);
                    yedekAviyonikVeriEkleme.Parameters.AddWithValue("@jiroskop_z", jiroskop_z);
                    yedekAviyonikVeriEkleme.Parameters.AddWithValue("@ivme_x", ivme_x);
                    yedekAviyonikVeriEkleme.Parameters.AddWithValue("@ivme_y", ivme_y);
                    yedekAviyonikVeriEkleme.Parameters.AddWithValue("@ivme_z", ivme_z);
                    yedekAviyonikVeriEkleme.Parameters.AddWithValue("@aci", aci);
                    yedekAviyonikVeriEkleme.Parameters.AddWithValue("@roket_gps_enlem", roket_gps_enlem);
                    yedekAviyonikVeriEkleme.Parameters.AddWithValue("@roket_gps_boylam", roket_gps_boylam);
                    yedekAviyonikVeriEkleme.Parameters.AddWithValue("@roket_gps_yukseklik", roket_gps_yukseklik);
                    yedekAviyonikVeriEkleme.Parameters.AddWithValue("@roket_gps_yukseklik", roket_gps_yukseklik);
                    yedekAviyonikVeriEkleme.Parameters.AddWithValue("@zaman", zaman);

                    yedekAviyonikVeriEkleme.ExecuteNonQuery();

                    connection.Close();
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
            string connectionString = yedekAviyonikTableConnectionString;

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    string insertQuery = "INSERT INTO [YedekAviyonikTablosu] (durum, roket_irtifa_basinc, dogrulama_kodu, paket_sayaci," +
                        " roket_basinc, roket_sicaklik, roket_hiz_x, roket_hiz_y, roket_hiz_z, roket_acisal_hiz_x, roket_acisal_hiz_y," +
                        " roket_acisal_hiz_z, jiroskop_x, jiroskop_y, jiroskop_z, ivme_x, ivme_y, ivme_z, aci, roket_gps_enlem," +
                        " roket_gps_boylam, roket_gps_yukseklik, zaman) VALUES (@durum, @roket_irtifa_basinc, @dogrulama_kodu, @paket_sayaci," +
                        " @roket_basinc, @roket_sicaklik, @roket_hiz_x, @roket_hiz_y, @roket_hiz_z, @roket_acisal_hiz_x, @roket_acisal_hiz_y," +
                        " @roket_acisal_hiz_z, @jiroskop_x, @jiroskop_y, @jiroskop_z, @ivme_x, @ivme_y, @ivme_z, @aci, @roket_gps_enlem," +
                        " @roket_gps_boylam, @roket_gps_yukseklik, @zaman)";
                    SQLiteCommand yedekAviyonikVeriEkleme = new SQLiteCommand(insertQuery, connection);
                    yedekAviyonikVeriEkleme.Parameters.AddWithValue("@durum", durum);
                    yedekAviyonikVeriEkleme.Parameters.AddWithValue("@roket_irtifa_basinc", roket_irtifa_basinc);
                    yedekAviyonikVeriEkleme.Parameters.AddWithValue("@dogrulama_kodu", dogrulama_kodu);
                    yedekAviyonikVeriEkleme.Parameters.AddWithValue("@paket_sayaci", paket_sayaci);
                    yedekAviyonikVeriEkleme.Parameters.AddWithValue("@roket_basinc", roket_basinc);
                    yedekAviyonikVeriEkleme.Parameters.AddWithValue("@roket_sicaklik", roket_sicaklik);
                    yedekAviyonikVeriEkleme.Parameters.AddWithValue("@roket_hiz_x", roket_hiz_x);
                    yedekAviyonikVeriEkleme.Parameters.AddWithValue("@roket_hiz_y", roket_hiz_y);
                    yedekAviyonikVeriEkleme.Parameters.AddWithValue("@roket_hiz_z", roket_hiz_z);
                    yedekAviyonikVeriEkleme.Parameters.AddWithValue("@roket_acisal_hiz_x", roket_acisal_hiz_x);
                    yedekAviyonikVeriEkleme.Parameters.AddWithValue("@roket_acisal_hiz_y", roket_acisal_hiz_y);
                    yedekAviyonikVeriEkleme.Parameters.AddWithValue("@roket_acisal_hiz_z", roket_acisal_hiz_z);
                    yedekAviyonikVeriEkleme.Parameters.AddWithValue("@jiroskop_x", jiroskop_x);
                    yedekAviyonikVeriEkleme.Parameters.AddWithValue("@jiroskop_y", jiroskop_y);
                    yedekAviyonikVeriEkleme.Parameters.AddWithValue("@jiroskop_z", jiroskop_z);
                    yedekAviyonikVeriEkleme.Parameters.AddWithValue("@ivme_x", ivme_x);
                    yedekAviyonikVeriEkleme.Parameters.AddWithValue("@ivme_y", ivme_y);
                    yedekAviyonikVeriEkleme.Parameters.AddWithValue("@ivme_z", ivme_z);
                    yedekAviyonikVeriEkleme.Parameters.AddWithValue("@aci", aci);
                    yedekAviyonikVeriEkleme.Parameters.AddWithValue("@roket_gps_enlem", roket_gps_enlem);
                    yedekAviyonikVeriEkleme.Parameters.AddWithValue("@roket_gps_boylam", roket_gps_boylam);
                    yedekAviyonikVeriEkleme.Parameters.AddWithValue("@roket_gps_yukseklik", roket_gps_yukseklik);
                    yedekAviyonikVeriEkleme.Parameters.AddWithValue("@roket_gps_yukseklik", roket_gps_yukseklik);
                    yedekAviyonikVeriEkleme.Parameters.AddWithValue("@zaman", DateTime.Now);

                    yedekAviyonikVeriEkleme.ExecuteNonQuery();

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Yedek Aviyonik veri eklemesinde hata: " + ex.Message);
            }
        }




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
                string veriPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "UcusVeri", databaseAdi);

                if (!Directory.Exists(Path.GetDirectoryName(gorevYukuDatabaseDosyaYolu)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(gorevYukuDatabaseDosyaYolu));
                }

                connectionString = $"Data Source={veriPath};Version=3;";

                if (!File.Exists(veriPath))
                {
                    SQLiteConnection.CreateFile(veriPath);

                    using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                    {
                        connection.Open();

                        string table = @"
                            CREATE TABLE IF NOT EXISTS [GorevYukuTablosu] (
                                gps_irtifa REAL, 
                                basinc_irtifa REAL, 
                                gps_enlem REAL, 
                                gps_boylam REAL, 
                                geiger_veri REAL,
                                paket_sayaci INTEGER,
                                dogrulama_kodu INTEGER,
                                zaman TEXT
                            )";
                        SQLiteCommand command = new SQLiteCommand(table, connection);
                        command.ExecuteNonQuery();

                        connection.Close();
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
            string connectionString = gorevYukuTableConnectionString;

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    string insertQuery = "INSERT INTO [GorevYukuTablosu] (gps_irtifa, basinc_irtifa, gps_enlem, gps_boylam," +
                        " geiger_veri, paket_sayaci, dogrulama_kodu, zaman) VALUES (@gps_irtifa, @basinc_irtifa, @gps_enlem," +
                        " @gps_boylam, @geiger_veri, @paket_sayaci, @dogrulama_kodu, @zaman)";
                    SQLiteCommand gorevYukuVeriEkleme = new SQLiteCommand(insertQuery, connection);
                    gorevYukuVeriEkleme.Parameters.AddWithValue("@gps_irtifa", gps_irtifa);
                    gorevYukuVeriEkleme.Parameters.AddWithValue("@basinc_irtifa", basinc_irtifa);
                    gorevYukuVeriEkleme.Parameters.AddWithValue("@gps_enlem", gps_enlem);
                    gorevYukuVeriEkleme.Parameters.AddWithValue("@gps_boylam", gps_boylam);
                    gorevYukuVeriEkleme.Parameters.AddWithValue("@geiger_veri", geiger_veri);
                    gorevYukuVeriEkleme.Parameters.AddWithValue("@paket_sayaci", paket_sayaci);
                    gorevYukuVeriEkleme.Parameters.AddWithValue("@dogrulama_kodu", dogrulama_kodu);
                    gorevYukuVeriEkleme.Parameters.AddWithValue("@zaman", zaman);
                    gorevYukuVeriEkleme.ExecuteNonQuery();

                    connection.Close();
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
            string connectionString = gorevYukuTableConnectionString;

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    string insertQuery = "INSERT INTO [GorevYukuTablosu] (gps_irtifa, basinc_irtifa, gps_enlem, gps_boylam," +
                        " geiger_veri, paket_sayaci, dogrulama_kodu, zaman) VALUES (@gps_irtifa, @basinc_irtifa, @gps_enlem," +
                        " @gps_boylam, @geiger_veri, @paket_sayaci, @dogrulama_kodu, @zaman)";
                    SQLiteCommand gorevYukuVeriEkleme = new SQLiteCommand(insertQuery, connection);
                    gorevYukuVeriEkleme.Parameters.AddWithValue("@gps_irtifa", gps_irtifa);
                    gorevYukuVeriEkleme.Parameters.AddWithValue("@basinc_irtifa", basinc_irtifa);
                    gorevYukuVeriEkleme.Parameters.AddWithValue("@gps_enlem", gps_enlem);
                    gorevYukuVeriEkleme.Parameters.AddWithValue("@gps_boylam", gps_boylam);
                    gorevYukuVeriEkleme.Parameters.AddWithValue("@geiger_veri", geiger_veri);
                    gorevYukuVeriEkleme.Parameters.AddWithValue("@paket_sayaci", paket_sayaci);
                    gorevYukuVeriEkleme.Parameters.AddWithValue("@dogrulama_kodu", dogrulama_kodu);
                    gorevYukuVeriEkleme.Parameters.AddWithValue("@zaman", DateTime.Now);
                    gorevYukuVeriEkleme.ExecuteNonQuery();

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Görev Yükü veri eklemesinde hata: " + ex.Message);
            }
        }





        private void btn_verileriExceleAktar_Click(object sender, EventArgs e)
        {
            if (rbtn_anaAviyonik.Checked)
            {
                verileriExceleAktar(tbl_anaAviyonik, "AnaAviyonikVerileri");
            }
            else if (rbtn_yedekAviyonik.Checked)
            {
                verileriExceleAktar(tbl_yedekAviyonik, "YedekAviyonikVerileri");
            }
            else if (rbtn_gorevYuku.Checked)
            {
                verileriExceleAktar(tbl_gorevYuku, "GorevYukuVerileri");
            }
            else
            {
                MessageBox.Show("Lütfen bir veri tablosu seçin.");
            }
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
                            
                            if (dataGridView.Visible && dataGridView.DataSource != null && dataGridView.Rows.Count > 0)
                            {
                                var worksheet = wb.Worksheets.Add(worksheetName);

                                
                                for (int j = 0; j < dataGridView.Columns.Count; j++)
                                {
                                    worksheet.Cell(1, j + 1).Value = dataGridView.Columns[j].HeaderText;
                                }

                                
                                for (int i = 0; i < dataGridView.Rows.Count; i++)
                                {
                                    for (int j = 0; j < dataGridView.Columns.Count; j++)
                                    {
                                        worksheet.Cell(i + 2, j + 1).Value = dataGridView.Rows[i].Cells[j].Value?.ToString();
                                    }
                                }

                                
                                worksheet.Columns().AdjustToContents();
                            }
                            else
                            {
                                
                                var worksheet = wb.Worksheets.Add(worksheetName);

                                
                                for (int j = 0; j < dataGridView.Columns.Count; j++)
                                {
                                    worksheet.Cell(1, j + 1).Value = dataGridView.Columns[j].HeaderText;
                                }

                                
                                worksheet.Columns().AdjustToContents();
                            }

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








        private void comboboxGuncelle()
        {
            cbox_veriTabloUcus.Items.Clear();

            string[] veriDosyalar;
            if (rbtn_anaAviyonik.Checked)
            {
                veriDosyalar = Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "UcusVeri"), "VeriAnaAv*.db");
            }
            else if (rbtn_yedekAviyonik.Checked)
            {
                veriDosyalar = Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "UcusVeri"), "VeriYedekAv*.db");
            }
            else
            {
                veriDosyalar = Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "UcusVeri"), "VeriGorevYuku*.db");
            }

            foreach (var veriDosya in veriDosyalar)
            {
                cbox_veriTabloUcus.Items.Add(Path.GetFileNameWithoutExtension(veriDosya));
            }

            cbox_veriTabloUcus.SelectedIndex = -1;
        }




        private void comboboxTiklanabilirlik()
        {
            if (!rbtn_anaAviyonik.Checked && !rbtn_yedekAviyonik.Checked && !rbtn_gorevYuku.Checked)
            {
                cbox_veriTabloUcus.Enabled = false;
            }
            else
            {
                cbox_veriTabloUcus.Enabled = true;
            }
        }


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


        
        private void tbl_yedekAviyonik_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }


        private void tbl_anaAviyonik_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }


        private void tbl_gorevYuku_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        
    }
}
