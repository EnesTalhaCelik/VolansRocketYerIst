using MaterialSkin;
using MaterialSkin.Controls;
using OxyPlot.Series;
using OxyPlot.WindowsForms;
using OxyPlot;
using System.Drawing.Drawing2D;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using GMap.NET;
using System.Windows.Forms;

namespace VolansGUI
{
    public partial class Form1 : MaterialForm
    {
        private LineSeries lineSeries;
        private LineSeries lineSeries2;
        private double maxTime = 10;
        private SimulationManager simManager = new SimulationManager();
        private System.Windows.Forms.Timer timer;

        public GMapOverlay markerOverlay;
        public GMarkerGoogle rocketMarker;
        public GMapRoute rocketPath;
        public List<PointLatLng> pathPoints = new List<PointLatLng>();
        private const double startLat = 40.716102;
        private const double startLng = 31.525017;

        public Form1()
        {
            InitializeComponent();
            InitializePlot();
            InitializePieChart();
            InitializeTimer();
            gMapControl2.ShowCenter = false;

            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.EnforceBackcolorOnAllComponents = false;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(
                ColorTranslator.FromHtml("#212121"),
                ColorTranslator.FromHtml("#171717"),
                ColorTranslator.FromHtml("#0f0f0f"),
                ColorTranslator.FromHtml("#000000"),
                TextShade.WHITE
            );


            this.Size = new Size(1870, 970);

            gMapControl2.DragButton = MouseButtons.Left;
            gMapControl2.MapProvider = GMap.NET.MapProviders.GoogleMapProvider.Instance;
            GMaps.Instance.Mode = AccessMode.ServerOnly;
            gMapControl2.Position = new PointLatLng(startLat, startLng);
            gMapControl2.MinZoom = 1;
            gMapControl2.MaxZoom = 20;
            gMapControl2.Zoom = 17;

            markerOverlay = new GMapOverlay("markers");
            rocketMarker = new GMarkerGoogle(new PointLatLng(startLat, startLng), GMarkerGoogleType.red_dot);
            markerOverlay.Markers.Add(rocketMarker);

            rocketPath = new GMapRoute(pathPoints, "roketRotasi")
            {
                Stroke = new Pen(Color.Red, 2)
            };
            markerOverlay.Routes.Add(rocketPath);
            gMapControl2.Overlays.Add(markerOverlay);
            /*
            simManager = new SimulationManager(
                lineSeries,
                lineSeries2,
                
                gMapControl2,
                rocketMarker,
                32,
                31
            );*/
        }

        private void materialButton2_Click(object sender, EventArgs e)
        {
            if (simManager.IsRunning)
            {
                simManager.Stop();
                materialButton2.Text = "Baþlat";
            }
            else
            {
                simManager.Irtifa = 1;
                simManager.Start();
                materialButton2.Text = "Durdur";
            }
        }

        private void materialButton3_Click(object sender, EventArgs e)
        {
            simManager.Reset();
            pathPoints.Clear();
            rocketPath.Points.Clear();
            gMapControl2.Refresh();
            materialButton2.Text = "Baþlat";
        }

