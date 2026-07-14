using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Windows.Forms;
using ClosedXML.Excel;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using MaterialSkin;
using MaterialSkin.Controls;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using VolansGUI.Map;
using VolansGUI.Telemetry;

namespace VolansGUI
{
    public partial class Form1 : MaterialForm
    {
        // ---- Serial ----
        private readonly SerialPort loraSerialPort = new SerialPort();
        private bool loraTestLedActive = false;

        // ---- Telemetri + simülasyon ----
        private readonly Roket roket = new Roket(7);
        private System.Windows.Forms.Timer ucusTimer;
        private double ucusGecenZaman = 0;
        private bool isUcusTimerRunning = false;
        private int paketSayac = 0;

        // ---- Harita ----
        private GMapOverlay markersOverlay;
        private GMapOverlay routesOverlay;
        private GMarkerGoogle gpsMarker;
        private readonly List<PointLatLng> positions = new List<PointLatLng>();
        private const double startLat = 40.716582514101354;
        private const double startLng = 31.52471117735377;

        // ---- Grafikler ----
        private LineSeries irtifaSeries;
        private LineSeries aciSeries;
        private int grafikX = 0;

        public Form1()
        {
            InitializeComponent();

            // Veri klasörü + sim için ana aviyonik DB'si hazırla
            UcusVeriYoneticisi.BaslangicKontrolu();
            VeriTabani.AnaAviyonikDatabaseOlustur();

            // Form boyutu sabit: kullanıcı en/boy değiştiremesin
            this.Sizable = false;
            this.MaximizeBox = false;

            KurMaterialSkin();
            KurHarita();
            KurGrafikler();
            KurSerialCombos();

            // LoRa parser event abonelikleri
            LoRaPaketCozumleyici.BasincTestiVerisiAlindi += OnBasincTestiVerisiAlindi;
            LoRaPaketCozumleyici.HaberlesmeTestiVerisiAlindi += OnHaberlesmeTestiVerisiAlindi;
            LoRaPaketCozumleyici.JiroskopTestiVerisiAlindi += OnJiroskopTestiVerisiAlindi;
            LoRaPaketCozumleyici.AyrilmaAlgoritmaTestiVerisiAlindi += OnAyrilmaAlgoritmaTestiVerisiAlindi;
            loraSerialPort.DataReceived += LoraSerialPort_DataReceived;
            loraSerialPort.RtsEnable = true;
            loraSerialPort.DtrEnable = true;

            this.FormClosing += (s, e) =>
            {
                LoRaPaketCozumleyici.BasincTestiVerisiAlindi -= OnBasincTestiVerisiAlindi;
                LoRaPaketCozumleyici.HaberlesmeTestiVerisiAlindi -= OnHaberlesmeTestiVerisiAlindi;
                LoRaPaketCozumleyici.JiroskopTestiVerisiAlindi -= OnJiroskopTestiVerisiAlindi;
                LoRaPaketCozumleyici.AyrilmaAlgoritmaTestiVerisiAlindi -= OnAyrilmaAlgoritmaTestiVerisiAlindi;
                try { if (loraSerialPort.IsOpen) loraSerialPort.Close(); } catch { }
            };

            lblTakimIdDeger.Text = roket.TakimID.ToString();

            // Analiz sekmesi: varsayılan filtre seçili gelsin ki liste hemen dolsun
            rbAnaAviyonik.Checked = true;
        }

        private void Form1_Load(object sender, EventArgs e) { }

        // ==================================================================
        // SETUP
        // ==================================================================
        private void KurMaterialSkin()
        {
            var skin = MaterialSkinManager.Instance;
            skin.EnforceBackcolorOnAllComponents = false;
            skin.AddFormToManage(this);
            skin.Theme = MaterialSkinManager.Themes.LIGHT;
            skin.ColorScheme = new ColorScheme(
                Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500,
                Accent.LightBlue200, TextShade.WHITE);
        }

