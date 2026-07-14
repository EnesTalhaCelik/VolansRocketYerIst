using GMap.NET.MapProviders;
using GMap.NET;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static GMap.NET.Entity.OpenStreetMapGraphHopperGeocodeEntity;
using System.IO.Ports;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Spreadsheet;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using GMap.NET.WindowsForms.ToolTips;
using System.Data.Entity;
using TeknofestVeriler;
using GMap.NET.WindowsPresentation;
using GMapRoute = GMap.NET.WindowsForms.GMapRoute;
using VolansYerIstasyonu.DrawMode;



namespace VolansYerIstasyonu.UserControls

{

    
    public partial class uc_AnaSayfa : UserControl
    {

        public static Roket Roket = new Roket (1,2,3,4,5);

        private GMapOverlay markersOverlay;
        private GMarkerGoogle gpsMarker;

        private Timer timer;
        private GMapOverlay overlay;
        private GMapOverlay routesOverlay;
        private Random random = new Random();
        private GMap.NET.WindowsForms.GMapRoute routeAna;
        private GMap.NET.WindowsForms.GMapRoute routeYedek;
        private GMap.NET.WindowsForms.GMapRoute routeGorev;
        private bool isSeparated = false;
        private List<PointLatLng> anaAviyonikCoordinates = new List<PointLatLng>();
        private List<PointLatLng> gorevYukuCoordinates = new List<PointLatLng>();
        public static Timer ucusTimer;
        public static double ucusGecenZaman = 0;
        private bool isUcusTimerRunning = false;
        int paketSayac = 0;
        private List<PointLatLng> positions;

        public uc_AnaSayfa()
        {
            InitializeComponent();
            InitializeMap();

            anasayfaHarita.MapProvider = GMap.NET.MapProviders.GMapProviders.OpenStreetMap;
            GMaps.Instance.Mode = AccessMode.ServerAndCache;
            anasayfaHarita.Position = new GMap.NET.PointLatLng(40.7166, 31.5247);
            anasayfaHarita.MinZoom = 1;
            anasayfaHarita.MaxZoom = 20;
            anasayfaHarita.Zoom = 15;

            markersOverlay = new GMapOverlay("markers");
            anasayfaHarita.Overlays.Add(markersOverlay);

            gpsMarker = new GMarkerGoogle(new PointLatLng(40.7350, 31.6060), GMarkerGoogleType.blue);
            markersOverlay.Markers.Add(gpsMarker);
            positions = new List<PointLatLng>();

            // =============================================================
            // YENİ - LoRa parser event'lerine abone ol
            // =============================================================
            LoRaPaketCozumleyici.BasincTestiVerisiAlindi += OnBasincTestiVerisiAlindi;
            LoRaPaketCozumleyici.HaberlesmeTestiVerisiAlindi += OnHaberlesmeTestiVerisiAlindi;
            LoRaPaketCozumleyici.JiroskopTestiVerisiAlindi += OnJiroskopTestiVerisiAlindi;
            LoRaPaketCozumleyici.AyrilmaAlgoritmaTestiVerisiAlindi += OnAyrilmaAlgoritmaTestiVerisiAlindi;

            this.Disposed += (s, e) =>
            {
                LoRaPaketCozumleyici.BasincTestiVerisiAlindi -= OnBasincTestiVerisiAlindi;
                LoRaPaketCozumleyici.HaberlesmeTestiVerisiAlindi -= OnHaberlesmeTestiVerisiAlindi;
                LoRaPaketCozumleyici.JiroskopTestiVerisiAlindi -= OnJiroskopTestiVerisiAlindi;
                LoRaPaketCozumleyici.AyrilmaAlgoritmaTestiVerisiAlindi -= OnAyrilmaAlgoritmaTestiVerisiAlindi;
            };
        }

        private void OnBasincTestiVerisiAlindi(BasincTestiVerisi veri)
        {
            if (this.InvokeRequired)
            {
                try
                {
                    this.BeginInvoke(new Action<BasincTestiVerisi>(OnBasincTestiVerisiAlindi), veri);
                }
                catch (ObjectDisposedException) { }
                return;
            }

            Basinc_dgr.Text = veri.Basinc.ToString("F2");
            sicaklik_dgr.Text = veri.Sicaklik.ToString("F2");
            nem_dgr.Text = veri.Nem.ToString("F2");
            lbl_bsncIrtıfa_dgr.Text = veri.GoreceliIrtifa.ToString("F2");
            lbl_paketSayac_dgr.Text = veri.PaketSayaci.ToString();

            basincTetiklendiMi.Text = veri.AyrilmaDurumu
                ? "Basınç İle Ayrılma Tetiklendi"
                : "Basınç İle Ayrılma Tetiklenmedi";

            basincTetiklendiMi.ForeColor = veri.AyrilmaDurumu
                ? System.Drawing.Color.Red
                : System.Drawing.Color.Black;
        }


        private void OnHaberlesmeTestiVerisiAlindi(HaberlesmeTestiVerisi veri)
        {
            if (this.InvokeRequired)
            {
                try
                {
                    this.BeginInvoke(new Action<HaberlesmeTestiVerisi>(OnHaberlesmeTestiVerisiAlindi), veri);
                }
                catch (ObjectDisposedException) { }
                return;
            }

            // Jiroskop label'ları
            JiroskopX_dgr.Text = veri.GyroX.ToString("F2");
            JiroskopY_dgr.Text = veri.GyroY.ToString("F2");
            JiroskopZ_dgr.Text = veri.GyroZ.ToString("F2");

            // Açı label'ı (label32 = Aci_dgr)
            label32.Text = veri.ToplamAci.ToString("F2");

            // Paket sayacı
            lbl_paketSayac_dgr.Text = veri.PaketSayaci.ToString();

            // LED durumu - aviyoniğin geri bildirimi
            // Bu, butona basıldıktan sonra aviyoniğin "anladım" sinyalidir
            aciTetiklendiMi.Text = veri.LedOn
                ? "Aviyonik LED: AÇIK"
                : "Aviyonik LED: KAPALI";
            aciTetiklendiMi.ForeColor = veri.LedOn
                ? System.Drawing.Color.Green
                : System.Drawing.Color.Black;
        }

        private void HaberlesmeTestiHaritayiGuncelle(PointLatLng yeniNokta)
        {
            gpsMarker.Position = yeniNokta;
            anasayfaHarita.Position = yeniNokta;

            positions.Add(yeniNokta);

            // Gradient'in doğru hesaplanması için tüm segmentler her seferinde
            // yeniden çizilir. DrawMode_GradientLine zaten routesOverlay.Routes'u
            // önce temizliyor; bizim Clear çağırmamıza gerek yok.
            if (positions.Count >= 2)
            {
                new DrawMode_GradientLine
                {
                    BaslangicRengi = System.Drawing.Color.Red,
                    SonRengi = System.Drawing.Color.Blue,
                    CizgiKalinlik = 3f
                }.Draw(routesOverlay, positions);
            }

            // 3. noktadan itibaren alan taraması (poligon)
            if (positions.Count >= 3)
            {
                new DrawMode_Area().Draw(routesOverlay, positions);
            }

            anasayfaHarita.Refresh();
        }