        private void InitializeTimer()
        {
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 100;
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {


            var newPos = new PointLatLng(simManager.Enlem + startLat, simManager.Boylam + startLng);
            pathPoints.Add(newPos);
            rocketPath.Points.Clear();
            rocketPath.Points.AddRange(pathPoints);
            gMapControl2.Refresh();

            materialLabel34.Text = $"Enlem: {simManager.Enlem + startLat:F6}";
            materialLabel33.Text = $"Boylam: {simManager.Boylam + startLng:F6}";
            materialLabel35.Text = $"Ýrtifa: {simManager.Irtifa:F2} m";

            if (simManager.IsRunning && simManager.Irtifa <= 0)
            {
                simManager.Stop();
                materialButton2.Text = "Baþlat";
                return;
            }
        }

        private void InitializePlot()
        {
            var plotModel = new PlotModel { Title = "Anten Yönü", TitleColor = OxyColors.White };
            var xAxis = new OxyPlot.Axes.LinearAxis
            {
                Position = OxyPlot.Axes.AxisPosition.Bottom,
                Minimum = 0,
                Maximum = maxTime,
                TitleColor = OxyColors.White,
                TextColor = OxyColors.White,
                AxislineColor = OxyColors.White,
                MajorGridlineColor = OxyColors.White,
                MinorGridlineColor = OxyColors.White,
                MinorGridlineStyle = LineStyle.None,
                MinorTickSize = 0
            };
            var yAxis = new OxyPlot.Axes.LinearAxis
            {
                Position = OxyPlot.Axes.AxisPosition.Left,
                Minimum = 0,
                Maximum = 360,
                TitleColor = OxyColors.White,
                TextColor = OxyColors.White,
                AxislineColor = OxyColors.White,
                MajorGridlineColor = OxyColors.White,
                MinorGridlineColor = OxyColors.White,
                MinorGridlineStyle = LineStyle.None,
                MinorTickSize = 0
            };
            lineSeries = new LineSeries
            {
                Title = "Veri Serisi",
                StrokeThickness = 2,
                MarkerType = MarkerType.None,
                LineStyle = LineStyle.Solid,
                Color = OxyColors.Green
            };
            plotModel.Series.Add(lineSeries);
            plotModel.Axes.Add(xAxis);
            plotModel.Axes.Add(yAxis);

            var plotModel2 = new PlotModel { Title = "Anten Eðimi", TitleColor = OxyColors.White };
            var xAxis2 = new OxyPlot.Axes.LinearAxis
            {
                Position = OxyPlot.Axes.AxisPosition.Bottom,
                Minimum = 0,
                Maximum = 1,
                TitleColor = OxyColors.White,
                TextColor = OxyColors.White,
                AxislineColor = OxyColors.White,
                MajorGridlineColor = OxyColors.White,
                MinorGridlineColor = OxyColors.White,
                MinorGridlineStyle = LineStyle.None,
                MinorTickSize = 0
            };
            var yAxis2 = new OxyPlot.Axes.LinearAxis
            {
                Position = OxyPlot.Axes.AxisPosition.Left,
                Minimum = 0,
                Maximum = 1,
                TitleColor = OxyColors.White,
                TextColor = OxyColors.White,
                AxislineColor = OxyColors.White,
                MajorGridlineColor = OxyColors.White,
                MinorGridlineColor = OxyColors.White,
                MinorGridlineStyle = LineStyle.None,
                MinorTickSize = 0
            };
            lineSeries2 = new LineSeries
            {
                Title = "Eðimli Çizgi",
                StrokeThickness = 2,
                MarkerType = MarkerType.None,
                LineStyle = LineStyle.Solid,
                Color = OxyColors.Green
            };
            plotModel2.Series.Add(lineSeries2);
            plotModel2.Axes.Add(xAxis2);
            plotModel2.Axes.Add(yAxis2);
        }

        private void InitializePieChart()
        {
            var plotModel3 = new PlotModel { Title = "Anten Eðim Kapasitesi", TitleColor = OxyColors.White };
            var pieSeries = new PieSeries
            {
                StrokeThickness = 1,
                InsideLabelPosition = 0.5,
                AngleSpan = 360,
                StartAngle = 0
            };
            pieSeries.Slices.Add(new PieSlice("", 0) { IsExploded = false, Fill = OxyColors.Gray });
            pieSeries.Slices.Add(new PieSlice("", 100) { IsExploded = false, Fill = OxyColors.Green });
            plotModel3.Series.Add(pieSeries);

            var plotModel4 = new PlotModel { Title = "Antenna Direction Capacity", TitleColor = OxyColors.White };
            var pieSeries4 = new PieSeries
            {
                StrokeThickness = 1,
                InsideLabelPosition = 0.5,
                AngleSpan = 360,
                StartAngle = 0
            };
            pieSeries4.Slices.Add(new PieSlice("", 0) { IsExploded = false, Fill = OxyColors.Gray });
            pieSeries4.Slices.Add(new PieSlice("", 100) { IsExploded = false, Fill = OxyColors.Green });
            plotModel4.Series.Add(pieSeries4);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void panel25_Paint(object sender, PaintEventArgs e)
        {

        }

        private void gMapControl2_Load(object sender, EventArgs e)
        {

        }
    }
}
