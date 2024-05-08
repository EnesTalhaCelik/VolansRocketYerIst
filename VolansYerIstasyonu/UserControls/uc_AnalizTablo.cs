using ClosedXML.Excel;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Spreadsheet;
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

namespace VolansYerIstasyonu.UserControls
{
    public partial class uc_AnalizTablo : UserControl


    {
        private Dictionary<string, List<string>> databaseFiles = new Dictionary<string, List<string>>();

        private string basincDatabaseDosyaYolu = "UcusVeri/BasincTablosu.db";
        private string aciDatabaseDosyaYolu = "UcusVeri/AciTablosu.db";


        public uc_AnalizTablo()
        {
            InitializeComponent();
            baslangicCalistir();
            
        }

        public string basincDatabaseOlustur()
        {
            string basincConnectionString = string.Empty;
            try
            {
                string ucusDatabaseAdi = $"basinc{DateTime.Now:yyyyMMdd_HHmmss}.db";
                string ucusVeriPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "UcusVeri", ucusDatabaseAdi);
                
                if (!Directory.Exists(Path.GetDirectoryName(basincDatabaseDosyaYolu)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(basincDatabaseDosyaYolu));
                }

                basincConnectionString = $"Data Source={ucusVeriPath};Version=3;";

                if (!File.Exists(ucusVeriPath))
                {
                    SQLiteConnection.CreateFile(ucusVeriPath);

                    using (SQLiteConnection connection = new SQLiteConnection(basincConnectionString))
                    {
                        connection.Open();

                        string basincTablosu = "CREATE TABLE IF NOT EXISTS [Basinc Tablosu] (basinc REAL, durum BOOLEAN, zaman DATETIME)";
                        SQLiteCommand basincTablosuOlustur = new SQLiteCommand(basincTablosu, connection);
                        basincTablosuOlustur.ExecuteNonQuery();

                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Basınç veritabanı oluşturmada hata: " + ex.Message);
            }
            return basincConnectionString;
        }

        public string aciDatabaseOlustur()
        {
            string aciConnectionString = string.Empty;
            try
            {
                string ucusDatabaseAdi = $"aci{DateTime.Now:yyyyMMdd_HHmmss}.db";
                string ucusVeriPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "UcusVeri", ucusDatabaseAdi);

                if (!Directory.Exists(Path.GetDirectoryName(aciDatabaseDosyaYolu)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(aciDatabaseDosyaYolu));
                }

                aciConnectionString = $"Data Source={ucusVeriPath};Version=3;";

                if (!File.Exists(ucusVeriPath))
                {
                    SQLiteConnection.CreateFile(ucusVeriPath);

                    using (SQLiteConnection connection = new SQLiteConnection(aciConnectionString))
                    {
                        connection.Open();

                        string aciTablosu = "CREATE TABLE IF NOT EXISTS [Aci Tablosu] (X REAL, Y REAL, Z REAL, durum BOOLEAN, zaman DATETIME)";
                        SQLiteCommand aciTablosuOlustur = new SQLiteCommand(aciTablosu, connection);
                        aciTablosuOlustur.ExecuteNonQuery();

                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Açı veritabanı oluşturmada hata: " + ex.Message);
            }
            return aciConnectionString;
        }

        public void basincVeriEkle(double basinc, bool durum, DateTime zaman)
        {
            string connectionString = basincDatabaseOlustur();

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    string insertQuery = "INSERT INTO [Basinc Tablosu] (basinc, durum, zaman) VALUES (@basinc, @durum, @zaman)";
                    SQLiteCommand basincVeriEkleme = new SQLiteCommand(insertQuery, connection);
                    basincVeriEkleme.Parameters.AddWithValue("@basinc", basinc);
                    basincVeriEkleme.Parameters.AddWithValue("@durum", durum);
                    basincVeriEkleme.Parameters.AddWithValue("@zaman", zaman);
                    basincVeriEkleme.ExecuteNonQuery();

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Basınç veri eklemesinde hata: " + ex.Message);
            }
        }

        public void basincVeriEkle(double basinc, bool durum)
        {
            string connectionString = basincDatabaseOlustur();

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    string insertQuery = "INSERT INTO [Basinc Tablosu] (basinc, durum, zaman) VALUES (@basinc, @durum, @zaman)";
                    SQLiteCommand basincVeriEkleme = new SQLiteCommand(insertQuery, connection);
                    basincVeriEkleme.Parameters.AddWithValue("@basinc", basinc);
                    basincVeriEkleme.Parameters.AddWithValue("@durum", durum);
                    basincVeriEkleme.Parameters.AddWithValue("@zaman", DateTime.Now);
                    basincVeriEkleme.ExecuteNonQuery();

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Basınç veri eklemesinde hata: " + ex.Message);
            }
        }

