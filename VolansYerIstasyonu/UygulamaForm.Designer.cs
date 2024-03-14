namespace VolansYerIstasyonu
{
    partial class UygulamaForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnl_AnaSayfa = new System.Windows.Forms.Panel();
            this.btn_yerIst = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.btn_PrtAyar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // pnl_AnaSayfa
            // 
            this.pnl_AnaSayfa.Location = new System.Drawing.Point(128, 1);
            this.pnl_AnaSayfa.Name = "pnl_AnaSayfa";
            this.pnl_AnaSayfa.Size = new System.Drawing.Size(986, 609);
            this.pnl_AnaSayfa.TabIndex = 0;
            // 
            // btn_yerIst
            // 
            this.btn_yerIst.Location = new System.Drawing.Point(12, 55);
            this.btn_yerIst.Name = "btn_yerIst";
            this.btn_yerIst.Size = new System.Drawing.Size(90, 60);
            this.btn_yerIst.TabIndex = 1;
            this.btn_yerIst.Text = "Yer İstasyonu";
            this.btn_yerIst.UseVisualStyleBackColor = true;
            this.btn_yerIst.Click += new System.EventHandler(this.btn_yerIst_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(12, 187);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(90, 60);
            this.button2.TabIndex = 1;
            this.button2.Text = "Veri Grafikleri";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(12, 253);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(90, 60);
            this.button3.TabIndex = 1;
            this.button3.Text = "Analiz (Grafik)";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(12, 319);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(90, 60);
            this.button4.TabIndex = 1;
            this.button4.Text = "Analiz (Tablo)";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // btn_PrtAyar
            // 
            this.btn_PrtAyar.Location = new System.Drawing.Point(12, 121);
            this.btn_PrtAyar.Name = "btn_PrtAyar";
            this.btn_PrtAyar.Size = new System.Drawing.Size(90, 60);
            this.btn_PrtAyar.TabIndex = 1;
            this.btn_PrtAyar.Text = "Port Ayarları";
            this.btn_PrtAyar.UseVisualStyleBackColor = true;
            this.btn_PrtAyar.Click += new System.EventHandler(this.btn_PrtAyar_Click);
            // 
            // UygulamaForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1116, 611);
            this.Controls.Add(this.btn_PrtAyar);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btn_yerIst);
            this.Controls.Add(this.pnl_AnaSayfa);
            this.Name = "UygulamaForm";
            this.Text = "Volans Roket Takımı Yer İstasyonu";
            this.Load += new System.EventHandler(this.UygulamaForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnl_AnaSayfa;
        private System.Windows.Forms.Button btn_yerIst;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button btn_PrtAyar;
    }
}