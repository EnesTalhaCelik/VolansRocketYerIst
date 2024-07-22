namespace VolansYerIstasyonu.UserControls
{
    partial class uc_VeriTablo
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.rbtn_yedekAviyonik = new System.Windows.Forms.RadioButton();
            this.rbtn_gorevYuku = new System.Windows.Forms.RadioButton();
            this.tbl_anaAviyonik = new System.Windows.Forms.DataGridView();
            this.tbl_yedekAviyonik = new System.Windows.Forms.DataGridView();
            this.cbox_veriTabloUcus = new System.Windows.Forms.ComboBox();
            this.btn_anaAvDbOlustur = new System.Windows.Forms.Button();
            this.btn_yedekAvDbOlustur = new System.Windows.Forms.Button();
            this.btn_gorevYukuDbOlustur = new System.Windows.Forms.Button();
            this.btn_verileriExceleAktar = new System.Windows.Forms.Button();
            this.tbl_gorevYuku = new System.Windows.Forms.DataGridView();
            this.rbtn_anaAviyonik = new System.Windows.Forms.RadioButton();
            this.label_veritabaniSec = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.tbl_anaAviyonik)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbl_yedekAviyonik)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbl_gorevYuku)).BeginInit();
            this.SuspendLayout();
            // 
            // rbtn_yedekAviyonik
            // 
            this.rbtn_yedekAviyonik.AutoSize = true;
            this.rbtn_yedekAviyonik.Location = new System.Drawing.Point(1041, 116);
            this.rbtn_yedekAviyonik.Name = "rbtn_yedekAviyonik";
            this.rbtn_yedekAviyonik.Size = new System.Drawing.Size(122, 20);
            this.rbtn_yedekAviyonik.TabIndex = 2;
            this.rbtn_yedekAviyonik.Text = "Yedek Aviyonik";
            this.rbtn_yedekAviyonik.UseVisualStyleBackColor = true;
            this.rbtn_yedekAviyonik.CheckedChanged += new System.EventHandler(this.rbtn_yedekAviyonik_CheckedChanged);
            // 
            // rbtn_gorevYuku
            // 
            this.rbtn_gorevYuku.AutoSize = true;
            this.rbtn_gorevYuku.Location = new System.Drawing.Point(1041, 142);
            this.rbtn_gorevYuku.Name = "rbtn_gorevYuku";
            this.rbtn_gorevYuku.Size = new System.Drawing.Size(98, 20);
            this.rbtn_gorevYuku.TabIndex = 3;
            this.rbtn_gorevYuku.Text = "Görev Yükü";
            this.rbtn_gorevYuku.UseVisualStyleBackColor = true;
            this.rbtn_gorevYuku.CheckedChanged += new System.EventHandler(this.rbtn_gorevYuku_CheckedChanged);
            // 
            // tbl_anaAviyonik
            // 
            this.tbl_anaAviyonik.AllowUserToAddRows = false;
            this.tbl_anaAviyonik.AllowUserToDeleteRows = false;
            this.tbl_anaAviyonik.AllowUserToResizeColumns = false;
            this.tbl_anaAviyonik.AllowUserToResizeRows = false;
            this.tbl_anaAviyonik.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tbl_anaAviyonik.Location = new System.Drawing.Point(67, 46);
            this.tbl_anaAviyonik.Name = "tbl_anaAviyonik";
            this.tbl_anaAviyonik.ReadOnly = true;
            this.tbl_anaAviyonik.RowHeadersVisible = false;
            this.tbl_anaAviyonik.RowHeadersWidth = 51;
            this.tbl_anaAviyonik.RowTemplate.Height = 24;
            this.tbl_anaAviyonik.Size = new System.Drawing.Size(925, 651);
            this.tbl_anaAviyonik.TabIndex = 4;
            this.tbl_anaAviyonik.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.tbl_anaAviyonik_CellContentClick);
            // 
            // tbl_yedekAviyonik
            // 
            this.tbl_yedekAviyonik.AllowUserToAddRows = false;
            this.tbl_yedekAviyonik.AllowUserToDeleteRows = false;
            this.tbl_yedekAviyonik.AllowUserToResizeColumns = false;
            this.tbl_yedekAviyonik.AllowUserToResizeRows = false;
            this.tbl_yedekAviyonik.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tbl_yedekAviyonik.Location = new System.Drawing.Point(67, 46);
            this.tbl_yedekAviyonik.Name = "tbl_yedekAviyonik";
            this.tbl_yedekAviyonik.ReadOnly = true;
            this.tbl_yedekAviyonik.RowHeadersVisible = false;
            this.tbl_yedekAviyonik.RowHeadersWidth = 51;
            this.tbl_yedekAviyonik.RowTemplate.Height = 24;
            this.tbl_yedekAviyonik.Size = new System.Drawing.Size(925, 651);
            this.tbl_yedekAviyonik.TabIndex = 5;
            this.tbl_yedekAviyonik.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.tbl_yedekAviyonik_CellContentClick);
            // 
            // cbox_veriTabloUcus
            // 
            this.cbox_veriTabloUcus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbox_veriTabloUcus.Location = new System.Drawing.Point(1041, 242);
            this.cbox_veriTabloUcus.Name = "cbox_veriTabloUcus";
            this.cbox_veriTabloUcus.Size = new System.Drawing.Size(234, 24);
            this.cbox_veriTabloUcus.TabIndex = 6;
            this.cbox_veriTabloUcus.SelectedIndexChanged += new System.EventHandler(this.cbox_veriTabloUcus_SelectedIndexChanged);
            // 
            // btn_anaAvDbOlustur
            // 
            this.btn_anaAvDbOlustur.Location = new System.Drawing.Point(1041, 333);
            this.btn_anaAvDbOlustur.Name = "btn_anaAvDbOlustur";
            this.btn_anaAvDbOlustur.Size = new System.Drawing.Size(234, 23);
            this.btn_anaAvDbOlustur.TabIndex = 7;
            this.btn_anaAvDbOlustur.Text = "Ana Aviyonik Database Oluştur";
            this.btn_anaAvDbOlustur.UseVisualStyleBackColor = true;
            this.btn_anaAvDbOlustur.Click += new System.EventHandler(this.btn_anaAvDbOlustur_Click);
            // 
            // btn_yedekAvDbOlustur
            // 
            this.btn_yedekAvDbOlustur.Location = new System.Drawing.Point(1041, 362);
            this.btn_yedekAvDbOlustur.Name = "btn_yedekAvDbOlustur";
            this.btn_yedekAvDbOlustur.Size = new System.Drawing.Size(234, 23);
            this.btn_yedekAvDbOlustur.TabIndex = 8;
            this.btn_yedekAvDbOlustur.Text = "Yedek Aviyonik Database Oluştur";
            this.btn_yedekAvDbOlustur.UseVisualStyleBackColor = true;
            this.btn_yedekAvDbOlustur.Click += new System.EventHandler(this.btn_yedekAvDbOlustur_Click);
            // 
            // btn_gorevYukuDbOlustur
            // 
            this.btn_gorevYukuDbOlustur.Location = new System.Drawing.Point(1041, 391);
            this.btn_gorevYukuDbOlustur.Name = "btn_gorevYukuDbOlustur";
            this.btn_gorevYukuDbOlustur.Size = new System.Drawing.Size(234, 23);
            this.btn_gorevYukuDbOlustur.TabIndex = 9;
            this.btn_gorevYukuDbOlustur.Text = "Görev Yükü Database Oluştur";
            this.btn_gorevYukuDbOlustur.UseVisualStyleBackColor = true;
            this.btn_gorevYukuDbOlustur.Click += new System.EventHandler(this.btn_gorevYukuDbOlustur_Click);
            // 
            // btn_verileriExceleAktar
            // 
            this.btn_verileriExceleAktar.Location = new System.Drawing.Point(1041, 420);
            this.btn_verileriExceleAktar.Name = "btn_verileriExceleAktar";
            this.btn_verileriExceleAktar.Size = new System.Drawing.Size(234, 23);
            this.btn_verileriExceleAktar.TabIndex = 10;
            this.btn_verileriExceleAktar.Text = "Excel Olarak Kaydet";
            this.btn_verileriExceleAktar.UseVisualStyleBackColor = true;
            this.btn_verileriExceleAktar.Click += new System.EventHandler(this.btn_verileriExceleAktar_Click);
            // 
            // tbl_gorevYuku
            // 
            this.tbl_gorevYuku.AllowUserToAddRows = false;
            this.tbl_gorevYuku.AllowUserToDeleteRows = false;
            this.tbl_gorevYuku.AllowUserToResizeColumns = false;
            this.tbl_gorevYuku.AllowUserToResizeRows = false;
            this.tbl_gorevYuku.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tbl_gorevYuku.Location = new System.Drawing.Point(67, 46);
            this.tbl_gorevYuku.Name = "tbl_gorevYuku";
            this.tbl_gorevYuku.ReadOnly = true;
            this.tbl_gorevYuku.RowHeadersVisible = false;
            this.tbl_gorevYuku.RowHeadersWidth = 51;
            this.tbl_gorevYuku.RowTemplate.Height = 24;
            this.tbl_gorevYuku.Size = new System.Drawing.Size(925, 651);
            this.tbl_gorevYuku.TabIndex = 11;
            this.tbl_gorevYuku.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.tbl_gorevYuku_CellContentClick);
            // 
            // rbtn_anaAviyonik
            // 
            this.rbtn_anaAviyonik.AutoSize = true;
            this.rbtn_anaAviyonik.Location = new System.Drawing.Point(1041, 90);
            this.rbtn_anaAviyonik.Name = "rbtn_anaAviyonik";
            this.rbtn_anaAviyonik.Size = new System.Drawing.Size(106, 20);
            this.rbtn_anaAviyonik.TabIndex = 12;
            this.rbtn_anaAviyonik.Text = "Ana Aviyonik";
            this.rbtn_anaAviyonik.UseVisualStyleBackColor = true;
            this.rbtn_anaAviyonik.CheckedChanged += new System.EventHandler(this.rbtn_anaAviyonik_CheckedChanged);
            // 
            // label_veritabaniSec
            // 
            this.label_veritabaniSec.AutoSize = true;
            this.label_veritabaniSec.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_veritabaniSec.Location = new System.Drawing.Point(1100, 223);
            this.label_veritabaniSec.Name = "label_veritabaniSec";
            this.label_veritabaniSec.Size = new System.Drawing.Size(114, 16);
            this.label_veritabaniSec.TabIndex = 13;
            this.label_veritabaniSec.Text = "Veritabanı Seçiniz";
            this.label_veritabaniSec.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // uc_VeriTablo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label_veritabaniSec);
            this.Controls.Add(this.rbtn_anaAviyonik);
            this.Controls.Add(this.tbl_gorevYuku);
            this.Controls.Add(this.btn_verileriExceleAktar);
            this.Controls.Add(this.btn_gorevYukuDbOlustur);
            this.Controls.Add(this.btn_yedekAvDbOlustur);
            this.Controls.Add(this.btn_anaAvDbOlustur);
            this.Controls.Add(this.cbox_veriTabloUcus);
            this.Controls.Add(this.tbl_yedekAviyonik);
            this.Controls.Add(this.tbl_anaAviyonik);
            this.Controls.Add(this.rbtn_gorevYuku);
            this.Controls.Add(this.rbtn_yedekAviyonik);
            this.Name = "uc_VeriTablo";
            this.Size = new System.Drawing.Size(1315, 750);
            this.Load += new System.EventHandler(this.uc_VeriTablo_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tbl_anaAviyonik)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbl_yedekAviyonik)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbl_gorevYuku)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.RadioButton rbtn_yedekAviyonik;
        private System.Windows.Forms.RadioButton rbtn_gorevYuku;
        private System.Windows.Forms.DataGridView tbl_anaAviyonik;
        private System.Windows.Forms.DataGridView tbl_yedekAviyonik;
        private System.Windows.Forms.ComboBox cbox_veriTabloUcus;
        private System.Windows.Forms.Button btn_anaAvDbOlustur;
        private System.Windows.Forms.Button btn_yedekAvDbOlustur;
        private System.Windows.Forms.Button btn_gorevYukuDbOlustur;
        private System.Windows.Forms.Button btn_verileriExceleAktar;
        private System.Windows.Forms.DataGridView tbl_gorevYuku;
        private System.Windows.Forms.RadioButton rbtn_anaAviyonik;
        private System.Windows.Forms.Label label_veritabaniSec;
    }
}