        public void aciVeriEkle(double x, double y, double z, bool durum, DateTime zaman)
        {
            string aciConnectionString = aciDatabaseOlustur();

            try
            {

                using (SQLiteConnection connection = new SQLiteConnection(aciConnectionString))
                {
                    connection.Open();

                    string insertQuery = "INSERT INTO [Aci Tablosu] (X, Y, Z, durum, zaman) VALUES (@x, @y, @z, @durum, @zaman)";
                    SQLiteCommand aciVeriEkleme = new SQLiteCommand(insertQuery, connection);
                    aciVeriEkleme.Parameters.AddWithValue("@x", x);
                    aciVeriEkleme.Parameters.AddWithValue("@y", y);
                    aciVeriEkleme.Parameters.AddWithValue("@z", z);
                    aciVeriEkleme.Parameters.AddWithValue("@durum", durum);
                    aciVeriEkleme.Parameters.AddWithValue("@zaman", zaman);
                    aciVeriEkleme.ExecuteNonQuery();

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Açı veri eklemesinde hata: " + ex.Message);
            }
        }

        public void aciVeriEkle(double x, double y, double z, bool durum)
        {
            string aciConnectionString = aciDatabaseOlustur();

            try
            {

                using (SQLiteConnection connection = new SQLiteConnection(aciConnectionString))
                {
                    connection.Open();

                    string insertQuery = "INSERT INTO [Aci Tablosu] (X, Y, Z, durum, zaman) VALUES (@x, @y, @z, @durum, @zaman)";
                    SQLiteCommand aciVeriEkleme = new SQLiteCommand(insertQuery, connection);
                    aciVeriEkleme.Parameters.AddWithValue("@x", x);
                    aciVeriEkleme.Parameters.AddWithValue("@y", y);
                    aciVeriEkleme.Parameters.AddWithValue("@z", z);
                    aciVeriEkleme.Parameters.AddWithValue("@durum", durum);
                    aciVeriEkleme.Parameters.AddWithValue("@zaman", DateTime.Now);
                    aciVeriEkleme.ExecuteNonQuery();

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Açı veri eklemesinde hata: " + ex.Message);
            }
        }

        public void veriBasincTablosunaYansit(string databaseDosyaYolu)
        {
            try
            {
                
                string connectionString = $"Data Source={databaseDosyaYolu};Version=3;";

                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    string selectQuery = $"SELECT basinc, CASE WHEN durum = 1 THEN 'Tetiklendi' ELSE 'Tetiklenmedi' END AS durum, zaman FROM [Basinc Tablosu]";
                    SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(selectQuery, connection);
                    System.Data.DataTable dataTable = new System.Data.DataTable();
                    dataAdapter.Fill(dataTable);
                    tbl_Basinc.DataSource = dataTable;

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veriyi basınç tablosuna yansıtmada hata: " + ex.Message);
            }
        }

        public void veriAciTablosunaYansit(string databaseDosyaYolu)
        {   

            try
            {
                
                string connectionString = $"Data Source={databaseDosyaYolu};Version=3;";

                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    string selectQuery = $"SELECT X, Y, Z, CASE WHEN durum = 1 THEN 'Tetiklendi' ELSE 'Tetiklenmedi' END AS durum, zaman FROM [Aci Tablosu]";
                    SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(selectQuery, connection);
                    System.Data.DataTable dataTable = new System.Data.DataTable();
                    dataAdapter.Fill(dataTable);
                    tbl_Aci.DataSource = dataTable;

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veriyi açı tablosuna yansıtmada hata: " + ex.Message);
            }
        }

        private void databaseComboboxDoldur()
        {
            string ucusVeriPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "UcusVeri");
            string[] dbFiles = Directory.GetFiles(ucusVeriPath, "*.db");

            cbox_analizTabloUcus.Items.Clear();

            foreach (string dbFile in dbFiles)
            {
                string fileName = Path.GetFileName(dbFile);

                cbox_analizTabloUcus.Items.Add(fileName);
            }
        }

        private void comboboxGuncelle()
        {
            cbox_analizTabloUcus.Items.Clear();

            string selectedDataType = secilmisDataTurunuAl();

            if (selectedDataType != null)
            {
                string ucusVeriPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "UcusVeri");
                string[] dbFiles = Directory.GetFiles(ucusVeriPath, "*.db");

                foreach (string dbFile in dbFiles)
                {
                    string fileName = Path.GetFileName(dbFile);

                    if (fileName.StartsWith(selectedDataType))
                    {
                        cbox_analizTabloUcus.Items.Add(fileName);
                    }
                }
            }
        }

