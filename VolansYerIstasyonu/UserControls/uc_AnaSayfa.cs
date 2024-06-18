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



namespace VolansYerIstasyonu.UserControls

{

    
    public partial class uc_AnaSayfa : UserControl
    {

        private Timer timer;
        private GMapOverlay overlay;
        private GMapOverlay routesOverlay;
        private Random random = new Random();
        private GMapRoute routeAna;
        private GMapRoute routeYedek;
        private GMapRoute routeGorev;
        private bool isSeparated = false;
        private List<PointLatLng> anaAviyonikCoordinates = new List<PointLatLng>();
        private List<PointLatLng> gorevYukuCoordinates = new List<PointLatLng>();
        public static Timer ucusTimer;
        public static double ucusGecenZaman = 0;
        private bool isUcusTimerRunning = false;
        public uc_AnaSayfa()
        {
            InitializeComponent();
            InitializeMap();
            //uc_PortAyarlari.loraSerialPort.DataReceived += loraSerialPort_DataReceived; //subscription test

            anasayfaHarita.MapProvider = GMapProviders.GoogleMap;
            GMaps.Instance.Mode = AccessMode.ServerOnly;
            anasayfaHarita.Position = new PointLatLng(40.7158587326396, 31.5250968933105);
            anasayfaHarita.MinZoom = 1;
            anasayfaHarita.MaxZoom = 24;
            anasayfaHarita.Zoom = 18;
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



        private void OnUcusTimerTick(object sender, EventArgs e)
        {
            ucusGecenZaman += 0.2;
            gecenSure.Text = $"{ucusGecenZaman}";
            
            TeknofestVeriler.Veriler.RoketGpsIrtifa = (float)(UcusSim.yukseklikHesap(ucusGecenZaman));
            UcusSim.enlemHesapla(ucusGecenZaman);
            UcusSim.boylamHesapla(ucusGecenZaman);
            roketGPSIrtifa.Text = $"{UcusSim.yukseklikHesap(ucusGecenZaman)}";
            roketEnlem.Text = $"{TeknofestVeriler.Veriler.RoketEnlem}";
            roketBoylam.Text = $"{TeknofestVeriler.Veriler.RoketBoylam}";
            UcusSim.gorevYukuEnlemHesapla(ucusGecenZaman);
            UcusSim.gorevYukuBoylamHesapla(ucusGecenZaman);
            gYukuEnlem.Text = $"{TeknofestVeriler.Veriler.GorevYukuEnlem}";
            gYukuBoylam.Text = $"{TeknofestVeriler.Veriler.GorevYukuBoylam}";
            roketHizY.Text = $"{UcusSim.hizHesap(ucusGecenZaman)}";


            //Database.addDataGorevYuku();
            //Database.addDataRoketBilgileri();
            //Database.addDataSensorVerileri();

            //ivmeXAnaSayfa.Text = $"{UcusSim.hizHesap(ucusGecenZaman)}";
            //ivmeYAnaSayfa.Text = $"{UcusSim.anlikYukseklikHesapla(ucusGecenZaman)}";
            //gpsirtifaAnaSayfa.Text = $"{Veriler.roketGpsIrtifa}";

            double newLatitude = TeknofestVeriler.Veriler.RoketEnlem;
            double newLongitude = TeknofestVeriler.Veriler.RoketBoylam;
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
            anasayfaHarita.Position = new GMap.NET.PointLatLng(40.7350, 31.6060); // Bolu, Türkiye
            anasayfaHarita.MinZoom = 2;
            anasayfaHarita.MaxZoom = 18;
            anasayfaHarita.Zoom = 5;


            overlay = new GMapOverlay("markers");
            routesOverlay = new GMapOverlay("routes");
            anasayfaHarita.Overlays.Add(overlay);
            anasayfaHarita.Overlays.Add(routesOverlay);
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



        private void UpdateMap(double ana_av_x, double ana_av_y, double yedek_av_x, double yedek_av_y, double gorev_yuku_x, double gorev_yuku_y, int durum)
        {
            overlay.Markers.Clear();
            routesOverlay.Routes.Clear();

            var anaAvCoordinate = new PointLatLng(ana_av_x, ana_av_y);
            var yedekAvCoordinate = new PointLatLng(yedek_av_x, yedek_av_y);
            var gorevYukuCoordinate = new PointLatLng(gorev_yuku_x, gorev_yuku_y);

            var anaAvMarker = new GMarkerGoogle(anaAvCoordinate, GMarkerGoogleType.red);
            var yedekAvMarker = new GMarkerGoogle(yedekAvCoordinate, GMarkerGoogleType.blue);
            var gorevYukuMarker = new GMarkerGoogle(gorevYukuCoordinate, GMarkerGoogleType.green);

            overlay.Markers.Add(anaAvMarker);
            overlay.Markers.Add(yedekAvMarker);
            overlay.Markers.Add(gorevYukuMarker);

            if (durum == 0)
            {

                anaAviyonikCoordinates.Add(anaAvCoordinate);


                if (anaAviyonikCoordinates.Count > 1)
                {
                    var anaAvRoute = new GMapRoute(anaAviyonikCoordinates, "Ana Aviyonik Route");
                    anaAvRoute.Stroke = new System.Drawing.Pen(System.Drawing.Color.Red, 2);
                    routesOverlay.Routes.Add(anaAvRoute);
                }

                //focus
                anasayfaHarita.Position = anaAvCoordinate;
                anasayfaHarita.Zoom = 8;
            }
            else if (durum == 1)
            {

                gorevYukuCoordinates.Add(gorevYukuCoordinate);


                if (gorevYukuCoordinates.Count > 1)
                {
                    var gorevYukuRoute = new GMapRoute(gorevYukuCoordinates, "Görev Yükü Route");
                    gorevYukuRoute.Stroke = new System.Drawing.Pen(System.Drawing.Color.Green, 2);
                    routesOverlay.Routes.Add(gorevYukuRoute);
                }


                anasayfaHarita.Position = gorevYukuCoordinate;
                anasayfaHarita.Zoom = 8;
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





        public void InitializeComponent() 
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.anasayfaHarita = new GMap.NET.WindowsForms.GMapControl();
            this.gboxVeriler = new System.Windows.Forms.GroupBox();
            this.aciTetiklendiMİ = new System.Windows.Forms.Label();
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
            this.JiroskopZ = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this.roketBoylam = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.gYukuEnlem = new System.Windows.Forms.Label();
            this.JiroskopY = new System.Windows.Forms.Label();
            this.roketEnlem = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.gYukuGPSIrtifa = new System.Windows.Forms.Label();
            this.JiroskopX = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.roketGPSIrtifa = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lbl_bsncIrtıfa_dgr = new System.Windows.Forms.Label();
            this.lbl_bsncIrtifa = new System.Windows.Forms.Label();
            this.label36 = new System.Windows.Forms.Label();
            this.lbl_haritaC_boylam_dgr = new System.Windows.Forms.Label();
            this.lbl_haritaC_enlem_dgr = new System.Windows.Forms.Label();
            this.Basinc = new System.Windows.Forms.Label();
            this.label42 = new System.Windows.Forms.Label();
            this.label51 = new System.Windows.Forms.Label();
            this.label40 = new System.Windows.Forms.Label();
            this.label50 = new System.Windows.Forms.Label();
            this.label38 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.roketHizY = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label34 = new System.Windows.Forms.Label();
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
            this.lbl_gecenSure = new System.Windows.Forms.Label();
            this.gecenSure = new System.Windows.Forms.Label();
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
            this.anasayfaHarita.Location = new System.Drawing.Point(397, 0);
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
            this.anasayfaHarita.Size = new System.Drawing.Size(589, 521);
            this.anasayfaHarita.TabIndex = 0;
            this.anasayfaHarita.Zoom = 0D;
            this.anasayfaHarita.Load += new System.EventHandler(this.anasayfaHarita_Load);
            this.anasayfaHarita.Scroll += new System.Windows.Forms.ScrollEventHandler(this.senkronHaritaCursorPozisyon);
            this.anasayfaHarita.Leave += new System.EventHandler(this.anasayfaHarita_Load);
            this.anasayfaHarita.MouseUp += new System.Windows.Forms.MouseEventHandler(this.anasayfaHarita_MouseUp);
            // 
            // gboxVeriler
            // 
            this.gboxVeriler.Controls.Add(this.gecenSure);
            this.gboxVeriler.Controls.Add(this.aciTetiklendiMİ);
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
            this.gboxVeriler.Controls.Add(this.JiroskopZ);
            this.gboxVeriler.Controls.Add(this.label31);
            this.gboxVeriler.Controls.Add(this.roketBoylam);
            this.gboxVeriler.Controls.Add(this.label28);
            this.gboxVeriler.Controls.Add(this.label17);
            this.gboxVeriler.Controls.Add(this.label27);
            this.gboxVeriler.Controls.Add(this.label5);
            this.gboxVeriler.Controls.Add(this.label26);
            this.gboxVeriler.Controls.Add(this.gYukuEnlem);
            this.gboxVeriler.Controls.Add(this.JiroskopY);
            this.gboxVeriler.Controls.Add(this.roketEnlem);
            this.gboxVeriler.Controls.Add(this.label24);
            this.gboxVeriler.Controls.Add(this.label15);
            this.gboxVeriler.Controls.Add(this.label23);
            this.gboxVeriler.Controls.Add(this.label3);
            this.gboxVeriler.Controls.Add(this.label22);
            this.gboxVeriler.Controls.Add(this.gYukuGPSIrtifa);
            this.gboxVeriler.Controls.Add(this.JiroskopX);
            this.gboxVeriler.Controls.Add(this.label20);
            this.gboxVeriler.Controls.Add(this.label19);
            this.gboxVeriler.Controls.Add(this.label13);
            this.gboxVeriler.Controls.Add(this.roketGPSIrtifa);
            this.gboxVeriler.Controls.Add(this.label1);
            this.gboxVeriler.Controls.Add(this.lbl_bsncIrtıfa_dgr);
            this.gboxVeriler.Controls.Add(this.lbl_bsncIrtifa);
            this.gboxVeriler.Controls.Add(this.label36);
            this.gboxVeriler.Controls.Add(this.lbl_haritaC_boylam_dgr);
            this.gboxVeriler.Controls.Add(this.lbl_haritaC_enlem_dgr);
            this.gboxVeriler.Controls.Add(this.Basinc);
            this.gboxVeriler.Controls.Add(this.label42);
            this.gboxVeriler.Controls.Add(this.label51);
            this.gboxVeriler.Controls.Add(this.label40);
            this.gboxVeriler.Controls.Add(this.label50);
            this.gboxVeriler.Controls.Add(this.label38);
            this.gboxVeriler.Controls.Add(this.label12);
            this.gboxVeriler.Controls.Add(this.roketHizY);
            this.gboxVeriler.Controls.Add(this.label8);
            this.gboxVeriler.Controls.Add(this.label34);
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
            this.gboxVeriler.Location = new System.Drawing.Point(4, 4);
            this.gboxVeriler.Name = "gboxVeriler";
            this.gboxVeriler.Size = new System.Drawing.Size(387, 602);
            this.gboxVeriler.TabIndex = 1;
            this.gboxVeriler.TabStop = false;
            this.gboxVeriler.Text = "Veriler";
            // 
            // aciTetiklendiMİ
            // 
            this.aciTetiklendiMİ.AutoSize = true;
            this.aciTetiklendiMİ.Location = new System.Drawing.Point(186, 369);
            this.aciTetiklendiMİ.Name = "aciTetiklendiMİ";
            this.aciTetiklendiMİ.Size = new System.Drawing.Size(135, 13);
            this.aciTetiklendiMİ.TabIndex = 13;
            this.aciTetiklendiMİ.Text = "Açı İle Ayrılma Tetiklenmedi";
            this.aciTetiklendiMİ.Click += new System.EventHandler(this.aciTetiklendiMİ_Click);
            // 
            // basincTetiklendiMi
            // 
            this.basincTetiklendiMi.AutoSize = true;
            this.basincTetiklendiMi.Location = new System.Drawing.Point(186, 348);
            this.basincTetiklendiMi.Name = "basincTetiklendiMi";
            this.basincTetiklendiMi.Size = new System.Drawing.Size(152, 13);
            this.basincTetiklendiMi.TabIndex = 12;
            this.basincTetiklendiMi.Text = "Basınç İle Ayrılma Tetiklenmedi";
            // 
            // chartGeiger
            // 
            this.chartGeiger.BackColor = System.Drawing.Color.WhiteSmoke;
            chartArea2.Name = "ChartArea1";
            this.chartGeiger.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.chartGeiger.Legends.Add(legend2);
            this.chartGeiger.Location = new System.Drawing.Point(13, 438);
            this.chartGeiger.Name = "chartGeiger";
            this.chartGeiger.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.SeaGreen;
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Rad";
            this.chartGeiger.Series.Add(series2);
            this.chartGeiger.Size = new System.Drawing.Size(288, 158);
            this.chartGeiger.TabIndex = 11;
            this.chartGeiger.Text = "Geiger Verileri";
            // 
            // label49
            // 
            this.label49.AutoSize = true;
            this.label49.Location = new System.Drawing.Point(186, 201);
            this.label49.Name = "label49";
            this.label49.Size = new System.Drawing.Size(89, 13);
            this.label49.TabIndex = 10;
            this.label49.Text = "Basınç (BME280)";
            // 
            // label48
            // 
            this.label48.AutoSize = true;
            this.label48.Location = new System.Drawing.Point(186, 222);
            this.label48.Name = "label48";
            this.label48.Size = new System.Drawing.Size(29, 13);
            this.label48.TabIndex = 9;
            this.label48.Text = "Nem";
            // 
            // label47
            // 
            this.label47.AutoSize = true;
            this.label47.Location = new System.Drawing.Point(186, 243);
            this.label47.Name = "label47";
            this.label47.Size = new System.Drawing.Size(44, 13);
            this.label47.TabIndex = 8;
            this.label47.Text = "Sıcaklık";
            // 
            // lbl_haritaC_boylam
            // 
            this.lbl_haritaC_boylam.AutoSize = true;
            this.lbl_haritaC_boylam.Location = new System.Drawing.Point(186, 312);
            this.lbl_haritaC_boylam.Name = "lbl_haritaC_boylam";
            this.lbl_haritaC_boylam.Size = new System.Drawing.Size(85, 13);
            this.lbl_haritaC_boylam.TabIndex = 7;
            this.lbl_haritaC_boylam.Text = "Harita C. Boylam";
            // 
            // lbl_haritaC_enlem
            // 
            this.lbl_haritaC_enlem.AutoSize = true;
            this.lbl_haritaC_enlem.Location = new System.Drawing.Point(186, 291);
            this.lbl_haritaC_enlem.Name = "lbl_haritaC_enlem";
            this.lbl_haritaC_enlem.Size = new System.Drawing.Size(80, 13);
            this.lbl_haritaC_enlem.TabIndex = 6;
            this.lbl_haritaC_enlem.Text = "Harita C. Enlem";
            // 
            // lbl_takimID_dgr
            // 
            this.lbl_takimID_dgr.AutoSize = true;
            this.lbl_takimID_dgr.Location = new System.Drawing.Point(140, 27);
            this.lbl_takimID_dgr.Name = "lbl_takimID_dgr";
            this.lbl_takimID_dgr.Size = new System.Drawing.Size(10, 13);
            this.lbl_takimID_dgr.TabIndex = 5;
            this.lbl_takimID_dgr.Text = "-";
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Location = new System.Drawing.Point(140, 285);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(13, 13);
            this.label32.TabIndex = 3;
            this.label32.Text = "0";
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(140, 348);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(13, 13);
            this.label30.TabIndex = 3;
            this.label30.Text = "0";
            // 
            // gYukuBoylam
            // 
            this.gYukuBoylam.AutoSize = true;
            this.gYukuBoylam.Location = new System.Drawing.Point(140, 243);
            this.gYukuBoylam.Name = "gYukuBoylam";
            this.gYukuBoylam.Size = new System.Drawing.Size(13, 13);
            this.gYukuBoylam.TabIndex = 3;
            this.gYukuBoylam.Text = "0";
            // 
            // JiroskopZ
            // 
            this.JiroskopZ.AutoSize = true;
            this.JiroskopZ.Location = new System.Drawing.Point(140, 411);
            this.JiroskopZ.Name = "JiroskopZ";
            this.JiroskopZ.Size = new System.Drawing.Size(13, 13);
            this.JiroskopZ.TabIndex = 3;
            this.JiroskopZ.Text = "0";
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(10, 285);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(22, 13);
            this.label31.TabIndex = 2;
            this.label31.Text = "Açı";
            // 
            // roketBoylam
            // 
            this.roketBoylam.AutoSize = true;
            this.roketBoylam.Location = new System.Drawing.Point(140, 161);
            this.roketBoylam.Name = "roketBoylam";
            this.roketBoylam.Size = new System.Drawing.Size(13, 13);
            this.roketBoylam.TabIndex = 3;
            this.roketBoylam.Text = "0";
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(10, 348);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(40, 13);
            this.label28.TabIndex = 2;
            this.label28.Text = "İvme Z";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(10, 243);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(83, 13);
            this.label17.TabIndex = 2;
            this.label17.Text = "G. Yükü Boylam";
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(10, 411);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(56, 13);
            this.label27.TabIndex = 2;
            this.label27.Text = "Jiroskop Z";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 161);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(73, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Roket Boylam";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(140, 327);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(13, 13);
            this.label26.TabIndex = 3;
            this.label26.Text = "0";
            // 
            // gYukuEnlem
            // 
            this.gYukuEnlem.AutoSize = true;
            this.gYukuEnlem.Location = new System.Drawing.Point(140, 222);
            this.gYukuEnlem.Name = "gYukuEnlem";
            this.gYukuEnlem.Size = new System.Drawing.Size(13, 13);
            this.gYukuEnlem.TabIndex = 3;
            this.gYukuEnlem.Text = "0";
            // 
            // JiroskopY
            // 
            this.JiroskopY.AutoSize = true;
            this.JiroskopY.Location = new System.Drawing.Point(140, 390);
            this.JiroskopY.Name = "JiroskopY";
            this.JiroskopY.Size = new System.Drawing.Size(13, 13);
            this.JiroskopY.TabIndex = 3;
            this.JiroskopY.Text = "0";
            // 
            // roketEnlem
            // 
            this.roketEnlem.AutoSize = true;
            this.roketEnlem.Location = new System.Drawing.Point(140, 142);
            this.roketEnlem.Name = "roketEnlem";
            this.roketEnlem.Size = new System.Drawing.Size(13, 13);
            this.roketEnlem.TabIndex = 3;
            this.roketEnlem.Text = "0";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(10, 327);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(40, 13);
            this.label24.TabIndex = 2;
            this.label24.Text = "İvme Y";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(10, 222);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(78, 13);
            this.label15.TabIndex = 2;
            this.label15.Text = "G. Yükü Enlem";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(10, 390);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(56, 13);
            this.label23.TabIndex = 2;
            this.label23.Text = "Jiroskop Y";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 142);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Roket Enlem";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(140, 306);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(13, 13);
            this.label22.TabIndex = 3;
            this.label22.Text = "0";
            // 
            // gYukuGPSIrtifa
            // 
            this.gYukuGPSIrtifa.AutoSize = true;
            this.gYukuGPSIrtifa.Location = new System.Drawing.Point(140, 201);
            this.gYukuGPSIrtifa.Name = "gYukuGPSIrtifa";
            this.gYukuGPSIrtifa.Size = new System.Drawing.Size(13, 13);
            this.gYukuGPSIrtifa.TabIndex = 3;
            this.gYukuGPSIrtifa.Text = "0";
            // 
            // JiroskopX
            // 
            this.JiroskopX.AutoSize = true;
            this.JiroskopX.Location = new System.Drawing.Point(140, 369);
            this.JiroskopX.Name = "JiroskopX";
            this.JiroskopX.Size = new System.Drawing.Size(13, 13);
            this.JiroskopX.TabIndex = 3;
            this.JiroskopX.Text = "0";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(10, 306);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(40, 13);
            this.label20.TabIndex = 2;
            this.label20.Text = "İvme X";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(10, 369);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(56, 13);
            this.label19.TabIndex = 2;
            this.label19.Text = "Jiroskop X";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(10, 201);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(100, 13);
            this.label13.TabIndex = 2;
            this.label13.Text = "G. Yükü İrtifa (GPS)";
            // 
            // roketGPSIrtifa
            // 
            this.roketGPSIrtifa.AutoSize = true;
            this.roketGPSIrtifa.Location = new System.Drawing.Point(140, 123);
            this.roketGPSIrtifa.Name = "roketGPSIrtifa";
            this.roketGPSIrtifa.Size = new System.Drawing.Size(13, 13);
            this.roketGPSIrtifa.TabIndex = 3;
            this.roketGPSIrtifa.Text = "0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 123);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Roket İrtifa (GPS)";
            // 
            // lbl_bsncIrtıfa_dgr
            // 
            this.lbl_bsncIrtıfa_dgr.AutoSize = true;
            this.lbl_bsncIrtıfa_dgr.Location = new System.Drawing.Point(140, 104);
            this.lbl_bsncIrtıfa_dgr.Name = "lbl_bsncIrtıfa_dgr";
            this.lbl_bsncIrtıfa_dgr.Size = new System.Drawing.Size(13, 13);
            this.lbl_bsncIrtıfa_dgr.TabIndex = 3;
            this.lbl_bsncIrtıfa_dgr.Text = "0";
            // 
            // lbl_bsncIrtifa
            // 
            this.lbl_bsncIrtifa.AutoSize = true;
            this.lbl_bsncIrtifa.Location = new System.Drawing.Point(10, 104);
            this.lbl_bsncIrtifa.Name = "lbl_bsncIrtifa";
            this.lbl_bsncIrtifa.Size = new System.Drawing.Size(100, 13);
            this.lbl_bsncIrtifa.TabIndex = 2;
            this.lbl_bsncIrtifa.Text = "Roket İrtifa (Basınç)";
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.Location = new System.Drawing.Point(140, 85);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(13, 13);
            this.label36.TabIndex = 3;
            this.label36.Text = "0";
            // 
            // lbl_haritaC_boylam_dgr
            // 
            this.lbl_haritaC_boylam_dgr.AutoSize = true;
            this.lbl_haritaC_boylam_dgr.Location = new System.Drawing.Point(315, 312);
            this.lbl_haritaC_boylam_dgr.Name = "lbl_haritaC_boylam_dgr";
            this.lbl_haritaC_boylam_dgr.Size = new System.Drawing.Size(13, 13);
            this.lbl_haritaC_boylam_dgr.TabIndex = 3;
            this.lbl_haritaC_boylam_dgr.Text = "0";
            this.lbl_haritaC_boylam_dgr.Click += new System.EventHandler(this.lbl_haritaC_boylam_dgr_Click);
            // 
            // lbl_haritaC_enlem_dgr
            // 
            this.lbl_haritaC_enlem_dgr.AutoSize = true;
            this.lbl_haritaC_enlem_dgr.Location = new System.Drawing.Point(315, 293);
            this.lbl_haritaC_enlem_dgr.Name = "lbl_haritaC_enlem_dgr";
            this.lbl_haritaC_enlem_dgr.Size = new System.Drawing.Size(13, 13);
            this.lbl_haritaC_enlem_dgr.TabIndex = 3;
            this.lbl_haritaC_enlem_dgr.Text = "0";
            // 
            // Basinc
            // 
            this.Basinc.AutoSize = true;
            this.Basinc.Location = new System.Drawing.Point(315, 201);
            this.Basinc.Name = "Basinc";
            this.Basinc.Size = new System.Drawing.Size(13, 13);
            this.Basinc.TabIndex = 3;
            this.Basinc.Text = "0";
            // 
            // label42
            // 
            this.label42.AutoSize = true;
            this.label42.Location = new System.Drawing.Point(315, 142);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(13, 13);
            this.label42.TabIndex = 3;
            this.label42.Text = "0";
            // 
            // label51
            // 
            this.label51.AutoSize = true;
            this.label51.Location = new System.Drawing.Point(315, 222);
            this.label51.Name = "label51";
            this.label51.Size = new System.Drawing.Size(13, 13);
            this.label51.TabIndex = 3;
            this.label51.Text = "0";
            // 
            // label40
            // 
            this.label40.AutoSize = true;
            this.label40.Location = new System.Drawing.Point(315, 123);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(13, 13);
            this.label40.TabIndex = 3;
            this.label40.Text = "0";
            // 
            // label50
            // 
            this.label50.AutoSize = true;
            this.label50.Location = new System.Drawing.Point(315, 243);
            this.label50.Name = "label50";
            this.label50.Size = new System.Drawing.Size(13, 13);
            this.label50.TabIndex = 3;
            this.label50.Text = "0";
            // 
            // label38
            // 
            this.label38.AutoSize = true;
            this.label38.Location = new System.Drawing.Point(315, 104);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(13, 13);
            this.label38.TabIndex = 3;
            this.label38.Text = "0";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(315, 85);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(13, 13);
            this.label12.TabIndex = 3;
            this.label12.Text = "0";
            // 
            // roketHizY
            // 
            this.roketHizY.AutoSize = true;
            this.roketHizY.Location = new System.Drawing.Point(315, 66);
            this.roketHizY.Name = "roketHizY";
            this.roketHizY.Size = new System.Drawing.Size(13, 13);
            this.roketHizY.TabIndex = 3;
            this.roketHizY.Text = "0";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(315, 47);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(13, 13);
            this.label8.TabIndex = 3;
            this.label8.Text = "0";
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Location = new System.Drawing.Point(140, 47);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(13, 13);
            this.label34.TabIndex = 3;
            this.label34.Text = "0";
            // 
            // label41
            // 
            this.label41.AutoSize = true;
            this.label41.Location = new System.Drawing.Point(186, 142);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(63, 13);
            this.label41.TabIndex = 2;
            this.label41.Text = "Açısal Hız Z";
            // 
            // label39
            // 
            this.label39.AutoSize = true;
            this.label39.Location = new System.Drawing.Point(186, 123);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(63, 13);
            this.label39.TabIndex = 2;
            this.label39.Text = "Açısal Hız Y";
            // 
            // lbl_paketSayac_dgr
            // 
            this.lbl_paketSayac_dgr.AutoSize = true;
            this.lbl_paketSayac_dgr.Location = new System.Drawing.Point(140, 66);
            this.lbl_paketSayac_dgr.Name = "lbl_paketSayac_dgr";
            this.lbl_paketSayac_dgr.Size = new System.Drawing.Size(13, 13);
            this.lbl_paketSayac_dgr.TabIndex = 3;
            this.lbl_paketSayac_dgr.Text = "0";
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.Location = new System.Drawing.Point(186, 104);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(63, 13);
            this.label37.TabIndex = 2;
            this.label37.Text = "Açısal Hız X";
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Location = new System.Drawing.Point(10, 85);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(38, 13);
            this.label35.TabIndex = 2;
            this.label35.Text = "Durum";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(186, 85);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(64, 13);
            this.label11.TabIndex = 2;
            this.label11.Text = "Roket Hız Z";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(186, 66);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(64, 13);
            this.label9.TabIndex = 2;
            this.label9.Text = "Roket Hız Y";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(186, 47);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(64, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "Roket Hız X";
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Location = new System.Drawing.Point(10, 47);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(29, 13);
            this.label33.TabIndex = 2;
            this.label33.Text = "CRC";
            // 
            // lbl_paketSayac
            // 
            this.lbl_paketSayac.AutoSize = true;
            this.lbl_paketSayac.Location = new System.Drawing.Point(10, 66);
            this.lbl_paketSayac.Name = "lbl_paketSayac";
            this.lbl_paketSayac.Size = new System.Drawing.Size(70, 13);
            this.lbl_paketSayac.TabIndex = 2;
            this.lbl_paketSayac.Text = "Paket Sayacı";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(82, 24);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(32, 20);
            this.textBox1.TabIndex = 1;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // lbl_takimID
            // 
            this.lbl_takimID.AutoSize = true;
            this.lbl_takimID.Location = new System.Drawing.Point(10, 27);
            this.lbl_takimID.Name = "lbl_takimID";
            this.lbl_takimID.Size = new System.Drawing.Size(50, 13);
            this.lbl_takimID.TabIndex = 0;
            this.lbl_takimID.Text = "Takım ID";
            // 
            // konumaGit_Enlem
            // 
            this.konumaGit_Enlem.Location = new System.Drawing.Point(397, 544);
            this.konumaGit_Enlem.Name = "konumaGit_Enlem";
            this.konumaGit_Enlem.Size = new System.Drawing.Size(100, 20);
            this.konumaGit_Enlem.TabIndex = 2;
            // 
            // lbl_gidilecekEnlem
            // 
            this.lbl_gidilecekEnlem.AutoSize = true;
            this.lbl_gidilecekEnlem.Location = new System.Drawing.Point(406, 528);
            this.lbl_gidilecekEnlem.Name = "lbl_gidilecekEnlem";
            this.lbl_gidilecekEnlem.Size = new System.Drawing.Size(83, 13);
            this.lbl_gidilecekEnlem.TabIndex = 3;
            this.lbl_gidilecekEnlem.Text = "Gidilecek Enlem";
            // 
            // konumaGit_Boylam
            // 
            this.konumaGit_Boylam.Location = new System.Drawing.Point(519, 544);
            this.konumaGit_Boylam.Name = "konumaGit_Boylam";
            this.konumaGit_Boylam.Size = new System.Drawing.Size(100, 20);
            this.konumaGit_Boylam.TabIndex = 2;
            // 
            // lbl_gidilecekBoylam
            // 
            this.lbl_gidilecekBoylam.AutoSize = true;
            this.lbl_gidilecekBoylam.Location = new System.Drawing.Point(525, 528);
            this.lbl_gidilecekBoylam.Name = "lbl_gidilecekBoylam";
            this.lbl_gidilecekBoylam.Size = new System.Drawing.Size(88, 13);
            this.lbl_gidilecekBoylam.TabIndex = 3;
            this.lbl_gidilecekBoylam.Text = "Gidilecek Boylam";
            // 
            // btnKonumaGit
            // 
            this.btnKonumaGit.Location = new System.Drawing.Point(641, 543);
            this.btnKonumaGit.Name = "btnKonumaGit";
            this.btnKonumaGit.Size = new System.Drawing.Size(91, 20);
            this.btnKonumaGit.TabIndex = 4;
            this.btnKonumaGit.Text = "KONUMA GİT";
            this.btnKonumaGit.UseVisualStyleBackColor = true;
            this.btnKonumaGit.Click += new System.EventHandler(this.btnKonumaGit_Click);
            // 
            // btnSimBaslat
            // 
            this.btnSimBaslat.Location = new System.Drawing.Point(773, 543);
            this.btnSimBaslat.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnSimBaslat.Name = "btnSimBaslat";
            this.btnSimBaslat.Size = new System.Drawing.Size(113, 20);
            this.btnSimBaslat.TabIndex = 5;
            this.btnSimBaslat.Text = "Simülasyonu Başlat";
            this.btnSimBaslat.UseVisualStyleBackColor = true;
            this.btnSimBaslat.Click += new System.EventHandler(this.btnSimBaslat_Click);
            // 
            // lbl_gecenSure
            // 
            this.lbl_gecenSure.AutoSize = true;
            this.lbl_gecenSure.Location = new System.Drawing.Point(186, 390);
            this.lbl_gecenSure.Name = "lbl_gecenSure";
            this.lbl_gecenSure.Size = new System.Drawing.Size(106, 13);
            this.lbl_gecenSure.TabIndex = 6;
            this.lbl_gecenSure.Text = "Geçen Süre (saniye):";
            this.lbl_gecenSure.Click += new System.EventHandler(this.lbl_gecenSure_Click);
            // 
            // gecenSure
            // 
            this.gecenSure.AutoSize = true;
            this.gecenSure.Location = new System.Drawing.Point(297, 390);
            this.gecenSure.Name = "gecenSure";
            this.gecenSure.Size = new System.Drawing.Size(13, 13);
            this.gecenSure.TabIndex = 7;
            this.gecenSure.Text = "0";
            // 
            // uc_AnaSayfa
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnSimBaslat);
            this.Controls.Add(this.btnKonumaGit);
            this.Controls.Add(this.lbl_gidilecekBoylam);
            this.Controls.Add(this.lbl_gidilecekEnlem);
            this.Controls.Add(this.konumaGit_Boylam);
            this.Controls.Add(this.konumaGit_Enlem);
            this.Controls.Add(this.gboxVeriler);
            this.Controls.Add(this.anasayfaHarita);
            this.Name = "uc_AnaSayfa";
            this.Size = new System.Drawing.Size(986, 609);
            this.Load += new System.EventHandler(this.uc_AnaSayfa_Load);
            this.gboxVeriler.ResumeLayout(false);
            this.gboxVeriler.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartGeiger)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }


      
    }
}
