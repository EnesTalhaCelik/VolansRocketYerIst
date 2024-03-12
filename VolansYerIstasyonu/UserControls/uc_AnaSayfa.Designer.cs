namespace VolansYerIstasyonu.UserControls
{
    partial class uc_AnaSayfa
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
            this.anasayfaHarita = new GMap.NET.WindowsForms.GMapControl();
            this.gboxVeriler = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.gboxVeriler.SuspendLayout();
            this.SuspendLayout();
            // 
            // anasayfaHarita
            // 
            this.anasayfaHarita.Bearing = 0F;
            this.anasayfaHarita.CanDragMap = true;
            this.anasayfaHarita.EmptyTileColor = System.Drawing.Color.Navy;
            this.anasayfaHarita.GrayScaleMode = false;
            this.anasayfaHarita.HelperLineOption = GMap.NET.WindowsForms.HelperLineOptions.DontShow;
            this.anasayfaHarita.LevelsKeepInMemory = 5;
            this.anasayfaHarita.Location = new System.Drawing.Point(330, 0);
            this.anasayfaHarita.MarkersEnabled = true;
            this.anasayfaHarita.MaxZoom = 2;
            this.anasayfaHarita.MinZoom = 2;
            this.anasayfaHarita.MouseWheelZoomEnabled = true;
            this.anasayfaHarita.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            this.anasayfaHarita.Name = "anasayfaHarita";
            this.anasayfaHarita.NegativeMode = false;
            this.anasayfaHarita.PolygonsEnabled = true;
            this.anasayfaHarita.RetryLoadTile = 0;
            this.anasayfaHarita.RoutesEnabled = true;
            this.anasayfaHarita.ScaleMode = GMap.NET.WindowsForms.ScaleModes.Integer;
            this.anasayfaHarita.SelectedAreaFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(65)))), ((int)(((byte)(105)))), ((int)(((byte)(225)))));
            this.anasayfaHarita.ShowTileGridLines = false;
            this.anasayfaHarita.Size = new System.Drawing.Size(656, 560);
            this.anasayfaHarita.TabIndex = 0;
            this.anasayfaHarita.Zoom = 0D;
            // 
            // gboxVeriler
            // 
            this.gboxVeriler.Controls.Add(this.textBox1);
            this.gboxVeriler.Controls.Add(this.label1);
            this.gboxVeriler.Location = new System.Drawing.Point(4, 4);
            this.gboxVeriler.Name = "gboxVeriler";
            this.gboxVeriler.Size = new System.Drawing.Size(320, 556);
            this.gboxVeriler.TabIndex = 1;
            this.gboxVeriler.TabStop = false;
            this.gboxVeriler.Text = "Veriler";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Takım ID";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(73, 17);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(58, 20);
            this.textBox1.TabIndex = 1;
            // 
            // uc_AnaSayfa
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gboxVeriler);
            this.Controls.Add(this.anasayfaHarita);
            this.Name = "uc_AnaSayfa";
            this.Size = new System.Drawing.Size(986, 609);
            this.gboxVeriler.ResumeLayout(false);
            this.gboxVeriler.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private GMap.NET.WindowsForms.GMapControl anasayfaHarita;
        private System.Windows.Forms.GroupBox gboxVeriler;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
    }
}