        private void OnJiroskopTestiVerisiAlindi(JiroskopTestiVerisi veri)
        {
            if (this.InvokeRequired)
            {
                try
                {
                    this.BeginInvoke(new Action<JiroskopTestiVerisi>(OnJiroskopTestiVerisiAlindi), veri);
                }
                catch (ObjectDisposedException) { }
                return;
            }

            // ----- Jiroskop ham verileri (°/s) -----
            JiroskopX_dgr.Text = veri.JiroskopX.ToString("F2");
            JiroskopY_dgr.Text = veri.JiroskopY.ToString("F2");
            JiroskopZ_dgr.Text = veri.JiroskopZ.ToString("F2");

            // ----- Hesaplanmış açı (yer normali ile, °) -----
            // Mevcut "Açı" değer label'ı şu an "label32" adıyla designer'da.
            // Tutarlılık için designer'da bunu Aci_dgr olarak rename etmenizi
            // öneririm. Şimdilik label32 adıyla kullanıyorum.
            label32.Text = veri.Aci.ToString("F2");

            // ----- Paket sayacı (diğer testlerle ortak) -----
            lbl_paketSayac_dgr.Text = veri.PaketSayaci.ToString();

            // ----- Eğim ile ayrılma tetikleme durumu -----
            aciTetiklendiMi.Text = veri.EgimTetiklendi
                ? "Açı İle Ayrılma Tetiklendi"
                : "Açı İle Ayrılma Tetiklenmedi";

            aciTetiklendiMi.ForeColor = veri.EgimTetiklendi
                ? System.Drawing.Color.Red
                : System.Drawing.Color.Black;
        }


        private void OnAyrilmaAlgoritmaTestiVerisiAlindi(AyrilmaAlgoritmaTestiVerisi veri)
        {
            if (this.InvokeRequired)
            {
                try
                {
                    this.BeginInvoke(new Action<AyrilmaAlgoritmaTestiVerisi>(OnAyrilmaAlgoritmaTestiVerisiAlindi), veri);
                }
                catch (ObjectDisposedException) { }
                return;
            }

            // ----- BME280 verileri -----
            Basinc_dgr.Text = veri.Basinc.ToString("F2");
            sicaklik_dgr.Text = veri.Sicaklik.ToString("F2");
            nem_dgr.Text = veri.Nem.ToString("F2");
            lbl_bsncIrtıfa_dgr.Text = veri.Irtifa.ToString("F2");

            // ----- Jiroskop ham verileri (°/s) -----
            JiroskopX_dgr.Text = veri.JiroskopX.ToString("F2");
            JiroskopY_dgr.Text = veri.JiroskopY.ToString("F2");
            JiroskopZ_dgr.Text = veri.JiroskopZ.ToString("F2");

            // ----- Hesaplanmış açı (°) -----
            label32.Text = veri.Aci.ToString("F2");

            // ----- Paket sayacı -----
            lbl_paketSayac_dgr.Text = veri.PaketSayaci.ToString();

            // ----- Basınç koşulu (irtifa kaybı eşik geçildi mi?) -----
            basincTetiklendiMi.Text = veri.BasincKosulu
                ? "Basınç İle Ayrılma Tetiklendi"
                : "Basınç İle Ayrılma Tetiklenmedi";
            basincTetiklendiMi.ForeColor = veri.BasincKosulu
                ? System.Drawing.Color.Red
                : System.Drawing.Color.Black;

            // ----- Eğim koşulu (açı eşik geçildi mi?) -----
            aciTetiklendiMi.Text = veri.EgimKosulu
                ? "Açı İle Ayrılma Tetiklendi"
                : "Açı İle Ayrılma Tetiklenmedi";
            aciTetiklendiMi.ForeColor = veri.EgimKosulu
                ? System.Drawing.Color.Red
                : System.Drawing.Color.Black;

            // ----- Genel ayrılma durumu: form başlığı uyarısı -----
            // Parent form'a ulaş; UserControl tek başına çalışırken FindForm() null dönebilir.
            var parentForm = this.FindForm();
            if (parentForm != null)
            {
                // Mevcut başlık metnini saklayıp uyarı eklemek/kaldırmak yerine
                // sabit bir patern kullanıyoruz. İlk ayrılma tetiklendiğinde
                // başlığa uyarı eklenir; tetiklenmediği durumlarda temizlenir.
                const string uyariMetni = "  ⚠  AYRILMA TETİKLENDİ  ⚠";
                string mevcutBaslik = parentForm.Text ?? string.Empty;

                if (veri.AyrilmaDurumu)
                {
                    if (!mevcutBaslik.Contains(uyariMetni))
                        parentForm.Text = mevcutBaslik + uyariMetni;
                }
                else
                {
                    if (mevcutBaslik.Contains(uyariMetni))
                        parentForm.Text = mevcutBaslik.Replace(uyariMetni, "");
                }
            }
        }

        private void InitializeUcusTimer()
        {
            ucusTimer = new Timer();
            ucusTimer.Interval = 200; // 0.2 saniye 5Hz
            ucusTimer.Tick += OnUcusTimerTick;
            ucusTimer.Start();
        }

        private void StopUcusTimer()
        {
            ucusTimer.Stop();
        }


        private Bitmap ResizeImage(Bitmap imgToResize, int width, int height)
        {
            return new Bitmap(imgToResize, new System.Drawing.Size(width, height));
        }

        private void OnUcusTimerTick(object sender, EventArgs e)
        {
            ucusGecenZaman += 0.2;
            gecenSure.Text = $"{ucusGecenZaman}";
            
            Roket.RoketGpsIrtifa = (float)(UcusSim.yukseklikHesap(ucusGecenZaman));

            if (Roket.RoketGpsIrtifa <= 1)
            {
                StopUcusTimer();
                isUcusTimerRunning = false;
                roketGPSIrtifa.Text = "0";
                btnSimBaslat.Text = "Simülasyonu Başlat";
                
            }

            UcusSim.enlemHesapla(ucusGecenZaman);
            UcusSim.boylamHesapla(ucusGecenZaman);
            roketGPSIrtifa.Text = $"{UcusSim.yukseklikHesap(ucusGecenZaman)}";
            roketEnlem.Text = $"{Roket.RoketEnlem}";
            roketBoylam.Text = $"{Roket.RoketBoylam}";
            UcusSim.gorevYukuEnlemHesapla(ucusGecenZaman);
            UcusSim.gorevYukuBoylamHesapla(ucusGecenZaman);
            gYukuEnlem.Text = $"{Roket.GorevYukuEnlem}";
            gYukuBoylam.Text = $"{Roket.GorevYukuBoylam}";
            roketHizY.Text = $"{UcusSim.hizHesap(ucusGecenZaman)}";
            UpdateMapIsaret(new PointLatLng(Roket.RoketEnlem,Roket.RoketBoylam));
            paketSayac++;
            uc_VeriTablo.anaAviyonikVeriEkle(0,Roket.BasincIrtifa,2,paketSayac,Roket.BasincIrtifa,Roket.RoketSicaklik,paketSayac*paketSayac + 5,(float)UcusSim.hizHesap(ucusGecenZaman),paketSayac+5,0,0,0,0,0,0,2,12,1,0,Roket.RoketEnlem,Roket.RoketBoylam,Roket.RoketGpsIrtifa);


            //Database.addDataGorevYuku();
            //Database.addDataRoketBilgileri();
            //Database.addDataSensorVerileri();

            //ivmeXAnaSayfa.Text = $"{UcusSim.hizHesap(ucusGecenZaman)}";
            //ivmeYAnaSayfa.Text = $"{UcusSim.anlikYukseklikHesapla(ucusGecenZaman)}";
            //gpsirtifaAnaSayfa.Text = $"{Veriler.roketGpsIrtifa}";


            /*
            UpdateRocketPosition(newLatitude, newLongitude);
            UpdateDistanceLabel();*/
            // konum güncelleme kodları buraya yerleştirilebilir.

        }



