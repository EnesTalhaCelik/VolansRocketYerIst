using System;
using System.Windows.Forms;

namespace VolansYerIstasyonu.UserControls
{
    partial class uc_AnalizTablo
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
            this.components = new System.ComponentModel.Container();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tbl_Basinc = new System.Windows.Forms.DataGridView();
            this.tbl_Aci = new System.Windows.Forms.DataGridView();
            this.rbtn_Basinc = new System.Windows.Forms.RadioButton();
            this.rbtn_Aci = new System.Windows.Forms.RadioButton();
            this.cbox_analizTabloUcus = new System.Windows.Forms.ComboBox();
            this.btn_verileriExcelAktar = new System.Windows.Forms.Button();
            this.btn_basincDbOlustur = new System.Windows.Forms.Button();
            this.btn_aciDbOlustur = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.tbl_Basinc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbl_Aci)).BeginInit();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // tbl_Basinc
            // 
            this.tbl_Basinc.AllowUserToAddRows = false;
            this.tbl_Basinc.AllowUserToDeleteRows = false;
            this.tbl_Basinc.AllowUserToResizeColumns = false;
            this.tbl_Basinc.AllowUserToResizeRows = false;
            this.tbl_Basinc.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tbl_Basinc.Location = new System.Drawing.Point(57, 40);
            this.tbl_Basinc.Margin = new System.Windows.Forms.Padding(2);
            this.tbl_Basinc.Name = "tbl_Basinc";
            this.tbl_Basinc.RowHeadersVisible = false;
            this.tbl_Basinc.RowHeadersWidth = 51;
            this.tbl_Basinc.RowTemplate.Height = 24;
            this.tbl_Basinc.Size = new System.Drawing.Size(694, 529);
            this.tbl_Basinc.TabIndex = 3;
            // 
            // tbl_Aci
            // 
            this.tbl_Aci.AllowUserToAddRows = false;
            this.tbl_Aci.AllowUserToDeleteRows = false;
            this.tbl_Aci.AllowUserToResizeColumns = false;
            this.tbl_Aci.AllowUserToResizeRows = false;
            this.tbl_Aci.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tbl_Aci.Location = new System.Drawing.Point(57, 40);
            this.tbl_Aci.Margin = new System.Windows.Forms.Padding(2);
            this.tbl_Aci.Name = "tbl_Aci";
            this.tbl_Aci.RowHeadersVisible = false;
            this.tbl_Aci.RowHeadersWidth = 51;
            this.tbl_Aci.RowTemplate.Height = 24;
            this.tbl_Aci.Size = new System.Drawing.Size(694, 529);
            this.tbl_Aci.TabIndex = 6;
            this.tbl_Aci.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.tbl_Aci_CellContentClick);
            // 
            // rbtn_Basinc
            // 
            this.rbtn_Basinc.AutoSize = true;
            this.rbtn_Basinc.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbtn_Basinc.Location = new System.Drawing.Point(817, 50);
            this.rbtn_Basinc.Margin = new System.Windows.Forms.Padding(2);
            this.rbtn_Basinc.Name = "rbtn_Basinc";
            this.rbtn_Basinc.Size = new System.Drawing.Size(98, 17);
            this.rbtn_Basinc.TabIndex = 7;
            this.rbtn_Basinc.TabStop = true;
            this.rbtn_Basinc.Text = "Basınç Tablosu";
            this.rbtn_Basinc.UseVisualStyleBackColor = true;
            this.rbtn_Basinc.CheckedChanged += new System.EventHandler(this.rbtn_Basinc_CheckedChanged);
            // 
            // rbtn_Aci
            // 
            this.rbtn_Aci.AutoSize = true;
            this.rbtn_Aci.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbtn_Aci.Location = new System.Drawing.Point(817, 72);
            this.rbtn_Aci.Margin = new System.Windows.Forms.Padding(2);
            this.rbtn_Aci.Name = "rbtn_Aci";
            this.rbtn_Aci.Size = new System.Drawing.Size(81, 17);
            this.rbtn_Aci.TabIndex = 8;
            this.rbtn_Aci.TabStop = true;
            this.rbtn_Aci.Text = "Açı Tablosu";
            this.rbtn_Aci.UseVisualStyleBackColor = true;
            this.rbtn_Aci.CheckedChanged += new System.EventHandler(this.rbtn_Aci_CheckedChanged);
            // 
            // cbox_analizTabloUcus
            // 
            this.cbox_analizTabloUcus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbox_analizTabloUcus.Location = new System.Drawing.Point(800, 110);
            this.cbox_analizTabloUcus.Margin = new System.Windows.Forms.Padding(2);
            this.cbox_analizTabloUcus.Name = "cbox_analizTabloUcus";
            this.cbox_analizTabloUcus.Size = new System.Drawing.Size(146, 23);
            this.cbox_analizTabloUcus.TabIndex = 11;
            this.cbox_analizTabloUcus.Text = "    Veritabanı Seçiniz";
            this.cbox_analizTabloUcus.SelectedIndexChanged += new System.EventHandler(this.cbox_analizTabloUcus_SelectedIndexChanged);
            // 
            // btn_verileriExcelAktar
            // 
            this.btn_verileriExcelAktar.Location = new System.Drawing.Point(800, 152);
            this.btn_verileriExcelAktar.Margin = new System.Windows.Forms.Padding(2);
            this.btn_verileriExcelAktar.Name = "btn_verileriExcelAktar";
            this.btn_verileriExcelAktar.Size = new System.Drawing.Size(145, 23);
            this.btn_verileriExcelAktar.TabIndex = 12;
            this.btn_verileriExcelAktar.Text = "Excel olarak kaydet";
            this.btn_verileriExcelAktar.UseVisualStyleBackColor = true;
            this.btn_verileriExcelAktar.Click += new System.EventHandler(this.btn_verileriExcelAktar_Click);
            // 
            // btn_basincDbOlustur
            // 
            this.btn_basincDbOlustur.Location = new System.Drawing.Point(801, 194);
            this.btn_basincDbOlustur.Margin = new System.Windows.Forms.Padding(2);
            this.btn_basincDbOlustur.Name = "btn_basincDbOlustur";
            this.btn_basincDbOlustur.Size = new System.Drawing.Size(145, 23);
            this.btn_basincDbOlustur.TabIndex = 12;
            this.btn_basincDbOlustur.Text = "Basınç Database Oluştur";
            this.btn_basincDbOlustur.UseVisualStyleBackColor = true;
            this.btn_basincDbOlustur.Click += new System.EventHandler(this.btn_basincDbOlustur_Click);
            // 
            // btn_aciDbOlustur
            // 
            this.btn_aciDbOlustur.Location = new System.Drawing.Point(801, 221);
            this.btn_aciDbOlustur.Margin = new System.Windows.Forms.Padding(2);
            this.btn_aciDbOlustur.Name = "btn_aciDbOlustur";
            this.btn_aciDbOlustur.Size = new System.Drawing.Size(145, 23);
            this.btn_aciDbOlustur.TabIndex = 12;
            this.btn_aciDbOlustur.Text = "Açı Database Oluştur";
            this.btn_aciDbOlustur.UseVisualStyleBackColor = true;
            this.btn_aciDbOlustur.Click += new System.EventHandler(this.btn_aciDbOlustur_Click);
            // 
            // uc_AnalizTablo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btn_aciDbOlustur);
            this.Controls.Add(this.btn_basincDbOlustur);
            this.Controls.Add(this.btn_verileriExcelAktar);
            this.Controls.Add(this.cbox_analizTabloUcus);
            this.Controls.Add(this.rbtn_Aci);
            this.Controls.Add(this.rbtn_Basinc);
            this.Controls.Add(this.tbl_Aci);
            this.Controls.Add(this.tbl_Basinc);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "uc_AnalizTablo";
            this.Size = new System.Drawing.Size(986, 609);
            ((System.ComponentModel.ISupportInitialize)(this.tbl_Basinc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbl_Aci)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

       

        #endregion
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.DataGridView tbl_Basinc;
        private System.Windows.Forms.DataGridView tbl_Aci;
        private System.Windows.Forms.RadioButton rbtn_Basinc;
        private System.Windows.Forms.RadioButton rbtn_Aci;
        private ComboBox cbox_analizTabloUcus;
        private Button btn_verileriExcelAktar;
        private Button btn_basincDbOlustur;
        private Button btn_aciDbOlustur;
    }
}
