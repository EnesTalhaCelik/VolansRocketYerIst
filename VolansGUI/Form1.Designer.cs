using System.Windows.Forms;
using MaterialSkin;
using MaterialSkin.Controls;
using GMap.NET.WindowsForms;
using OxyPlot.WindowsForms;

namespace VolansGUI
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        // ---- Tabs ----
        private MaterialTabSelector tabSelector;
        private MaterialTabControl tabControl;
        private TabPage tabYerIstasyonu;
        private TabPage tabPortAyarlari;
        private TabPage tabVeriGrafikleri;
        private TabPage tabAnaliz;

        // ---- Tab 1: Yer İstasyonu ----
        private GMapControl mapControl;
        private Panel pnlDashboard;
        private MaterialLabel lblTakimIdDeger, lblPaketSayaciDeger, lblDurumDeger, lblBasincIrtifaDeger,
            lblGpsIrtifaDeger, lblRoketEnlemDeger, lblRoketBoylamDeger, lblGorevEnlemDeger, lblGorevBoylamDeger,
            lblSicaklikDeger, lblNemDeger, lblBasincDeger, lblJiroskopXDeger, lblJiroskopYDeger, lblJiroskopZDeger,
            lblAciDeger, lblHizDeger, lblHaritaEnlemDeger, lblHaritaBoylamDeger, lblGecenSureDeger,
            lblBasincAyrilma, lblAciAyrilma;
        private MaterialButton btnSimBaslat, btnSimSifirla, btnKonumaGit;
        private MaterialTextBox txtKonumEnlem, txtKonumBoylam;

        // ---- Tab 2: Port Ayarları ----
        private MaterialComboBox cmbLoraPort, cmbLoraBaud, cmbLoraDataBit, cmbLoraStopBit, cmbLoraParity;
        private MaterialButton btnPortYenile, btnLoraBaglan, btnLoraBaglantiKes, btnPingRoket, btnPingGorevYuku,
            btnPingPong, btnParamCek, btnTestLed, btnFunyePatlat, btnMesajGonder;
        private MaterialTextBox txtLoraMesaj;
        private TextBox txtLog;

        // ---- Tab 3: Veri Grafikleri ----
        private PlotView plotIrtifa;
        private PlotView plotAci;

        // ---- Tab 4: Analiz ----
        private MaterialRadioButton rbAnaAviyonik, rbYedekAviyonik, rbGorevYuku, rbTestler;
        private ComboBox cmbVeritabani;
        private MaterialButton btnAnaAvDbOlustur, btnYedekAvDbOlustur, btnGorevDbOlustur, btnExcelKaydet;
        private DataGridView gridVeri;

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            this.tabSelector = new MaterialTabSelector();
            this.tabControl = new MaterialTabControl();
            this.tabYerIstasyonu = new TabPage();
            this.tabPortAyarlari = new TabPage();
            this.tabVeriGrafikleri = new TabPage();
            this.tabAnaliz = new TabPage();
            this.mapControl = new GMapControl();
            this.pnlDashboard = new Panel();
            this.plotIrtifa = new PlotView();
            this.plotAci = new PlotView();
            this.gridVeri = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.gridVeri)).BeginInit();

            this.SuspendLayout();

            // ================= FORM =================
            this.AutoScaleMode = AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1600, 900);
            this.Padding = new Padding(3, 64, 3, 3);
            this.Text = "          VOLANS ROKET TAKIMI — YER İSTASYONU";
            this.Name = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);

            // ================= TABS =================
            this.tabControl.Dock = DockStyle.Fill;
            this.tabControl.Multiline = true;
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Controls.Add(this.tabYerIstasyonu);
            this.tabControl.Controls.Add(this.tabPortAyarlari);
            this.tabControl.Controls.Add(this.tabVeriGrafikleri);
            this.tabControl.Controls.Add(this.tabAnaliz);

            this.tabSelector.BaseTabControl = this.tabControl;
            this.tabSelector.Dock = DockStyle.Top;
            this.tabSelector.Height = 48;

            this.tabYerIstasyonu.Text = "Yer İstasyonu";
            this.tabYerIstasyonu.BackColor = System.Drawing.Color.White;
            this.tabPortAyarlari.Text = "Port Ayarları";
            this.tabPortAyarlari.BackColor = System.Drawing.Color.White;
            this.tabVeriGrafikleri.Text = "Veri Grafikleri";
            this.tabVeriGrafikleri.BackColor = System.Drawing.Color.White;
            this.tabAnaliz.Text = "Analiz (Tablo)";
            this.tabAnaliz.BackColor = System.Drawing.Color.White;

            // ================= TAB 1: YER İSTASYONU =================
            this.mapControl.Dock = DockStyle.Left;
            this.mapControl.Width = 900;
            this.mapControl.Bearing = 0F;
            this.mapControl.CanDragMap = true;
            this.mapControl.MarkersEnabled = true;
            this.mapControl.MaxZoom = 20;
            this.mapControl.MinZoom = 1;
            this.mapControl.MouseWheelZoomEnabled = true;
            this.mapControl.Name = "mapControl";
            this.mapControl.PolygonsEnabled = true;
            this.mapControl.RoutesEnabled = true;
            this.mapControl.ShowCenter = false;
            this.mapControl.Zoom = 15D;
            this.mapControl.MouseUp += new MouseEventHandler(this.mapControl_MouseUp);

            this.pnlDashboard.Dock = DockStyle.Fill;
            this.pnlDashboard.AutoScroll = true;
            this.pnlDashboard.BackColor = System.Drawing.Color.White;
            this.pnlDashboard.Name = "pnlDashboard";

            BuildDashboard();
            this.tabYerIstasyonu.Controls.Add(this.pnlDashboard);
            this.tabYerIstasyonu.Controls.Add(this.mapControl);

            // ================= TAB 2: PORT AYARLARI =================
            BuildPortAyarlari();

            // ================= TAB 3: VERI GRAFIKLERI =================
            this.plotAci.Dock = DockStyle.Bottom;
            this.plotAci.Height = 400;
            this.plotAci.Name = "plotAci";
            this.plotAci.BackColor = System.Drawing.Color.White;
            this.plotIrtifa.Dock = DockStyle.Fill;
            this.plotIrtifa.Name = "plotIrtifa";
            this.plotIrtifa.BackColor = System.Drawing.Color.White;
            this.tabVeriGrafikleri.Controls.Add(this.plotIrtifa);
            this.tabVeriGrafikleri.Controls.Add(this.plotAci);

            // ================= TAB 4: ANALIZ =================
            BuildAnaliz();

            // ================= ADD TO FORM =================
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.tabSelector);
            this.tabSelector.BringToFront();

            ((System.ComponentModel.ISupportInitialize)(this.gridVeri)).EndInit();
            this.ResumeLayout(false);
        }

        // ---- layout helpers ----
        private static MaterialLabel Caption(string text, int x, int y, Control parent)
        {
            var lbl = new MaterialLabel
            {
                AutoSize = true,
                Location = new System.Drawing.Point(x, y),
                Text = text,
                FontType = MaterialSkinManager.fontType.Body1
            };
            parent.Controls.Add(lbl);
            return lbl;
        }

        private static MaterialLabel Value(int x, int y, Control parent)
        {
            var lbl = new MaterialLabel
            {
                AutoSize = true,
                Location = new System.Drawing.Point(x, y),
                Text = "0",
                FontType = MaterialSkinManager.fontType.Body2
            };
            parent.Controls.Add(lbl);
            return lbl;
        }

        private void BuildDashboard()
        {
            int cx = 15, vx = 240, y = 15, step = 28;

            Caption("Takım ID", cx, y, pnlDashboard); lblTakimIdDeger = Value(vx, y, pnlDashboard); y += step;
            Caption("Paket Sayacı", cx, y, pnlDashboard); lblPaketSayaciDeger = Value(vx, y, pnlDashboard); y += step;
            Caption("Durum", cx, y, pnlDashboard); lblDurumDeger = Value(vx, y, pnlDashboard); y += step;
            Caption("Roket İrtifa (Basınç)", cx, y, pnlDashboard); lblBasincIrtifaDeger = Value(vx, y, pnlDashboard); y += step;
            Caption("Roket İrtifa (GPS)", cx, y, pnlDashboard); lblGpsIrtifaDeger = Value(vx, y, pnlDashboard); y += step;
            Caption("Roket Enlem", cx, y, pnlDashboard); lblRoketEnlemDeger = Value(vx, y, pnlDashboard); y += step;
            Caption("Roket Boylam", cx, y, pnlDashboard); lblRoketBoylamDeger = Value(vx, y, pnlDashboard); y += step;
            Caption("G. Yükü Enlem", cx, y, pnlDashboard); lblGorevEnlemDeger = Value(vx, y, pnlDashboard); y += step;
            Caption("G. Yükü Boylam", cx, y, pnlDashboard); lblGorevBoylamDeger = Value(vx, y, pnlDashboard); y += step;
            Caption("Sıcaklık (°C)", cx, y, pnlDashboard); lblSicaklikDeger = Value(vx, y, pnlDashboard); y += step;
            Caption("Nem (%)", cx, y, pnlDashboard); lblNemDeger = Value(vx, y, pnlDashboard); y += step;
            Caption("Basınç (hPa, BME280)", cx, y, pnlDashboard); lblBasincDeger = Value(vx, y, pnlDashboard); y += step;
            Caption("Jiroskop X (°/s)", cx, y, pnlDashboard); lblJiroskopXDeger = Value(vx, y, pnlDashboard); y += step;
            Caption("Jiroskop Y (°/s)", cx, y, pnlDashboard); lblJiroskopYDeger = Value(vx, y, pnlDashboard); y += step;
            Caption("Jiroskop Z (°/s)", cx, y, pnlDashboard); lblJiroskopZDeger = Value(vx, y, pnlDashboard); y += step;
            Caption("Açı (°)", cx, y, pnlDashboard); lblAciDeger = Value(vx, y, pnlDashboard); y += step;
            Caption("Roket Hız", cx, y, pnlDashboard); lblHizDeger = Value(vx, y, pnlDashboard); y += step;
            Caption("Harita C. Enlem", cx, y, pnlDashboard); lblHaritaEnlemDeger = Value(vx, y, pnlDashboard); y += step;
            Caption("Harita C. Boylam", cx, y, pnlDashboard); lblHaritaBoylamDeger = Value(vx, y, pnlDashboard); y += step;
            Caption("Geçen Süre (sn)", cx, y, pnlDashboard); lblGecenSureDeger = Value(vx, y, pnlDashboard); y += step + 10;

            lblBasincAyrilma = Caption("Basınç İle Ayrılma Tetiklenmedi", cx, y, pnlDashboard); y += step;
            lblAciAyrilma = Caption("Açı İle Ayrılma Tetiklenmedi", cx, y, pnlDashboard); y += step + 14;

            btnSimBaslat = new MaterialButton { Text = "Simülasyonu Başlat", Location = new System.Drawing.Point(cx, y), AutoSize = false, Size = new System.Drawing.Size(210, 36) };
            btnSimBaslat.Click += new System.EventHandler(this.btnSimBaslat_Click);
            pnlDashboard.Controls.Add(btnSimBaslat);
            btnSimSifirla = new MaterialButton { Text = "Sıfırla", Location = new System.Drawing.Point(240, y), AutoSize = false, Size = new System.Drawing.Size(120, 36) };
            btnSimSifirla.Click += new System.EventHandler(this.btnSimSifirla_Click);
            pnlDashboard.Controls.Add(btnSimSifirla);
            y += 52;

            txtKonumEnlem = new MaterialTextBox { Hint = "Gidilecek Enlem", Location = new System.Drawing.Point(cx, y), Size = new System.Drawing.Size(160, 48) };
            pnlDashboard.Controls.Add(txtKonumEnlem);
            txtKonumBoylam = new MaterialTextBox { Hint = "Gidilecek Boylam", Location = new System.Drawing.Point(185, y), Size = new System.Drawing.Size(160, 48) };
            pnlDashboard.Controls.Add(txtKonumBoylam);
            btnKonumaGit = new MaterialButton { Text = "Konuma Git", Location = new System.Drawing.Point(360, y), AutoSize = false, Size = new System.Drawing.Size(130, 36) };
            btnKonumaGit.Click += new System.EventHandler(this.btnKonumaGit_Click);
            pnlDashboard.Controls.Add(btnKonumaGit);
        }

        private void BuildPortAyarlari()
        {
            var page = this.tabPortAyarlari;

            // --- LoRa Port Ayarları (sol) ---
            Caption("LoRa Port Ayarları", 20, 15, page);
            int lx = 20, cx2 = 200, y = 55, step = 60;

            Caption("Seri Port", lx, y + 12, page);
            cmbLoraPort = new MaterialComboBox { Location = new System.Drawing.Point(cx2, y), Size = new System.Drawing.Size(180, 48), Hint = "Port" };
            page.Controls.Add(cmbLoraPort);
            btnPortYenile = new MaterialButton { Text = "Yenile", Location = new System.Drawing.Point(390, y + 4), AutoSize = false, Size = new System.Drawing.Size(90, 36) };
            btnPortYenile.Click += new System.EventHandler(this.btnPortYenile_Click);
            page.Controls.Add(btnPortYenile);
            y += step;

            Caption("Baud Hızı", lx, y + 12, page);
            cmbLoraBaud = new MaterialComboBox { Location = new System.Drawing.Point(cx2, y), Size = new System.Drawing.Size(180, 48), Hint = "Baud" };
            page.Controls.Add(cmbLoraBaud); y += step;

            Caption("Data Bit", lx, y + 12, page);
            cmbLoraDataBit = new MaterialComboBox { Location = new System.Drawing.Point(cx2, y), Size = new System.Drawing.Size(180, 48), Hint = "Data Bit" };
            page.Controls.Add(cmbLoraDataBit); y += step;

            Caption("Stop Bit", lx, y + 12, page);
            cmbLoraStopBit = new MaterialComboBox { Location = new System.Drawing.Point(cx2, y), Size = new System.Drawing.Size(180, 48), Hint = "Stop Bit" };
            page.Controls.Add(cmbLoraStopBit); y += step;

            Caption("Parity", lx, y + 12, page);
            cmbLoraParity = new MaterialComboBox { Location = new System.Drawing.Point(cx2, y), Size = new System.Drawing.Size(180, 48), Hint = "Parity" };
            page.Controls.Add(cmbLoraParity); y += step;

            btnLoraBaglan = new MaterialButton { Text = "Bağlan", Location = new System.Drawing.Point(lx, y), AutoSize = false, Size = new System.Drawing.Size(170, 40) };
            btnLoraBaglan.Click += new System.EventHandler(this.btnLoraBaglan_Click);
            page.Controls.Add(btnLoraBaglan);
            btnLoraBaglantiKes = new MaterialButton { Text = "Bağlantıyı Kes", Location = new System.Drawing.Point(200, y), AutoSize = false, Size = new System.Drawing.Size(190, 40), Enabled = false };
            btnLoraBaglantiKes.Click += new System.EventHandler(this.btnLoraBaglantiKes_Click);
            page.Controls.Add(btnLoraBaglantiKes);

            // --- LoRa Komutları (orta) ---
            int gx = 560; y = 55;
            Caption("LoRa Komutları", gx, 15, page);
            btnPingRoket = CmdButton("Ping Roket", gx, ref y); btnPingRoket.Click += new System.EventHandler(this.btnPingRoket_Click);
            btnPingGorevYuku = CmdButton("Ping Görev Yükü", gx, ref y); btnPingGorevYuku.Click += new System.EventHandler(this.btnPingGorevYuku_Click);
            btnPingPong = CmdButton("Ping Pong Test", gx, ref y); btnPingPong.Click += new System.EventHandler(this.btnPingPong_Click);
            btnParamCek = CmdButton("Parametreleri Çek", gx, ref y); btnParamCek.Click += new System.EventHandler(this.btnParamCek_Click);
            btnTestLed = CmdButton("Test: Aviyonik LED Yak", gx, ref y); btnTestLed.Click += new System.EventHandler(this.btnTestLed_Click);
            btnFunyePatlat = CmdButton("Fünyeyi Patlat", gx, ref y); btnFunyePatlat.Click += new System.EventHandler(this.btnFunyePatlat_Click);

            txtLoraMesaj = new MaterialTextBox { Hint = "Gönderilecek mesaj", Location = new System.Drawing.Point(gx, y), Size = new System.Drawing.Size(230, 48) };
            page.Controls.Add(txtLoraMesaj); y += 56;
            btnMesajGonder = new MaterialButton { Text = "Mesaj Gönder", Location = new System.Drawing.Point(gx, y), AutoSize = false, Size = new System.Drawing.Size(230, 40), Enabled = false };
            btnMesajGonder.Click += new System.EventHandler(this.btnMesajGonder_Click);
            page.Controls.Add(btnMesajGonder);

            // --- Bağlantı kaydı (sağ) ---
            Caption("Bağlantı Kaydı", 900, 15, page);
            txtLog = new TextBox { Multiline = true, ScrollBars = ScrollBars.Vertical, ReadOnly = true, Location = new System.Drawing.Point(900, 55), Size = new System.Drawing.Size(650, 700) };
            page.Controls.Add(txtLog);
        }

        private MaterialButton CmdButton(string text, int x, ref int y)
        {
            var b = new MaterialButton { Text = text, Location = new System.Drawing.Point(x, y), AutoSize = false, Size = new System.Drawing.Size(230, 40), Enabled = false };
            this.tabPortAyarlari.Controls.Add(b);
            y += 52;
            return b;
        }

        private void BuildAnaliz()
        {
            var page = this.tabAnaliz;
            int y = 15;
            rbAnaAviyonik = new MaterialRadioButton { Text = "Ana Aviyonik", Location = new System.Drawing.Point(20, y), AutoSize = true };
            rbYedekAviyonik = new MaterialRadioButton { Text = "Yedek Aviyonik", Location = new System.Drawing.Point(180, y), AutoSize = true };
            rbGorevYuku = new MaterialRadioButton { Text = "Görev Yükü", Location = new System.Drawing.Point(360, y), AutoSize = true };
            rbTestler = new MaterialRadioButton { Text = "Testler", Location = new System.Drawing.Point(520, y), AutoSize = true };
            rbAnaAviyonik.CheckedChanged += new System.EventHandler(this.rbTablo_CheckedChanged);
            rbYedekAviyonik.CheckedChanged += new System.EventHandler(this.rbTablo_CheckedChanged);
            rbGorevYuku.CheckedChanged += new System.EventHandler(this.rbTablo_CheckedChanged);
            rbTestler.CheckedChanged += new System.EventHandler(this.rbTablo_CheckedChanged);
            page.Controls.Add(rbAnaAviyonik);
            page.Controls.Add(rbYedekAviyonik);
            page.Controls.Add(rbGorevYuku);
            page.Controls.Add(rbTestler);

            y = 60;
            Caption("Veritabanı Seçiniz", 20, y + 8, page);
            cmbVeritabani = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Location = new System.Drawing.Point(200, y + 4),
                Size = new System.Drawing.Size(420, 28),
                Font = new System.Drawing.Font("Segoe UI", 10F),
                Enabled = false
            };
            cmbVeritabani.SelectedIndexChanged += new System.EventHandler(this.cmbVeritabani_SelectedIndexChanged);
            cmbVeritabani.DropDown += new System.EventHandler(this.cmbVeritabani_DropDown);
            page.Controls.Add(cmbVeritabani);

            btnAnaAvDbOlustur = new MaterialButton { Text = "Ana Aviyonik DB Oluştur", Location = new System.Drawing.Point(650, y), AutoSize = false, Size = new System.Drawing.Size(230, 36) };
            btnAnaAvDbOlustur.Click += new System.EventHandler(this.btnAnaAvDbOlustur_Click);
            page.Controls.Add(btnAnaAvDbOlustur);
            btnYedekAvDbOlustur = new MaterialButton { Text = "Yedek Aviyonik DB Oluştur", Location = new System.Drawing.Point(890, y), AutoSize = false, Size = new System.Drawing.Size(240, 36) };
            btnYedekAvDbOlustur.Click += new System.EventHandler(this.btnYedekAvDbOlustur_Click);
            page.Controls.Add(btnYedekAvDbOlustur);
            btnGorevDbOlustur = new MaterialButton { Text = "Görev Yükü DB Oluştur", Location = new System.Drawing.Point(1140, y), AutoSize = false, Size = new System.Drawing.Size(220, 36) };
            btnGorevDbOlustur.Click += new System.EventHandler(this.btnGorevDbOlustur_Click);
            page.Controls.Add(btnGorevDbOlustur);
            btnExcelKaydet = new MaterialButton { Text = "Excel Olarak Kaydet", Location = new System.Drawing.Point(1370, y), AutoSize = false, Size = new System.Drawing.Size(190, 36) };
            btnExcelKaydet.Click += new System.EventHandler(this.btnExcelKaydet_Click);
            page.Controls.Add(btnExcelKaydet);

            gridVeri.Location = new System.Drawing.Point(20, 120);
            gridVeri.Size = new System.Drawing.Size(1540, 640);
            gridVeri.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            gridVeri.ReadOnly = true;
            gridVeri.AllowUserToAddRows = false;
            gridVeri.Name = "gridVeri";
            page.Controls.Add(gridVeri);
        }
    }
}