        private void anasayfaHarita_MouseUp(object sender, MouseEventArgs e)
        {
            //ana sayfadaki harita cursor label larının değerlerini senkron eder 
            //Harita pozisyonu değişimi tetiklenince çağıracağım.
            lbl_haritaC_enlem_dgr.Text = anasayfaHarita.Position.Lat.ToString();
            lbl_haritaC_boylam_dgr.Text = anasayfaHarita.Position.Lng.ToString();
        }

        private void senkronHaritaCursorPozisyon(object sender, ScrollEventArgs e)
        {
            lbl_haritaC_enlem_dgr.Text = anasayfaHarita.Position.Lat.ToString();
            lbl_haritaC_boylam_dgr.Text = anasayfaHarita.Position.Lng.ToString();
        }

        private void senkronHaritaCursorPozisyon(object sender,EventArgs e, ScrollEventArgs b)
        {
            //ana sayfadaki harita cursor label larının değerlerini senkron eder 
            //Harita pozisyonu değişimi tetiklenince çağıracağım.

            lbl_haritaC_enlem_dgr.Text = anasayfaHarita.Position.Lat.ToString();
            lbl_haritaC_boylam_dgr.Text = anasayfaHarita.Position.Lng.ToString();
        }


        private void InitializeMap()
        {
            anasayfaHarita.MapProvider = GMap.NET.MapProviders.GoogleMapProvider.Instance;
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerOnly;
            anasayfaHarita.Position = new GMap.NET.PointLatLng(40.716582514101354, 31.52471117735377); // Bolu, Türkiye
            anasayfaHarita.MinZoom = 1;
            anasayfaHarita.MaxZoom = 20;
            anasayfaHarita.Zoom = 15;


            overlay = new GMapOverlay("markers");
            routesOverlay = new GMapOverlay("routes");
           
            anasayfaHarita.Overlays.Add(overlay);
            anasayfaHarita.Overlays.Add(routesOverlay);
            anasayfaHarita.ShowCenter = false;

        }


        private void anasayfaHarita_Load(object sender, EventArgs e)
        {
            InitializeMap();
        }

        private void uc_AnaSayfa_Load(object sender, EventArgs e)
        {

        }

        private void lbl_haritaC_boylam_dgr_Click(object sender, EventArgs e)
        {

        }

        private void anasayfaHarita_Scroll(object sender, ScrollEventArgs e)
        {
            senkronHaritaCursorPozisyon(sender, e);
        }