        private void KurHarita()
        {
            mapControl.MapProvider = GMapProviders.GoogleMap;
            GMaps.Instance.Mode = AccessMode.ServerOnly;
            mapControl.Position = new PointLatLng(startLat, startLng);
            mapControl.MinZoom = 1;
            mapControl.MaxZoom = 20;
            mapControl.Zoom = 15;
            mapControl.ShowCenter = false;

            markersOverlay = new GMapOverlay("markers");
            routesOverlay = new GMapOverlay("routes");
            mapControl.Overlays.Add(routesOverlay);
            mapControl.Overlays.Add(markersOverlay);

            gpsMarker = new GMarkerGoogle(new PointLatLng(startLat, startLng), GMarkerGoogleType.red_dot);
            markersOverlay.Markers.Add(gpsMarker);
        }

        private void KurGrafikler()
        {
            var irtifaModel = new PlotModel { Title = "İrtifa (m) — Zaman" };
            irtifaModel.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "Paket" });
            irtifaModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "İrtifa (m)" });
            irtifaSeries = new LineSeries { Title = "İrtifa", Color = OxyColors.SteelBlue, StrokeThickness = 2 };
            irtifaModel.Series.Add(irtifaSeries);
            plotIrtifa.Model = irtifaModel;

            var aciModel = new PlotModel { Title = "Açı (°) — Zaman" };
            aciModel.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "Paket" });
            aciModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Açı (°)" });
            aciSeries = new LineSeries { Title = "Açı", Color = OxyColors.IndianRed, StrokeThickness = 2 };
            aciModel.Series.Add(aciSeries);
            plotAci.Model = aciModel;
        }

        private void KurSerialCombos()
        {
            foreach (int b in new[] { 9600, 19200, 115200 }) cmbLoraBaud.Items.Add(b);
            cmbLoraDataBit.Items.Add(8);
            cmbLoraStopBit.Items.Add(1);
            cmbLoraStopBit.Items.Add(2);
            foreach (Parity p in new[] { Parity.None, Parity.Odd, Parity.Even, Parity.Mark, Parity.Space })
                cmbLoraParity.Items.Add(p);

            cmbLoraBaud.SelectedIndex = 0;
            cmbLoraDataBit.SelectedIndex = 0;
            cmbLoraStopBit.SelectedIndex = 0;
            cmbLoraParity.SelectedIndex = 0;
            PortlariYenile();
        }

        private void PortlariYenile()
        {
            cmbLoraPort.Items.Clear();
            cmbLoraPort.Items.AddRange(SerialPort.GetPortNames());
            if (cmbLoraPort.Items.Count > 0) cmbLoraPort.SelectedIndex = 0;
        }

        // ==================================================================
        // SERIAL — bağlantı
        // ==================================================================
        private void btnPortYenile_Click(object sender, EventArgs e) => PortlariYenile();

        private void btnLoraBaglan_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbLoraPort.SelectedItem == null) { MessageBox.Show("Lütfen bir seri port seçin."); return; }
                loraSerialPort.PortName = cmbLoraPort.SelectedItem.ToString();
                loraSerialPort.BaudRate = (int)cmbLoraBaud.SelectedItem;
                loraSerialPort.DataBits = (int)cmbLoraDataBit.SelectedItem;
                loraSerialPort.StopBits = (int)cmbLoraStopBit.SelectedItem == 2 ? StopBits.Two : StopBits.One;
                loraSerialPort.Parity = (Parity)cmbLoraParity.SelectedItem;

                loraSerialPort.Open();
                KomutButonlari(true);
                btnLoraBaglan.Enabled = false;
                btnLoraBaglantiKes.Enabled = true;
                cmbLoraPort.Enabled = cmbLoraBaud.Enabled = cmbLoraDataBit.Enabled =
                    cmbLoraStopBit.Enabled = cmbLoraParity.Enabled = false;

                string cihaz = SeriPortCihazAdi(loraSerialPort.PortName);
                Log($"Seri port açıldı: {loraSerialPort.PortName} @ {loraSerialPort.BaudRate} — {cihaz}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bağlantı hatası: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLoraBaglantiKes_Click(object sender, EventArgs e)
        {
            try
            {
                if (loraSerialPort.IsOpen) loraSerialPort.Close();
                KomutButonlari(false);
                btnLoraBaglan.Enabled = true;
                btnLoraBaglantiKes.Enabled = false;
                cmbLoraPort.Enabled = cmbLoraBaud.Enabled = cmbLoraDataBit.Enabled =
                    cmbLoraStopBit.Enabled = cmbLoraParity.Enabled = true;
                Log("Seri port kapatıldı.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bağlantı kesme hatası: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void KomutButonlari(bool aktif)
        {
            btnPingRoket.Enabled = btnPingGorevYuku.Enabled = btnPingPong.Enabled =
                btnParamCek.Enabled = btnTestLed.Enabled = btnFunyePatlat.Enabled = btnMesajGonder.Enabled = aktif;
        }

        private void LoraSerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                string gelen = ((SerialPort)sender).ReadExisting();
                if (!string.IsNullOrEmpty(gelen))
                    LoRaPaketCozumleyici.GelenVeriyiIsle(gelen);
            }
            catch (Exception ex)
            {
                // Worker thread — MessageBox yok, veri akışını bloklamayalım.
                Debug.WriteLine("[LoRa] Veri okuma hatasi: " + ex.Message);
            }
        }

        private string SeriPortCihazAdi(string portName)
        {
            try
            {
                using (var searcher = new ManagementObjectSearcher(
                    "SELECT * FROM Win32_PnPEntity WHERE Caption LIKE '%(" + portName + ")%'"))
                {
                    foreach (var device in searcher.Get())
                        return device["Caption"]?.ToString() ?? "Bilinmeyen cihaz";
                }
            }
            catch { /* WMI erişilemezse önemli değil */ }
            return "Bilinmeyen cihaz";
        }

        // ==================================================================
        // SERIAL — komutlar
        // ==================================================================
        private void GonderBytes(byte[] paket, string log)
        {
            if (!loraSerialPort.IsOpen) { MessageBox.Show("LoRa portu açık değil."); return; }
            try { loraSerialPort.Write(paket, 0, paket.Length); Log(log); }
            catch (Exception ex) { MessageBox.Show("Gönderim hatası: " + ex.Message); }
        }

        private void btnPingRoket_Click(object sender, EventArgs e) =>
            GonderBytes(new byte[] { 0x0, 0x2c, 0x17, 0x76, 0x6F, 0x6C, 0x61, 0x6E, 0x73 }, "Ping Roket gönderildi.");

        private void btnPingGorevYuku_Click(object sender, EventArgs e) =>
            GonderBytes(new byte[] { 0x0, 0x2c, 0x17, 0x67, 0x79 }, "Ping Görev Yükü gönderildi.");

        private void btnPingPong_Click(object sender, EventArgs e) =>
            GonderBytes(new byte[] { 0xc1, 0xc1, 0xc1 }, "Ping Pong test gönderildi.");

        private void btnParamCek_Click(object sender, EventArgs e) =>
            GonderBytes(new byte[] { 0xc1, 0xc1, 0xc1 }, "Parametre okuma komutu gönderildi.");

        private void btnTestLed_Click(object sender, EventArgs e)
        {
            const byte AVIONIK_ADDR_H = 0x00, AVIONIK_ADDR_L = 0x02, CHANNEL = 0x17;
            loraTestLedActive = !loraTestLedActive;
            byte komut = loraTestLedActive ? (byte)'L' : (byte)'l';
            GonderBytes(new byte[] { AVIONIK_ADDR_H, AVIONIK_ADDR_L, CHANNEL, komut },
                $"Aviyonik LED komutu: {(loraTestLedActive ? "AÇ" : "KAPAT")}");
            btnTestLed.Text = loraTestLedActive ? "Test: Aviyonik LED Kapat" : "Test: Aviyonik LED Yak";
        }

        private void btnFunyePatlat_Click(object sender, EventArgs e)
        {
            var onay = MessageBox.Show(
                "Fünye patlatma komutu gönderilecek. Emin misiniz?",
                "Fünye Patlatma Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (onay != DialogResult.Yes) return;
            GonderBytes(new byte[] { 0x00, 0x01, 0x17, 0x6D, 0x61, 0x68, 0x6D, 0x75, 0x74, 0x20, 0x63, 0x61, 0x6E },
                "FÜNYE PATLATMA komutu gönderildi.");
        }

        private void btnMesajGonder_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtLoraMesaj.Text)) { MessageBox.Show("Boş mesaj gönderilemez."); return; }
            byte[] paket = System.Text.Encoding.ASCII.GetBytes(txtLoraMesaj.Text);
            GonderBytes(paket, $"Mesaj gönderildi: {txtLoraMesaj.Text}");
        }

        private void Log(string mesaj)
        {
            if (txtLog.InvokeRequired) { txtLog.BeginInvoke(new Action<string>(Log), mesaj); return; }
            txtLog.AppendText($"{DateTime.Now:HH:mm:ss}  {mesaj}{Environment.NewLine}");
        }

        // ==================================================================
        // PARSER EVENT'LERİ → dashboard + grafik
        // ==================================================================
        private void OnBasincTestiVerisiAlindi(BasincTestiVerisi veri)
        {
            if (InvokeRequired) { try { BeginInvoke(new Action<BasincTestiVerisi>(OnBasincTestiVerisiAlindi), veri); } catch (ObjectDisposedException) { } return; }
            lblBasincDeger.Text = veri.Basinc.ToString("F2");
            lblSicaklikDeger.Text = veri.Sicaklik.ToString("F2");
            lblNemDeger.Text = veri.Nem.ToString("F2");
            lblBasincIrtifaDeger.Text = veri.GoreceliIrtifa.ToString("F2");
            lblPaketSayaciDeger.Text = veri.PaketSayaci.ToString();
            AyrilmaEtiketi(lblBasincAyrilma, veri.AyrilmaDurumu, "Basınç İle Ayrılma");
            GrafikEkle(veri.GoreceliIrtifa, null);
        }

        private void OnHaberlesmeTestiVerisiAlindi(HaberlesmeTestiVerisi veri)
        {
            if (InvokeRequired) { try { BeginInvoke(new Action<HaberlesmeTestiVerisi>(OnHaberlesmeTestiVerisiAlindi), veri); } catch (ObjectDisposedException) { } return; }
            lblJiroskopXDeger.Text = veri.GyroX.ToString("F2");
            lblJiroskopYDeger.Text = veri.GyroY.ToString("F2");
            lblJiroskopZDeger.Text = veri.GyroZ.ToString("F2");
            lblAciDeger.Text = veri.ToplamAci.ToString("F2");
            lblPaketSayaciDeger.Text = veri.PaketSayaci.ToString();
            lblAciAyrilma.Text = veri.LedOn ? "Aviyonik LED: AÇIK" : "Aviyonik LED: KAPALI";
            lblAciAyrilma.ForeColor = veri.LedOn ? Color.Green : Color.Black;
            GrafikEkle(null, veri.ToplamAci);
        }

        private void OnJiroskopTestiVerisiAlindi(JiroskopTestiVerisi veri)
        {
            if (InvokeRequired) { try { BeginInvoke(new Action<JiroskopTestiVerisi>(OnJiroskopTestiVerisiAlindi), veri); } catch (ObjectDisposedException) { } return; }
            lblJiroskopXDeger.Text = veri.JiroskopX.ToString("F2");
            lblJiroskopYDeger.Text = veri.JiroskopY.ToString("F2");
            lblJiroskopZDeger.Text = veri.JiroskopZ.ToString("F2");
            lblAciDeger.Text = veri.Aci.ToString("F2");
            lblPaketSayaciDeger.Text = veri.PaketSayaci.ToString();
            AyrilmaEtiketi(lblAciAyrilma, veri.EgimTetiklendi, "Açı İle Ayrılma");
            GrafikEkle(null, veri.Aci);
        }

        private void OnAyrilmaAlgoritmaTestiVerisiAlindi(AyrilmaAlgoritmaTestiVerisi veri)
        {
            if (InvokeRequired) { try { BeginInvoke(new Action<AyrilmaAlgoritmaTestiVerisi>(OnAyrilmaAlgoritmaTestiVerisiAlindi), veri); } catch (ObjectDisposedException) { } return; }
            lblBasincDeger.Text = veri.Basinc.ToString("F2");
            lblSicaklikDeger.Text = veri.Sicaklik.ToString("F2");
            lblNemDeger.Text = veri.Nem.ToString("F2");
            lblBasincIrtifaDeger.Text = veri.Irtifa.ToString("F2");
            lblJiroskopXDeger.Text = veri.JiroskopX.ToString("F2");
            lblJiroskopYDeger.Text = veri.JiroskopY.ToString("F2");
            lblJiroskopZDeger.Text = veri.JiroskopZ.ToString("F2");
            lblAciDeger.Text = veri.Aci.ToString("F2");
            lblPaketSayaciDeger.Text = veri.PaketSayaci.ToString();
            AyrilmaEtiketi(lblBasincAyrilma, veri.BasincKosulu, "Basınç İle Ayrılma");
            AyrilmaEtiketi(lblAciAyrilma, veri.EgimKosulu, "Açı İle Ayrılma");
            this.Text = veri.AyrilmaDurumu
                ? "          VOLANS ROKET TAKIMI — YER İSTASYONU   ⚠ AYRILMA TETİKLENDİ ⚠"
                : "          VOLANS ROKET TAKIMI — YER İSTASYONU";
            GrafikEkle(veri.Irtifa, veri.Aci);
        }

        private static void AyrilmaEtiketi(MaterialLabel lbl, bool tetiklendi, string ad)
        {
            lbl.Text = tetiklendi ? $"{ad} Tetiklendi" : $"{ad} Tetiklenmedi";
            lbl.ForeColor = tetiklendi ? Color.Red : Color.Black;
        }

        private void GrafikEkle(float? irtifa, float? aci)
        {
            grafikX++;
            if (irtifa.HasValue)
            {
                irtifaSeries.Points.Add(new DataPoint(grafikX, irtifa.Value));
                if (irtifaSeries.Points.Count > 300) irtifaSeries.Points.RemoveAt(0);
                plotIrtifa.InvalidatePlot(true);
            }
            if (aci.HasValue)
            {
                aciSeries.Points.Add(new DataPoint(grafikX, aci.Value));
                if (aciSeries.Points.Count > 300) aciSeries.Points.RemoveAt(0);
                plotAci.InvalidatePlot(true);
            }
        }

        // ==================================================================
        // SİMÜLASYON
        // ==================================================================
        private void btnSimBaslat_Click(object sender, EventArgs e)
        {
            if (isUcusTimerRunning)
            {
                ucusTimer?.Stop();
                isUcusTimerRunning = false;
                btnSimBaslat.Text = "Simülasyonu Başlat";
            }
            else
            {
                if (ucusTimer == null)
                {
                    ucusTimer = new System.Windows.Forms.Timer { Interval = 200 };
                    ucusTimer.Tick += UcusTimer_Tick;
                }
                ucusTimer.Start();
                isUcusTimerRunning = true;
                btnSimBaslat.Text = "Simülasyonu Durdur";
            }
        }

        private void btnSimSifirla_Click(object sender, EventArgs e)
        {
            ucusTimer?.Stop();
            isUcusTimerRunning = false;
            ucusGecenZaman = 0;
            paketSayac = 0;
            positions.Clear();
            routesOverlay.Routes.Clear();
            routesOverlay.Polygons.Clear();
            mapControl.Refresh();
            btnSimBaslat.Text = "Simülasyonu Başlat";
            lblGecenSureDeger.Text = "0";
        }

        private void UcusTimer_Tick(object sender, EventArgs e)
        {
            ucusGecenZaman += 0.2;
            UcusSim.RoketiGuncelle(roket, ucusGecenZaman);
            paketSayac++;

            if (roket.RoketGpsIrtifa <= 1 && ucusGecenZaman > 1)
            {
                ucusTimer.Stop();
                isUcusTimerRunning = false;
                btnSimBaslat.Text = "Simülasyonu Başlat";
            }

            double hiz = UcusSim.HizHesap(ucusGecenZaman);
            lblGecenSureDeger.Text = ucusGecenZaman.ToString("F1");
            lblGpsIrtifaDeger.Text = roket.RoketGpsIrtifa.ToString("F1");
            lblRoketEnlemDeger.Text = roket.RoketEnlem.ToString("F6");
            lblRoketBoylamDeger.Text = roket.RoketBoylam.ToString("F6");
            lblGorevEnlemDeger.Text = roket.GorevYukuEnlem.ToString("F6");
            lblGorevBoylamDeger.Text = roket.GorevYukuBoylam.ToString("F6");
            lblHizDeger.Text = hiz.ToString("F1");
            lblPaketSayaciDeger.Text = paketSayac.ToString();

            HaritaGuncelle(new PointLatLng(roket.RoketEnlem, roket.RoketBoylam));

            VeriTabani.AnaAviyonikVeriEkle(0, roket.BasincIrtifa, 2, paketSayac, roket.BasincIrtifa,
                roket.RoketSicaklik, (float)hiz, (float)hiz, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, roket.Aci,
                roket.RoketEnlem, roket.RoketBoylam, roket.RoketGpsIrtifa);

            GrafikEkle(roket.RoketGpsIrtifa, null);
        }

        private void HaritaGuncelle(PointLatLng nokta)
        {
            gpsMarker.Position = nokta;
            mapControl.Position = nokta;
            positions.Add(nokta);

            if (positions.Count >= 2)
                new DrawMode_GradientLine { CizgiKalinlik = 3f }.Draw(routesOverlay, positions);
            if (positions.Count >= 3)
                new DrawMode_Area().Draw(routesOverlay, positions);

            mapControl.Refresh();
        }

        private void btnKonumaGit_Click(object sender, EventArgs e)
        {
            try
            {
                double lat = double.Parse(txtKonumEnlem.Text, CultureInfo.InvariantCulture);
                double lng = double.Parse(txtKonumBoylam.Text, CultureInfo.InvariantCulture);
                mapControl.Position = new PointLatLng(lat, lng);
            }
            catch { MessageBox.Show("Geçerli bir enlem/boylam giriniz."); }
        }

        private void mapControl_MouseUp(object sender, MouseEventArgs e)
        {
            lblHaritaEnlemDeger.Text = mapControl.Position.Lat.ToString("F6");
            lblHaritaBoylamDeger.Text = mapControl.Position.Lng.ToString("F6");
        }

        // ==================================================================
        // ANALİZ (TABLO)
        // ==================================================================
        private void rbTablo_CheckedChanged(object sender, EventArgs e)
        {
            if (sender is MaterialRadioButton rb && !rb.Checked) return;
            VeritabaniListesiniGuncelle();
            cmbVeritabani.Enabled = rbAnaAviyonik.Checked || rbYedekAviyonik.Checked
                                    || rbGorevYuku.Checked || rbTestler.Checked;
        }

        private void VeritabaniListesiniGuncelle()
        {
            cmbVeritabani.Items.Clear();
            string[] dosyalar;
            if (rbAnaAviyonik.Checked) dosyalar = UcusVeriYoneticisi.DosyalariListele("VeriAnaAv*.db");
            else if (rbYedekAviyonik.Checked) dosyalar = UcusVeriYoneticisi.DosyalariListele("VeriYedekAv*.db");
            else if (rbGorevYuku.Checked) dosyalar = UcusVeriYoneticisi.DosyalariListele("VeriGorevYuku*.db");
            else if (rbTestler.Checked) dosyalar = UcusVeriYoneticisi.DosyalariListele(TestTanimlari.TumTestDosyalariPattern);
            else dosyalar = Array.Empty<string>();

            foreach (var d in dosyalar)
                cmbVeritabani.Items.Add(System.IO.Path.GetFileNameWithoutExtension(d));
        }

        private void cmbVeritabani_DropDown(object sender, EventArgs e)
        {
            // Açılışta listeyi tazele: sim/DB oluştur sonrası yeni dosyalar görünsün
            object seciliydi = cmbVeritabani.SelectedItem;
            VeritabaniListesiniGuncelle();
            if (seciliydi != null && cmbVeritabani.Items.Contains(seciliydi))
                cmbVeritabani.SelectedItem = seciliydi;
        }

        private void cmbVeritabani_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbVeritabani.SelectedItem == null) return;
            string ad = cmbVeritabani.SelectedItem.ToString();
            string yol = UcusVeriYoneticisi.DosyaYolu(ad + ".db");

            string tabloAdi;
            if (rbAnaAviyonik.Checked) tabloAdi = "AnaAviyonikTablosu";
            else if (rbYedekAviyonik.Checked) tabloAdi = "YedekAviyonikTablosu";
            else if (rbGorevYuku.Checked) tabloAdi = "GorevYukuTablosu";
            else if (rbTestler.Checked)
            {
                TestTipi tip = TestTanimlari.DosyaAdindanTipiBul(ad);
                if (tip == TestTipi.Bilinmeyen) { MessageBox.Show("Test türü tespit edilemedi: " + ad); return; }
                tabloAdi = TestTanimlari.Tanimlar[tip].TabloAdi;
            }
            else return;

            try
            {
                gridVeri.DataSource = VeriTabani.TabloyuOku(yol, tabloAdi);
                foreach (DataGridViewColumn col in gridVeri.Columns)
                    col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
            catch (Exception ex) { MessageBox.Show("Tablo okuma hatası: " + ex.Message); }
        }

        private void btnAnaAvDbOlustur_Click(object sender, EventArgs e)
        {
            VeriTabani.AnaAviyonikDatabaseOlustur();
            if (rbAnaAviyonik.Checked) VeritabaniListesiniGuncelle();
            MessageBox.Show("Ana Aviyonik veritabanı oluşturuldu.");
        }

        private void btnYedekAvDbOlustur_Click(object sender, EventArgs e)
        {
            VeriTabani.YedekAviyonikDatabaseOlustur();
            if (rbYedekAviyonik.Checked) VeritabaniListesiniGuncelle();
            MessageBox.Show("Yedek Aviyonik veritabanı oluşturuldu.");
        }

        private void btnGorevDbOlustur_Click(object sender, EventArgs e)
        {
            VeriTabani.GorevYukuDatabaseOlustur();
            if (rbGorevYuku.Checked) VeritabaniListesiniGuncelle();
            MessageBox.Show("Görev Yükü veritabanı oluşturuldu.");
        }

        private void btnExcelKaydet_Click(object sender, EventArgs e)
        {
            if (gridVeri.DataSource is not DataTable dt || dt.Rows.Count == 0)
            {
                MessageBox.Show("Önce bir veritabanı seçip tabloyu görüntüleyin.");
                return;
            }
            try
            {
                using (var sfd = new SaveFileDialog { Filter = "Excel Workbook|*.xlsx", ValidateNames = true })
                {
                    if (sfd.ShowDialog() != DialogResult.OK) return;
                    using (var wb = new XLWorkbook())
                    {
                        wb.Worksheets.Add(dt, "Veri");
                        wb.SaveAs(sfd.FileName);
                    }
                    MessageBox.Show("Veriler Excel'e aktarıldı.");
                }
            }
            catch (Exception ex) { MessageBox.Show("Excel'e aktarma hatası: " + ex.Message); }
        }
    }
}