        private string secilmisDataTurunuAl()
        {
            if (rbtn_Basinc.Checked)
            {
                return "basinc";
            }
            else if (rbtn_Aci.Checked)
            {
                return "aci";
            }
            return null;
        }

        private void secilenDatabaseYansit()
        {
            string secilenDatabase = cbox_analizTabloUcus.SelectedItem as string;

            if (secilenDatabase != null)
            {
                veriTablosunaYansit(secilenDatabase);
            }
        }

        private void veriTablosunaYansit(string selectedDatabase)
        {
            string veritabanıYolu = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "UcusVeri", selectedDatabase);


            if (rbtn_Basinc.Checked)
            {
                veriBasincTablosunaYansit(veritabanıYolu);
            }
            else if (rbtn_Aci.Checked)
            {
                veriAciTablosunaYansit(veritabanıYolu);
            }
        }

        private void comboboxTiklanabilirlik()
        {
            if (!rbtn_Basinc.Checked && !rbtn_Aci.Checked)
            {
                cbox_analizTabloUcus.Enabled = false; 
            }
            else
            {
                cbox_analizTabloUcus.Enabled = true;
            }
        }

        public void baslangicCalistir()
        {
            tbl_Aci.ReadOnly = true;
            tbl_Basinc.ReadOnly = true;
            tbl_Basinc.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            tbl_Aci.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            
            databaseComboboxDoldur();
            comboboxTiklanabilirlik();

        }

        private void btn_verileriExcelAktar_Click(object sender, EventArgs e)
        {
            string selectedDatabase = cbox_analizTabloUcus.SelectedItem as string;

            if (string.IsNullOrEmpty(selectedDatabase))
            {
                MessageBox.Show("Lütfen bir veritabanı seçin.");
                return;
            }

            string databasePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "UcusVeri",selectedDatabase);
            
            string selectQuery = "";

            if (rbtn_Basinc.Checked)
            {
                selectQuery = "SELECT basinc, CASE WHEN durum = 1 THEN 'Tetiklendi' ELSE 'Tetiklenmedi' END AS durum, zaman FROM [Basinc Tablosu]";
            }
            else if (rbtn_Aci.Checked)
            {
                selectQuery = "SELECT X, Y, Z, CASE WHEN durum = 1 THEN 'Tetiklendi' ELSE 'Tetiklenmedi' END AS durum, zaman FROM [Aci Tablosu]";
            }
            else
            {
                MessageBox.Show("Geçersiz tablo seçimi.");
                return;
            }

            try
            {
                string connectionString = $"Data Source={databasePath};Version=3;";

                using (SaveFileDialog sfd = new SaveFileDialog() { Filter = "Excel Workbook|*.xlsx", ValidateNames = true })
                {
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                        {
                            connection.Open();
                            SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(selectQuery, connection);
                            System.Data.DataTable dataTable = new System.Data.DataTable();
                            dataAdapter.Fill(dataTable);
                            connection.Close();

                            
                            using (XLWorkbook wb = new XLWorkbook())
                            {
                                wb.Worksheets.Add(dataTable, "Veri");

                                wb.SaveAs(sfd.FileName);
                            }

                            MessageBox.Show("Veriler Excel'e aktarıldı.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Excel'e aktarma hatası: " + ex.Message);
            }
        }

        private void btn_Aci_CheckedChanged(object sender, EventArgs e)
        {
            tbl_Aci.Visible = true;
            tbl_Basinc.Visible = false;
           
        }

        private void btn_Basinc_CheckedChanged(object sender, EventArgs e)
        {
            tbl_Aci.Visible = false;
            tbl_Basinc.Visible = true;

        }

        private void rbtn_Basinc_CheckedChanged(object sender, EventArgs e)
        {
            tbl_Aci.Visible = false;
            tbl_Basinc.Visible = true;
            comboboxGuncelle();
            tbl_Aci.DataSource = null;
            comboboxTiklanabilirlik();
        }

        private void rbtn_Aci_CheckedChanged(object sender, EventArgs e)
        {
            tbl_Aci.Visible = true;
            tbl_Basinc.Visible = false;
            comboboxGuncelle();
            tbl_Basinc.DataSource = null;
            comboboxTiklanabilirlik();
            
        }

        private void cbox_analizTabloUcus_SelectedIndexChanged(object sender, EventArgs e)
        {
            secilenDatabaseYansit();
        }

    }

}