        private void btnKonumaGit_Click(object sender, EventArgs e)
        {

            try
            {
                anasayfaHarita.Position = new PointLatLng(Double.Parse(konumaGit_Enlem.Text), Double.Parse(konumaGit_Boylam.Text));
            }
            catch (Exception)
            {
                MessageBox.Show($"Geçerli bir konum giriniz.");
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void aciTetiklendiMİ_Click(object sender, EventArgs e)
        {

        }

        private void btnSimBaslat_Click(object sender, EventArgs e)
        {
            if (isUcusTimerRunning)
            {
                StopUcusTimer();
                btnSimBaslat.Text = "Simülasyonu Başlat";
                isUcusTimerRunning = false;
            }
            else
            {
                InitializeUcusTimer();
                btnSimBaslat.Text = "Simülasyonu Durdur";
                isUcusTimerRunning = true;
            }






            /*
            //test degerleri
            double ana_av_x = 39.9334;
            double ana_av_y = 32.8597;
            double yedek_av_x = 38.4237;
            double yedek_av_y = 27.1428;
            double gorev_yuku_x = 41.0082;
            double gorev_yuku_y = 28.9784;
            int durum = 0; 

            UpdateMap(ana_av_x, ana_av_y, yedek_av_x, yedek_av_y, gorev_yuku_x, gorev_yuku_y, durum);*/
        }



        private void UpdateMapIsaret(PointLatLng point)
        {
            if (anasayfaHarita.InvokeRequired)
            {
                anasayfaHarita.Invoke(new MethodInvoker(delegate
                {
                    ProcessNewPoint(point);
                }));
            }
            else
            {
                ProcessNewPoint(point);
            }
        }

        private void ProcessNewPoint(PointLatLng point)
        {
            gpsMarker.Position = point;
            anasayfaHarita.Position = point; // Haritayı yeni noktaya merkezle

            positions.Add(point);

            // Çizgiyi sadece son iki nokta arasında çiz
            if (positions.Count > 1)
            {
                var lastTwoPoints = new List<PointLatLng>
        {
            positions[positions.Count - 2], // Önceki nokta
            positions[positions.Count - 1]  // Şu anki nokta
        };

                routesOverlay.Routes.Clear(); // Önceki tüm çizgileri kaldır
                new DrawMode_Straight().Draw(routesOverlay, lastTwoPoints);
            }

            // Alan taraması (3. noktadan itibaren başlar)
            if (positions.Count >= 3)
            {
                new DrawMode_Area().Draw(routesOverlay, positions);
            }

            anasayfaHarita.Refresh();
        }


        private double GetRandomCoordinate(double min, double max)
        {
            return random.NextDouble() * (max - min) + min;
        }



        private bool GetRandomBoolean()
        {
            return random.Next(0, 2) == 1;
        }

        private void lbl_gecenSure_Click(object sender, EventArgs e)
        {

        }

        private void DrawRoute()
        {
            routesOverlay.Routes.Clear(); // Clear previous routes

            GMapRoute route = new GMapRoute(positions, "My Route")
            {
                Stroke = new Pen(System.Drawing.Color.Red, 2) // Set the color and width of the line
            };

            routesOverlay.Routes.Add(route);
            anasayfaHarita.Refresh(); // Refresh the map to show the updated route

            // Optionally, add a marker at each point
            markersOverlay.Markers.Clear();
           
            // Re-add the custom icon marker to keep it on top
            markersOverlay.Markers.Add(gpsMarker);
        }

        

        public void InitializeComponent() 
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.anasayfaHarita = new GMap.NET.WindowsForms.GMapControl();
            this.gboxVeriler = new System.Windows.Forms.GroupBox();
            this.gecenSure = new System.Windows.Forms.Label();
            this.aciTetiklendiMi = new System.Windows.Forms.Label();
            this.lbl_gecenSure = new System.Windows.Forms.Label();
            this.basincTetiklendiMi = new System.Windows.Forms.Label();
            this.chartGeiger = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.label49 = new System.Windows.Forms.Label();
            this.label48 = new System.Windows.Forms.Label();
            this.label47 = new System.Windows.Forms.Label();
            this.lbl_haritaC_boylam = new System.Windows.Forms.Label();
            this.lbl_haritaC_enlem = new System.Windows.Forms.Label();
            this.lbl_takimID_dgr = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.gYukuBoylam = new System.Windows.Forms.Label();
            this.JiroskopZ_dgr = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this.roketBoylam = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.gYukuEnlem = new System.Windows.Forms.Label();
            this.JiroskopY_dgr = new System.Windows.Forms.Label();
            this.roketEnlem = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.gYukuGPSIrtifa = new System.Windows.Forms.Label();
            this.JiroskopX_dgr = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.roketGPSIrtifa = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lbl_bsncIrtıfa_dgr = new System.Windows.Forms.Label();
            this.lbl_bsncIrtifa = new System.Windows.Forms.Label();
            this.lbl_durum = new System.Windows.Forms.Label();
            this.lbl_haritaC_boylam_dgr = new System.Windows.Forms.Label();
            this.lbl_haritaC_enlem_dgr = new System.Windows.Forms.Label();
            this.Basinc_dgr = new System.Windows.Forms.Label();
            this.label42 = new System.Windows.Forms.Label();
            this.nem_dgr = new System.Windows.Forms.Label();
            this.label40 = new System.Windows.Forms.Label();
            this.sicaklik_dgr = new System.Windows.Forms.Label();
            this.label38 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.roketHizY = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lbl_crc = new System.Windows.Forms.Label();
            this.label41 = new System.Windows.Forms.Label();
            this.label39 = new System.Windows.Forms.Label();
            this.lbl_paketSayac_dgr = new System.Windows.Forms.Label();
            this.label37 = new System.Windows.Forms.Label();
            this.label35 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label33 = new System.Windows.Forms.Label();
            this.lbl_paketSayac = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.lbl_takimID = new System.Windows.Forms.Label();
            this.konumaGit_Enlem = new System.Windows.Forms.TextBox();
            this.lbl_gidilecekEnlem = new System.Windows.Forms.Label();
            this.konumaGit_Boylam = new System.Windows.Forms.TextBox();
            this.lbl_gidilecekBoylam = new System.Windows.Forms.Label();
            this.btnKonumaGit = new System.Windows.Forms.Button();
            this.btnSimBaslat = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.gboxVeriler.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartGeiger)).BeginInit();
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
            this.anasayfaHarita.Location = new System.Drawing.Point(529, 0);
            this.anasayfaHarita.Margin = new System.Windows.Forms.Padding(4);
            this.anasayfaHarita.MarkersEnabled = true;
            this.anasayfaHarita.MaxZoom = 20;
            this.anasayfaHarita.MinZoom = 1;
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
            this.anasayfaHarita.Size = new System.Drawing.Size(785, 641);
            this.anasayfaHarita.TabIndex = 0;
            this.anasayfaHarita.Zoom = 15D;
            this.anasayfaHarita.Load += new System.EventHandler(this.anasayfaHarita_Load);
            this.anasayfaHarita.Scroll += new System.Windows.Forms.ScrollEventHandler(this.senkronHaritaCursorPozisyon);
            this.anasayfaHarita.Leave += new System.EventHandler(this.anasayfaHarita_Load);
            this.anasayfaHarita.MouseUp += new System.Windows.Forms.MouseEventHandler(this.anasayfaHarita_MouseUp);
            // 
            // gboxVeriler
            // 
            this.gboxVeriler.Controls.Add(this.gecenSure);
            this.gboxVeriler.Controls.Add(this.aciTetiklendiMi);
            this.gboxVeriler.Controls.Add(this.lbl_gecenSure);
            this.gboxVeriler.Controls.Add(this.basincTetiklendiMi);
            this.gboxVeriler.Controls.Add(this.chartGeiger);
            this.gboxVeriler.Controls.Add(this.label49);
            this.gboxVeriler.Controls.Add(this.label48);
            this.gboxVeriler.Controls.Add(this.label47);
            this.gboxVeriler.Controls.Add(this.lbl_haritaC_boylam);
            this.gboxVeriler.Controls.Add(this.lbl_haritaC_enlem);
            this.gboxVeriler.Controls.Add(this.lbl_takimID_dgr);
            this.gboxVeriler.Controls.Add(this.label32);
            this.gboxVeriler.Controls.Add(this.label30);
            this.gboxVeriler.Controls.Add(this.gYukuBoylam);
            this.gboxVeriler.Controls.Add(this.JiroskopZ_dgr);
            this.gboxVeriler.Controls.Add(this.label31);
            this.gboxVeriler.Controls.Add(this.roketBoylam);
            this.gboxVeriler.Controls.Add(this.label28);
            this.gboxVeriler.Controls.Add(this.label17);
            this.gboxVeriler.Controls.Add(this.label27);
            this.gboxVeriler.Controls.Add(this.label5);
            this.gboxVeriler.Controls.Add(this.label26);
            this.gboxVeriler.Controls.Add(this.gYukuEnlem);
            this.gboxVeriler.Controls.Add(this.JiroskopY_dgr);
            this.gboxVeriler.Controls.Add(this.roketEnlem);
            this.gboxVeriler.Controls.Add(this.label24);
            this.gboxVeriler.Controls.Add(this.label15);
            this.gboxVeriler.Controls.Add(this.label23);
            this.gboxVeriler.Controls.Add(this.label3);
            this.gboxVeriler.Controls.Add(this.label22);
            this.gboxVeriler.Controls.Add(this.gYukuGPSIrtifa);
            this.gboxVeriler.Controls.Add(this.JiroskopX_dgr);
            this.gboxVeriler.Controls.Add(this.label20);
            this.gboxVeriler.Controls.Add(this.label19);
            this.gboxVeriler.Controls.Add(this.label13);
            this.gboxVeriler.Controls.Add(this.roketGPSIrtifa);
            this.gboxVeriler.Controls.Add(this.label1);
            this.gboxVeriler.Controls.Add(this.lbl_bsncIrtıfa_dgr);
            this.gboxVeriler.Controls.Add(this.lbl_bsncIrtifa);
            this.gboxVeriler.Controls.Add(this.lbl_durum);
            this.gboxVeriler.Controls.Add(this.lbl_haritaC_boylam_dgr);
            this.gboxVeriler.Controls.Add(this.lbl_haritaC_enlem_dgr);
            this.gboxVeriler.Controls.Add(this.Basinc_dgr);
            this.gboxVeriler.Controls.Add(this.label42);
            this.gboxVeriler.Controls.Add(this.nem_dgr);
            this.gboxVeriler.Controls.Add(this.label40);
            this.gboxVeriler.Controls.Add(this.sicaklik_dgr);
            this.gboxVeriler.Controls.Add(this.label38);
            this.gboxVeriler.Controls.Add(this.label12);
            this.gboxVeriler.Controls.Add(this.roketHizY);
            this.gboxVeriler.Controls.Add(this.label8);
            this.gboxVeriler.Controls.Add(this.lbl_crc);
            this.gboxVeriler.Controls.Add(this.label41);
            this.gboxVeriler.Controls.Add(this.label39);
            this.gboxVeriler.Controls.Add(this.lbl_paketSayac_dgr);
            this.gboxVeriler.Controls.Add(this.label37);
            this.gboxVeriler.Controls.Add(this.label35);
            this.gboxVeriler.Controls.Add(this.label11);
            this.gboxVeriler.Controls.Add(this.label9);
            this.gboxVeriler.Controls.Add(this.label7);
            this.gboxVeriler.Controls.Add(this.label33);
            this.gboxVeriler.Controls.Add(this.lbl_paketSayac);
            this.gboxVeriler.Controls.Add(this.textBox1);
            this.gboxVeriler.Controls.Add(this.lbl_takimID);
            this.gboxVeriler.Location = new System.Drawing.Point(5, 5);
            this.gboxVeriler.Margin = new System.Windows.Forms.Padding(4);
            this.gboxVeriler.Name = "gboxVeriler";
            this.gboxVeriler.Padding = new System.Windows.Forms.Padding(4);
            this.gboxVeriler.Size = new System.Drawing.Size(516, 741);
            this.gboxVeriler.TabIndex = 1;
            this.gboxVeriler.TabStop = false;
            this.gboxVeriler.Text = "Veriler";
            this.gboxVeriler.Enter += new System.EventHandler(this.gboxVeriler_Enter);
            // 
            // gecenSure
            // 
            this.gecenSure.AutoSize = true;
            this.gecenSure.Location = new System.Drawing.Point(396, 480);
            this.gecenSure.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.gecenSure.Name = "gecenSure";
            this.gecenSure.Size = new System.Drawing.Size(14, 16);
            this.gecenSure.TabIndex = 7;
            this.gecenSure.Text = "0";
            // 
            // aciTetiklendiMi
            // 
            this.aciTetiklendiMi.AutoSize = true;
            this.aciTetiklendiMi.Location = new System.Drawing.Point(248, 454);
            this.aciTetiklendiMi.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.aciTetiklendiMi.Name = "aciTetiklendiMi";
            this.aciTetiklendiMi.Size = new System.Drawing.Size(172, 16);
            this.aciTetiklendiMi.TabIndex = 13;
            this.aciTetiklendiMi.Text = "Açı İle Ayrılma Tetiklenmedi";
            this.aciTetiklendiMi.Click += new System.EventHandler(this.aciTetiklendiMİ_Click);
            // 
            // lbl_gecenSure
            // 
            this.lbl_gecenSure.AutoSize = true;
            this.lbl_gecenSure.Location = new System.Drawing.Point(248, 480);
            this.lbl_gecenSure.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_gecenSure.Name = "lbl_gecenSure";
            this.lbl_gecenSure.Size = new System.Drawing.Size(132, 16);
            this.lbl_gecenSure.TabIndex = 6;
            this.lbl_gecenSure.Text = "Geçen Süre (saniye):";
            this.lbl_gecenSure.Click += new System.EventHandler(this.lbl_gecenSure_Click);
            // 
            // basincTetiklendiMi
            // 
            this.basincTetiklendiMi.AutoSize = true;
            this.basincTetiklendiMi.Location = new System.Drawing.Point(248, 428);
            this.basincTetiklendiMi.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.basincTetiklendiMi.Name = "basincTetiklendiMi";
            this.basincTetiklendiMi.Size = new System.Drawing.Size(194, 16);
            this.basincTetiklendiMi.TabIndex = 12;
            this.basincTetiklendiMi.Text = "Basınç İle Ayrılma Tetiklenmedi";
            // 
            // chartGeiger
            // 
            this.chartGeiger.BackColor = System.Drawing.Color.WhiteSmoke;
            chartArea1.Name = "ChartArea1";
            this.chartGeiger.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chartGeiger.Legends.Add(legend1);
            this.chartGeiger.Location = new System.Drawing.Point(17, 539);
            this.chartGeiger.Margin = new System.Windows.Forms.Padding(4);
            this.chartGeiger.Name = "chartGeiger";
            this.chartGeiger.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.SeaGreen;
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Rad";
            this.chartGeiger.Series.Add(series1);
            this.chartGeiger.Size = new System.Drawing.Size(384, 194);
            this.chartGeiger.TabIndex = 11;
            this.chartGeiger.Text = "Geiger Verileri";
            // 
            // label49
            // 
            this.label49.AutoSize = true;
            this.label49.Location = new System.Drawing.Point(248, 247);
            this.label49.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label49.Name = "label49";
            this.label49.Size = new System.Drawing.Size(109, 16);
            this.label49.TabIndex = 10;
            this.label49.Text = "Basınç (BME280)";
            // 
            // label48
            // 
            this.label48.AutoSize = true;
            this.label48.Location = new System.Drawing.Point(248, 273);
            this.label48.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label48.Name = "label48";
            this.label48.Size = new System.Drawing.Size(36, 16);
            this.label48.TabIndex = 9;
            this.label48.Text = "Nem";
            // 
            // label47
            // 
            this.label47.AutoSize = true;
            this.label47.Location = new System.Drawing.Point(248, 299);
            this.label47.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label47.Name = "label47";
            this.label47.Size = new System.Drawing.Size(54, 16);
            this.label47.TabIndex = 8;
            this.label47.Text = "Sıcaklık";
            // 
            // lbl_haritaC_boylam
            // 
            this.lbl_haritaC_boylam.AutoSize = true;
            this.lbl_haritaC_boylam.Location = new System.Drawing.Point(248, 384);
            this.lbl_haritaC_boylam.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_haritaC_boylam.Name = "lbl_haritaC_boylam";
            this.lbl_haritaC_boylam.Size = new System.Drawing.Size(107, 16);
            this.lbl_haritaC_boylam.TabIndex = 7;
            this.lbl_haritaC_boylam.Text = "Harita C. Boylam";
            // 
            // lbl_haritaC_enlem
            // 
            this.lbl_haritaC_enlem.AutoSize = true;
            this.lbl_haritaC_enlem.Location = new System.Drawing.Point(248, 358);
            this.lbl_haritaC_enlem.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_haritaC_enlem.Name = "lbl_haritaC_enlem";
            this.lbl_haritaC_enlem.Size = new System.Drawing.Size(99, 16);
            this.lbl_haritaC_enlem.TabIndex = 6;
            this.lbl_haritaC_enlem.Text = "Harita C. Enlem";
            // 
            // lbl_takimID_dgr
            // 
            this.lbl_takimID_dgr.AutoSize = true;
            this.lbl_takimID_dgr.Location = new System.Drawing.Point(187, 33);
            this.lbl_takimID_dgr.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_takimID_dgr.Name = "lbl_takimID_dgr";
            this.lbl_takimID_dgr.Size = new System.Drawing.Size(11, 16);
            this.lbl_takimID_dgr.TabIndex = 5;
            this.lbl_takimID_dgr.Text = "-";
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Location = new System.Drawing.Point(187, 351);
            this.label32.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(14, 16);
            this.label32.TabIndex = 3;
            this.label32.Text = "0";
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(187, 428);
            this.label30.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(14, 16);
            this.label30.TabIndex = 3;
            this.label30.Text = "0";
            // 
            // gYukuBoylam
            // 
            this.gYukuBoylam.AutoSize = true;
            this.gYukuBoylam.Location = new System.Drawing.Point(187, 299);
            this.gYukuBoylam.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.gYukuBoylam.Name = "gYukuBoylam";
            this.gYukuBoylam.Size = new System.Drawing.Size(14, 16);
            this.gYukuBoylam.TabIndex = 3;
            this.gYukuBoylam.Text = "0";
            // 
            // JiroskopZ_dgr
            // 
            this.JiroskopZ_dgr.AutoSize = true;
            this.JiroskopZ_dgr.Location = new System.Drawing.Point(187, 506);
            this.JiroskopZ_dgr.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.JiroskopZ_dgr.Name = "JiroskopZ_dgr";
            this.JiroskopZ_dgr.Size = new System.Drawing.Size(14, 16);
            this.JiroskopZ_dgr.TabIndex = 3;
            this.JiroskopZ_dgr.Text = "0";
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(13, 351);
            this.label31.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(26, 16);
            this.label31.TabIndex = 2;
            this.label31.Text = "Açı";
            // 
            // roketBoylam
            // 
            this.roketBoylam.AutoSize = true;
            this.roketBoylam.Location = new System.Drawing.Point(187, 198);
            this.roketBoylam.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.roketBoylam.Name = "roketBoylam";
            this.roketBoylam.Size = new System.Drawing.Size(14, 16);
            this.roketBoylam.TabIndex = 3;
            this.roketBoylam.Text = "0";
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(13, 428);
            this.label28.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(47, 16);
            this.label28.TabIndex = 2;
            this.label28.Text = "İvme Z";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(13, 299);
            this.label17.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(102, 16);
            this.label17.TabIndex = 2;
            this.label17.Text = "G. Yükü Boylam";
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(13, 506);
            this.label27.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(70, 16);
            this.label27.TabIndex = 2;
            this.label27.Text = "Jiroskop Z";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 198);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(92, 16);
            this.label5.TabIndex = 2;
            this.label5.Text = "Roket Boylam";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(187, 402);
            this.label26.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(14, 16);
            this.label26.TabIndex = 3;
            this.label26.Text = "0";
            // 
            // gYukuEnlem
            // 
            this.gYukuEnlem.AutoSize = true;
            this.gYukuEnlem.Location = new System.Drawing.Point(187, 273);
            this.gYukuEnlem.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.gYukuEnlem.Name = "gYukuEnlem";
            this.gYukuEnlem.Size = new System.Drawing.Size(14, 16);
            this.gYukuEnlem.TabIndex = 3;
            this.gYukuEnlem.Text = "0";
            // 
            // JiroskopY_dgr
            // 
            this.JiroskopY_dgr.AutoSize = true;
            this.JiroskopY_dgr.Location = new System.Drawing.Point(187, 480);
            this.JiroskopY_dgr.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.JiroskopY_dgr.Name = "JiroskopY_dgr";
            this.JiroskopY_dgr.Size = new System.Drawing.Size(14, 16);
            this.JiroskopY_dgr.TabIndex = 3;
            this.JiroskopY_dgr.Text = "0";
            // 
            // roketEnlem
            // 
            this.roketEnlem.AutoSize = true;
            this.roketEnlem.Location = new System.Drawing.Point(187, 175);
            this.roketEnlem.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.roketEnlem.Name = "roketEnlem";
            this.roketEnlem.Size = new System.Drawing.Size(14, 16);
            this.roketEnlem.TabIndex = 3;
            this.roketEnlem.Text = "0";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(13, 402);
            this.label24.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(48, 16);
            this.label24.TabIndex = 2;
            this.label24.Text = "İvme Y";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(13, 273);
            this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(94, 16);
            this.label15.TabIndex = 2;
            this.label15.Text = "G. Yükü Enlem";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(13, 480);
            this.label23.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(71, 16);
            this.label23.TabIndex = 2;
            this.label23.Text = "Jiroskop Y";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 175);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 16);
            this.label3.TabIndex = 2;
            this.label3.Text = "Roket Enlem";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(187, 377);
            this.label22.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(14, 16);
            this.label22.TabIndex = 3;
            this.label22.Text = "0";
            // 
            // gYukuGPSIrtifa
            // 
            this.gYukuGPSIrtifa.AutoSize = true;
            this.gYukuGPSIrtifa.Location = new System.Drawing.Point(187, 247);
            this.gYukuGPSIrtifa.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.gYukuGPSIrtifa.Name = "gYukuGPSIrtifa";
            this.gYukuGPSIrtifa.Size = new System.Drawing.Size(14, 16);
            this.gYukuGPSIrtifa.TabIndex = 3;
            this.gYukuGPSIrtifa.Text = "0";
            // 
            // JiroskopX_dgr
            // 
            this.JiroskopX_dgr.AutoSize = true;
            this.JiroskopX_dgr.Location = new System.Drawing.Point(187, 454);
            this.JiroskopX_dgr.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.JiroskopX_dgr.Name = "JiroskopX_dgr";
            this.JiroskopX_dgr.Size = new System.Drawing.Size(14, 16);
            this.JiroskopX_dgr.TabIndex = 3;
            this.JiroskopX_dgr.Text = "0";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(13, 377);
            this.label20.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(47, 16);
            this.label20.TabIndex = 2;
            this.label20.Text = "İvme X";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(13, 454);
            this.label19.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(70, 16);
            this.label19.TabIndex = 2;
            this.label19.Text = "Jiroskop X";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(13, 247);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(119, 16);
            this.label13.TabIndex = 2;
            this.label13.Text = "G. Yükü İrtifa (GPS)";
            // 
            // roketGPSIrtifa
            // 
            this.roketGPSIrtifa.AutoSize = true;
            this.roketGPSIrtifa.Location = new System.Drawing.Point(187, 151);
            this.roketGPSIrtifa.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.roketGPSIrtifa.Name = "roketGPSIrtifa";
            this.roketGPSIrtifa.Size = new System.Drawing.Size(14, 16);
            this.roketGPSIrtifa.TabIndex = 3;
            this.roketGPSIrtifa.Text = "0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 151);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(109, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "Roket İrtifa (GPS)";
            // 
            // lbl_bsncIrtıfa_dgr
            // 
            this.lbl_bsncIrtıfa_dgr.AutoSize = true;
            this.lbl_bsncIrtıfa_dgr.Location = new System.Drawing.Point(187, 128);
            this.lbl_bsncIrtıfa_dgr.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_bsncIrtıfa_dgr.Name = "lbl_bsncIrtıfa_dgr";
            this.lbl_bsncIrtıfa_dgr.Size = new System.Drawing.Size(14, 16);
            this.lbl_bsncIrtıfa_dgr.TabIndex = 3;
            this.lbl_bsncIrtıfa_dgr.Text = "0";
            // 
            // lbl_bsncIrtifa
            // 
            this.lbl_bsncIrtifa.AutoSize = true;
            this.lbl_bsncIrtifa.Location = new System.Drawing.Point(13, 128);
            this.lbl_bsncIrtifa.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_bsncIrtifa.Name = "lbl_bsncIrtifa";
            this.lbl_bsncIrtifa.Size = new System.Drawing.Size(122, 16);
            this.lbl_bsncIrtifa.TabIndex = 2;
            this.lbl_bsncIrtifa.Text = "Roket İrtifa (Basınç)";
            // 
            // lbl_durum
            // 
            this.lbl_durum.AutoSize = true;
            this.lbl_durum.Location = new System.Drawing.Point(187, 105);
            this.lbl_durum.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_durum.Name = "lbl_durum";
            this.lbl_durum.Size = new System.Drawing.Size(14, 16);
            this.lbl_durum.TabIndex = 3;
            this.lbl_durum.Text = "0";
            // 
            // lbl_haritaC_boylam_dgr
            // 
            this.lbl_haritaC_boylam_dgr.AutoSize = true;
            this.lbl_haritaC_boylam_dgr.Location = new System.Drawing.Point(420, 384);
            this.lbl_haritaC_boylam_dgr.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_haritaC_boylam_dgr.Name = "lbl_haritaC_boylam_dgr";
            this.lbl_haritaC_boylam_dgr.Size = new System.Drawing.Size(14, 16);
            this.lbl_haritaC_boylam_dgr.TabIndex = 3;
            this.lbl_haritaC_boylam_dgr.Text = "0";
            this.lbl_haritaC_boylam_dgr.Click += new System.EventHandler(this.lbl_haritaC_boylam_dgr_Click);
            // 
            // lbl_haritaC_enlem_dgr
            // 
            this.lbl_haritaC_enlem_dgr.AutoSize = true;
            this.lbl_haritaC_enlem_dgr.Location = new System.Drawing.Point(420, 361);
            this.lbl_haritaC_enlem_dgr.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_haritaC_enlem_dgr.Name = "lbl_haritaC_enlem_dgr";
            this.lbl_haritaC_enlem_dgr.Size = new System.Drawing.Size(14, 16);
            this.lbl_haritaC_enlem_dgr.TabIndex = 3;
            this.lbl_haritaC_enlem_dgr.Text = "0";
            // 
            // Basinc_dgr
            // 
            this.Basinc_dgr.AutoSize = true;
            this.Basinc_dgr.Location = new System.Drawing.Point(420, 247);
            this.Basinc_dgr.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Basinc_dgr.Name = "Basinc_dgr";
            this.Basinc_dgr.Size = new System.Drawing.Size(14, 16);
            this.Basinc_dgr.TabIndex = 3;
            this.Basinc_dgr.Text = "0";
            // 
            // label42
            // 
            this.label42.AutoSize = true;
            this.label42.Location = new System.Drawing.Point(420, 175);
            this.label42.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(14, 16);
            this.label42.TabIndex = 3;
            this.label42.Text = "0";
            // 
            // nem_dgr
            // 
            this.nem_dgr.AutoSize = true;
            this.nem_dgr.Location = new System.Drawing.Point(420, 273);
            this.nem_dgr.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.nem_dgr.Name = "nem_dgr";
            this.nem_dgr.Size = new System.Drawing.Size(14, 16);
            this.nem_dgr.TabIndex = 3;
            this.nem_dgr.Text = "0";
            // 
            // label40
            // 
            this.label40.AutoSize = true;
            this.label40.Location = new System.Drawing.Point(420, 151);
            this.label40.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(14, 16);
            this.label40.TabIndex = 3;
            this.label40.Text = "0";
            // 
            // sicaklik_dgr
            // 
            this.sicaklik_dgr.AutoSize = true;
            this.sicaklik_dgr.Location = new System.Drawing.Point(420, 299);
            this.sicaklik_dgr.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.sicaklik_dgr.Name = "sicaklik_dgr";
            this.sicaklik_dgr.Size = new System.Drawing.Size(14, 16);
            this.sicaklik_dgr.TabIndex = 3;
            this.sicaklik_dgr.Text = "0";
            // 
            // label38
            // 
            this.label38.AutoSize = true;
            this.label38.Location = new System.Drawing.Point(420, 128);
            this.label38.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(14, 16);
            this.label38.TabIndex = 3;
            this.label38.Text = "0";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(420, 105);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(14, 16);
            this.label12.TabIndex = 3;
            this.label12.Text = "0";
            // 
            // roketHizY
            // 
            this.roketHizY.AutoSize = true;
            this.roketHizY.Location = new System.Drawing.Point(420, 81);
            this.roketHizY.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.roketHizY.Name = "roketHizY";
            this.roketHizY.Size = new System.Drawing.Size(14, 16);
            this.roketHizY.TabIndex = 3;
            this.roketHizY.Text = "0";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(420, 58);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(14, 16);
            this.label8.TabIndex = 3;
            this.label8.Text = "0";
            // 
            // lbl_crc
            // 
            this.lbl_crc.AutoSize = true;
            this.lbl_crc.Location = new System.Drawing.Point(187, 58);
            this.lbl_crc.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_crc.Name = "lbl_crc";
            this.lbl_crc.Size = new System.Drawing.Size(14, 16);
            this.lbl_crc.TabIndex = 3;
            this.lbl_crc.Text = "0";
            // 
            // label41
            // 
            this.label41.AutoSize = true;
            this.label41.Location = new System.Drawing.Point(248, 175);
            this.label41.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(77, 16);
            this.label41.TabIndex = 2;
            this.label41.Text = "Açısal Hız Z";
            // 
            // label39
            // 
            this.label39.AutoSize = true;
            this.label39.Location = new System.Drawing.Point(248, 151);
            this.label39.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(78, 16);
            this.label39.TabIndex = 2;
            this.label39.Text = "Açısal Hız Y";
            this.label39.Click += new System.EventHandler(this.label39_Click);
            // 
            // lbl_paketSayac_dgr
            // 
            this.lbl_paketSayac_dgr.AutoSize = true;
            this.lbl_paketSayac_dgr.Location = new System.Drawing.Point(187, 81);
            this.lbl_paketSayac_dgr.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_paketSayac_dgr.Name = "lbl_paketSayac_dgr";
            this.lbl_paketSayac_dgr.Size = new System.Drawing.Size(14, 16);
            this.lbl_paketSayac_dgr.TabIndex = 3;
            this.lbl_paketSayac_dgr.Text = "0";
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.Location = new System.Drawing.Point(248, 128);
            this.label37.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(77, 16);
            this.label37.TabIndex = 2;
            this.label37.Text = "Açısal Hız X";
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Location = new System.Drawing.Point(13, 105);
            this.label35.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(46, 16);
            this.label35.TabIndex = 2;
            this.label35.Text = "Durum";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(248, 105);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(76, 16);
            this.label11.TabIndex = 2;
            this.label11.Text = "Roket Hız Z";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(248, 81);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(77, 16);
            this.label9.TabIndex = 2;
            this.label9.Text = "Roket Hız Y";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(248, 58);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(76, 16);
            this.label7.TabIndex = 2;
            this.label7.Text = "Roket Hız X";
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Location = new System.Drawing.Point(13, 58);
            this.label33.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(35, 16);
            this.label33.TabIndex = 2;
            this.label33.Text = "CRC";
            // 
            // lbl_paketSayac
            // 
            this.lbl_paketSayac.AutoSize = true;
            this.lbl_paketSayac.Location = new System.Drawing.Point(13, 81);
            this.lbl_paketSayac.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_paketSayac.Name = "lbl_paketSayac";
            this.lbl_paketSayac.Size = new System.Drawing.Size(87, 16);
            this.lbl_paketSayac.TabIndex = 2;
            this.lbl_paketSayac.Text = "Paket Sayacı";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(109, 30);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(41, 22);
            this.textBox1.TabIndex = 1;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // lbl_takimID
            // 
            this.lbl_takimID.AutoSize = true;
            this.lbl_takimID.Location = new System.Drawing.Point(13, 33);
            this.lbl_takimID.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_takimID.Name = "lbl_takimID";
            this.lbl_takimID.Size = new System.Drawing.Size(61, 16);
            this.lbl_takimID.TabIndex = 0;
            this.lbl_takimID.Text = "Takım ID";
            // 
            // konumaGit_Enlem
            // 
            this.konumaGit_Enlem.Location = new System.Drawing.Point(529, 670);
            this.konumaGit_Enlem.Margin = new System.Windows.Forms.Padding(4);
            this.konumaGit_Enlem.Name = "konumaGit_Enlem";
            this.konumaGit_Enlem.Size = new System.Drawing.Size(132, 22);
            this.konumaGit_Enlem.TabIndex = 2;
            // 
            // lbl_gidilecekEnlem
            // 
            this.lbl_gidilecekEnlem.AutoSize = true;
            this.lbl_gidilecekEnlem.Location = new System.Drawing.Point(541, 650);
            this.lbl_gidilecekEnlem.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_gidilecekEnlem.Name = "lbl_gidilecekEnlem";
            this.lbl_gidilecekEnlem.Size = new System.Drawing.Size(105, 16);
            this.lbl_gidilecekEnlem.TabIndex = 3;
            this.lbl_gidilecekEnlem.Text = "Gidilecek Enlem";
            // 
            // konumaGit_Boylam
            // 
            this.konumaGit_Boylam.Location = new System.Drawing.Point(692, 670);
            this.konumaGit_Boylam.Margin = new System.Windows.Forms.Padding(4);
            this.konumaGit_Boylam.Name = "konumaGit_Boylam";
            this.konumaGit_Boylam.Size = new System.Drawing.Size(132, 22);
            this.konumaGit_Boylam.TabIndex = 2;
            // 
            // lbl_gidilecekBoylam
            // 
            this.lbl_gidilecekBoylam.AutoSize = true;
            this.lbl_gidilecekBoylam.Location = new System.Drawing.Point(700, 650);
            this.lbl_gidilecekBoylam.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_gidilecekBoylam.Name = "lbl_gidilecekBoylam";
            this.lbl_gidilecekBoylam.Size = new System.Drawing.Size(113, 16);
            this.lbl_gidilecekBoylam.TabIndex = 3;
            this.lbl_gidilecekBoylam.Text = "Gidilecek Boylam";
            // 
            // btnKonumaGit
            // 
            this.btnKonumaGit.Location = new System.Drawing.Point(855, 668);
            this.btnKonumaGit.Margin = new System.Windows.Forms.Padding(4);
            this.btnKonumaGit.Name = "btnKonumaGit";
            this.btnKonumaGit.Size = new System.Drawing.Size(121, 25);
            this.btnKonumaGit.TabIndex = 4;
            this.btnKonumaGit.Text = "KONUMA GİT";
            this.btnKonumaGit.UseVisualStyleBackColor = true;
            this.btnKonumaGit.Click += new System.EventHandler(this.btnKonumaGit_Click);
            // 
            // btnSimBaslat
            // 
            this.btnSimBaslat.Location = new System.Drawing.Point(1031, 668);
            this.btnSimBaslat.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSimBaslat.Name = "btnSimBaslat";
            this.btnSimBaslat.Size = new System.Drawing.Size(151, 25);
            this.btnSimBaslat.TabIndex = 5;
            this.btnSimBaslat.Text = "Simülasyonu Başlat";
            this.btnSimBaslat.UseVisualStyleBackColor = true;
            this.btnSimBaslat.Click += new System.EventHandler(this.btnSimBaslat_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(1031, 697);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(151, 25);
            this.button1.TabIndex = 5;
            this.button1.Text = "Simülasyonu Sıfırla";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // uc_AnaSayfa
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnSimBaslat);
            this.Controls.Add(this.btnKonumaGit);
            this.Controls.Add(this.lbl_gidilecekBoylam);
            this.Controls.Add(this.lbl_gidilecekEnlem);
            this.Controls.Add(this.konumaGit_Boylam);
            this.Controls.Add(this.konumaGit_Enlem);
            this.Controls.Add(this.gboxVeriler);
            this.Controls.Add(this.anasayfaHarita);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "uc_AnaSayfa";
            this.Size = new System.Drawing.Size(1315, 750);
            this.Load += new System.EventHandler(this.uc_AnaSayfa_Load);
            this.gboxVeriler.ResumeLayout(false);
            this.gboxVeriler.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartGeiger)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void label39_Click(object sender, EventArgs e)
        {

        }

        private void gboxVeriler_Enter(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            markersOverlay.Markers.Clear();
            
            


            routesOverlay.Routes.Clear();
            positions.Clear();
            anasayfaHarita.Refresh();
            ucusGecenZaman = 0;
            isUcusTimerRunning = false;
            StopUcusTimer();
        }
    }
}
