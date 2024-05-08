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
            this.btn_AnalizTablo = new System.Windows.Forms.Button();
            this.btn_PrtAyar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // pnl_AnaSayfa
            // 
            this.pnl_AnaSayfa.Location = new System.Drawing.Point(171, 1);
            this.pnl_AnaSayfa.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnl_AnaSayfa.Name = "pnl_AnaSayfa";
            this.pnl_AnaSayfa.Size = new System.Drawing.Size(1315, 750);
            this.pnl_AnaSayfa.TabIndex = 0;
            // 
            // btn_yerIst
            // 
            this.btn_yerIst.Location = new System.Drawing.Point(16, 66);
            this.btn_yerIst.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btn_yerIst.Name = "btn_yerIst";
            this.btn_yerIst.Size = new System.Drawing.Size(120, 74);
            this.btn_yerIst.TabIndex = 1;
            this.btn_yerIst.Text = "Yer İstasyonu";
            this.btn_yerIst.UseVisualStyleBackColor = true;
            this.btn_yerIst.Click += new System.EventHandler(this.btn_yerIst_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(16, 229);
            this.button2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(120, 74);
            this.button2.TabIndex = 1;
            this.button2.Text = "Veri Grafikleri";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(16, 310);
            this.button3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(120, 74);
            this.button3.TabIndex = 1;
            this.button3.Text = "Analiz (Grafik)";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // btn_AnalizTablo
            // 
            this.btn_AnalizTablo.Location = new System.Drawing.Point(16, 390);
            this.btn_AnalizTablo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btn_AnalizTablo.Name = "btn_AnalizTablo";
            this.btn_AnalizTablo.Size = new System.Drawing.Size(120, 74);
            this.btn_AnalizTablo.TabIndex = 1;
            this.btn_AnalizTablo.Text = "Analiz (Tablo)";
            this.btn_AnalizTablo.UseVisualStyleBackColor = true;
            this.btn_AnalizTablo.Click += new System.EventHandler(this.btn_AnalizTablo_Click);
            // 
            // btn_PrtAyar
            // 
            this.btn_PrtAyar.Location = new System.Drawing.Point(16, 148);
            this.btn_PrtAyar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btn_PrtAyar.Name = "btn_PrtAyar";
            this.btn_PrtAyar.Size = new System.Drawing.Size(120, 74);
            this.btn_PrtAyar.TabIndex = 1;
            this.btn_PrtAyar.Text = "Port Ayarları";
            this.btn_PrtAyar.UseVisualStyleBackColor = true;
            this.btn_PrtAyar.Click += new System.EventHandler(this.btn_PrtAyar_Click);
            // 
            // UygulamaForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1488, 752);
            this.Controls.Add(this.btn_PrtAyar);
            this.Controls.Add(this.btn_AnalizTablo);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btn_yerIst);
            this.Controls.Add(this.pnl_AnaSayfa);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
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
        private System.Windows.Forms.Button btn_AnalizTablo;
        private System.Windows.Forms.Button btn_PrtAyar;
    }
}