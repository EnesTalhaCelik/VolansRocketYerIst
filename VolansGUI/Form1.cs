using MaterialSkin;
using MaterialSkin.Controls;
using OxyPlot.Series;
using OxyPlot.WindowsForms;
using OxyPlot;
using System.Drawing.Drawing2D;

namespace VolansGUI
{
    public partial class Form1 : MaterialForm
    {
        private System.Windows.Forms.Timer timer;  // Burada tam ismi belirtiyoruz
        private LineSeries lineSeries;
        private LineSeries lineSeries2; // Ýkinci grafik için yeni LineSeries
        private double time = 0;
        private double maxTime = 10; // Zaman aralýðýný 10 saniye yapýyoruz
        private double timeStep = 0.1; // Zamanýn daha küçük adýmlarla artmasý
        private int dataRate = 10;    // Veri noktalarýnýn daha sýk alýnmasý için sayýyý arttýrdýk
        private int score = 15;
        public Form1()
        {
            InitializeComponent();
            InitializePlot();        // Grafik baþlatma burada yapýlmalý
            InitializeTimer();       // Zamanlayýcý baþlatma burada yapýlmalý
            InitializePieChart();

            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.EnforceBackcolorOnAllComponents = false;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(
                ColorTranslator.FromHtml("#212121"), // action bar, deðiþken olmayan labellar.
                ColorTranslator.FromHtml("#171717"), // durum çubuðu.
                ColorTranslator.FromHtml("#0f0f0f"), // tabpage arkaplan rengi.
                ColorTranslator.FromHtml("#000000"), // tabpage yazýlarý, butonalar, deðiþken labellar.
                TextShade.WHITE
            );
            plotView11.Visible = false;
            this.Size = new Size(1870, 970);

            void InitializePlot()
            {
                // Yeni bir PlotModel (grafik modeli) oluþtur
                var plotModel = new PlotModel
                {
                    Title = "Anten Yönü",
                    TitleColor = OxyColors.White // Baþlýk rengini beyaz yapýyoruz
                };

                // X eksenini oluþtur
                var xAxis = new OxyPlot.Axes.LinearAxis
                {
                    Position = OxyPlot.Axes.AxisPosition.Bottom,
                    Minimum = 0, // Baþlangýçta 0
                    Maximum = maxTime, // Baþlangýçta 10 saniye
                    TitleColor = OxyColors.White, // Eksen baþlýðý beyaz
                    TextColor = OxyColors.White, // Eksen etiketleri beyaz
                    AxislineColor = OxyColors.White, // X ekseninin çizgi rengini beyaz yapýyoruz
                    MajorGridlineColor = OxyColors.White, // Ana grid çizgilerinin rengini beyaz yapýyoruz
                    MinorGridlineColor = OxyColors.White, // Minör grid çizgilerinin rengini beyaz yapýyoruz
                    MinorGridlineStyle = LineStyle.None, // Ara çizgileri gizle
                    MinorTickSize = 0, // Ara týklara yer verme
                };

                // Y eksenini oluþtur (Deðer sabit aralýkta olacak)
                var yAxis = new OxyPlot.Axes.LinearAxis
                {
                    Position = OxyPlot.Axes.AxisPosition.Left,
                    Minimum = 0, // Sabit minimum deðer
                    Maximum = 360,  // Sabit maksimum deðer
                    TitleColor = OxyColors.White, // Eksen baþlýðý beyaz
                    TextColor = OxyColors.White, // Eksen etiketleri beyaz
                    AxislineColor = OxyColors.White, // Y ekseninin çizgi rengini beyaz yapýyoruz
                    MajorGridlineColor = OxyColors.White, // Ana grid çizgilerinin rengini beyaz yapýyoruz
                    MinorGridlineColor = OxyColors.White, // Minör grid çizgilerinin rengini beyaz yapýyoruz
                    MinorGridlineStyle = LineStyle.None, // Ara çizgileri gizle
                    MinorTickSize = 0, // Ara týklara yer verme
                };

                // LineSeries oluþtur (grafikteki çizgiyi gösterir)
                lineSeries = new LineSeries
                {
                    Title = "Veri Serisi",
                    StrokeThickness = 2,
                    MarkerType = MarkerType.None, // Noktalarý kaldýrdýk
                    LineStyle = LineStyle.Solid,  // Akýþkan bir çizgi
                    Color = OxyColors.Green, // Çizgi rengini yeþil yapýyoruz
                };

                // Grafik üzerine veri ekleme
                plotModel.Series.Add(lineSeries);

                // Eksenleri ekle
                plotModel.Axes.Add(xAxis);
                plotModel.Axes.Add(yAxis);

                // plotView1'e modeli atama
                plotView10.Model = plotModel;

                // Ýkinci PlotModel (grafik modeli) oluþtur
                var plotModel2 = new PlotModel
                {
                    Title = "Anten Eðimi",
                    TitleColor = OxyColors.White // Baþlýk rengini beyaz yapýyoruz
                };

                // X ve Y eksenini yeniden oluþturmak gerekebilir (y ekseninde eðim deðiþiyor)
                var xAxis2 = new OxyPlot.Axes.LinearAxis
                {
                    Position = OxyPlot.Axes.AxisPosition.Bottom,
                    Minimum = 0, // Baþlangýçta 0
                    Maximum = 1, // Baþlangýçta 10 saniye
                    TitleColor = OxyColors.White, // Eksen baþlýðý beyaz
                    TextColor = OxyColors.White, // Eksen etiketleri beyaz
                    AxislineColor = OxyColors.White, // X ekseninin çizgi rengini beyaz yapýyoruz
                    MajorGridlineColor = OxyColors.White, // Ana grid çizgilerinin rengini beyaz yapýyoruz
                    MinorGridlineColor = OxyColors.White, // Minör grid çizgilerinin rengini beyaz yapýyoruz
                    MinorGridlineStyle = LineStyle.None, // Ara çizgileri gizle
                    MinorTickSize = 0, // Ara týklara yer verme
                };

                var yAxis2 = new OxyPlot.Axes.LinearAxis
                {
                    Position = OxyPlot.Axes.AxisPosition.Left,
                    Minimum = 0, // Sabit minimum deðer
                    Maximum = 1,  // Sabit maksimum deðer
                    TitleColor = OxyColors.White, // Eksen baþlýðý beyaz
                    TextColor = OxyColors.White, // Eksen etiketleri beyaz
                    AxislineColor = OxyColors.White, // Y ekseninin çizgi rengini beyaz yapýyoruz
                    MajorGridlineColor = OxyColors.White, // Ana grid çizgilerinin rengini beyaz yapýyoruz
                    MinorGridlineColor = OxyColors.White, // Minör grid çizgilerinin rengini beyaz yapýyoruz
                    MinorGridlineStyle = LineStyle.None, // Ara çizgileri gizle
                    MinorTickSize = 0, // Ara týklara yer verme
                };

                // Ýkinci LineSeries oluþtur (eðimli çizgi)
                lineSeries2 = new LineSeries
                {
                    Title = "Eðimli Çizgi",
                    StrokeThickness = 2,
                    MarkerType = MarkerType.None, // Noktalarý kaldýrdýk
                    LineStyle = LineStyle.Solid,  // Akýþkan bir çizgi
                    Color = OxyColors.Green, // Çizgi rengini yeþil yapýyoruz
                };

                // Grafik üzerine veri ekleme
                plotModel2.Series.Add(lineSeries2);

                // Eksenleri ekle
                plotModel2.Axes.Add(xAxis2);
                plotModel2.Axes.Add(yAxis2);

                // plotView8'ye modeli atama
                plotView8.Model = plotModel2;
            }


            void InitializeTimer()
            {
                // Timer'ý oluþtur ve her saniye tetiklenmesini saðla
                timer = new System.Windows.Forms.Timer();  // Tam ismi kullanýyoruz
                timer.Interval = 1000; // 1000 ms (1 saniye) aralýklarla tetiklenecek
                timer.Tick += Timer_Tick;
                timer.Start();
            }

            void InitializePieChart()
            {
                // Yeni bir PlotModel (grafik modeli) oluþtur
                var plotModel3 = new PlotModel
                {
                    Title = "Anten Eðim Kapasitesi",
                    TitleColor = OxyColors.White // Baþlýk rengini beyaz yapýyoruz
                };

                // PieSeries (Pasta grafiði) oluþtur
                var pieSeries = new PieSeries
                {
                    StrokeThickness = 1,
                    InsideLabelPosition = 0.5,  // Ýç etiketlerin pozisyonunu ayarlýyoruz
                    AngleSpan = 360,  // Tam bir daire yapmak için 360 derece
                    StartAngle = 0,   // Baþlangýç açýsýný belirliyoruz
                };

                // Pasta grafiðine veri ekleme
                pieSeries.Slices.Add(new PieSlice("", 0) { IsExploded = false, Fill = OxyColors.Gray });  // Boþluk gri
                pieSeries.Slices.Add(new PieSlice("", 100) { IsExploded = false, Fill = OxyColors.Green });  // Dolu yeþil

                // Grafik üzerine PieSeries ekleme
                plotModel3.Series.Add(pieSeries);

                // plotView9'e modeli atama
                plotView9.Model = plotModel3;

                // plotView11 için yeni bir PlotModel oluþtur
                var plotModel4 = new PlotModel
                {
                    Title = "Antenna Direction Capacity",
                    TitleColor = OxyColors.White // Baþlýk rengini beyaz yapýyoruz
                };

                // PieSeries (Pasta grafiði) oluþtur
                var pieSeries4 = new PieSeries
                {
                    StrokeThickness = 1,
                    InsideLabelPosition = 0.5,  // Ýç etiketlerin pozisyonunu ayarlýyoruz
                    AngleSpan = 360,  // Tam bir daire yapmak için 360 derece
                    StartAngle = 0,   // Baþlangýç açýsýný belirliyoruz
                };
                // Pasta grafiðine veri ekleme (Baþlangýçta 0%)
                pieSeries4.Slices.Add(new PieSlice("", 0) { IsExploded = false, Fill = OxyColors.Gray });
                pieSeries4.Slices.Add(new PieSlice("", 100) { IsExploded = false, Fill = OxyColors.Green });

                // Grafik üzerine PieSeries ekleme
                plotModel4.Series.Add(pieSeries4);

                // plotView11'e modeli atama
                plotView11.Model = plotModel4;
            }

            void Timer_Tick(object sender, EventArgs e)
            {
                // Veriyi her 0.1 saniyede bir ekleyelim ve aralarýndaki farký azaltalým
                for (double t = time; t < time + timeStep * dataRate; t += timeStep)
                {
                    // materialTextBox2'den gelen deðeri al ve Y koordinatýný belirle
                    double yValue = double.TryParse(materialTextBox2.Text, out var result) ? result : 0;
                    lineSeries.Points.Add(new DataPoint(t, yValue)); // Y koordinatýný materialTextBox2'den alýyoruz
                }

                // Zamaný arttýr
                time += timeStep * dataRate;

                // Eksenleri güncelle: X ekseninin minimum deðerini zamanla ilerlet
                if (time > maxTime)
                {
                    // Eski verileri sil
                    lineSeries.Points.RemoveAt(0); // Ýlk veriyi (en eski) sil
                }

                // X ekseninin minimum ve maksimum deðerlerini güncelle
                plotView10.Model.Axes[0].Minimum = time - maxTime; // X ekseninin minimum deðeri
                plotView10.Model.Axes[0].Maximum = time; // X ekseninin maksimum deðeri

                // Grafiði güncelle
                plotView10.InvalidatePlot(true); // Grafik yenileniyor

                // plotView8 için eðimi hesaplayalým ve çizgiyi ekleyelim
                double angle = double.TryParse(materialTextBox1.Text, out var result2) ? result2 : 0;

                // Eðim 90'a yaklaþýrsa, sabit bir deðere sýnýrla (örneðin, 89 derece)
                if (angle >= 90)
                {
                    angle = 89.99999999999999; // Eðimi 89 derece ile sýnýrlýyoruz
                }

                double slope = Math.Tan(angle * Math.PI / 180); // Dereceyi radiana çevirerek eðimi hesaplýyoruz

                // Bu çizgi sabit bir noktadan (0,0) çýkacak
                lineSeries2.Points.Clear(); // Önceki verileri temizleyelim
                for (double t2 = 0; t2 <= 10; t2 += timeStep)
                {
                    double yValue2 = slope * t2; // Eðimli çizgi için Y deðeri
                    if (Math.Abs(yValue2) > 1000)
                    {
                        yValue2 = yValue2 > 0 ? 1000 : -1000; // Sabit bir deðere sýnýrlýyoruz
                    }
                    lineSeries2.Points.Add(new DataPoint(t2, yValue2)); // Yeni noktayý ekle
                }

                // plotView8'yi güncelle
                plotView8.InvalidatePlot(true); // Ýkinci grafik yenileniyor

                // materialTextBox1'den gelen deðeri al ve Pie chart'ý güncelle
                double value = double.TryParse(materialTextBox1.Text, out var result3) ? result3 : 0;

                // Deðeri 90'a bölüp, yüzdeyi hesapla
                double percentage = Math.Min(value / 90.0 * 100, 100);  // Maksimum %100 olacak þekilde sýnýrla

                // Boþluk oranýný hesapla (doluluk oranýnýn tersini alýyoruz)
                double emptyPercentage = 100 - percentage;

                // Pie chart'ýn dilimlerini güncelle
                var plotModel3 = plotView9.Model;

                if (plotModel3.Series.Count > 0)
                {
                    var pieSeries = plotModel3.Series[0] as PieSeries;
                    if (pieSeries != null)
                    {
                        // PieChart dilimlerini sýfýrla
                        pieSeries.Slices.Clear();

                        // Yeni dilimleri ekleyelim (boþluk oraný yeþil, doluluk oraný sarý)
                        pieSeries.Slices.Add(new PieSlice("", emptyPercentage) { IsExploded = false, Fill = OxyColors.Gray });  // Boþluk yeþil
                        pieSeries.Slices.Add(new PieSlice("", percentage) { IsExploded = false, Fill = OxyColors.Green });  // Dolu sarý
                    }
                }

                // Pie chart'ý güncelle
                plotView9.InvalidatePlot(true); // Pie chart yenileniyor

                // materialTextBox2'den gelen deðeri al ve plotView11'ü güncelle
                double value2 = double.TryParse(materialTextBox2.Text, out var result4) ? result4 : 0;

                // 360'a göre %100'ü hesapla
                double percentage2 = Math.Min(value2 / 360.0 * 100, 100);  // Maksimum %100 olacak þekilde sýnýrla

                // Boþluk oranýný hesapla (doluluk oranýnýn tersini alýyoruz)
                double emptyPercentage2 = 100 - percentage2;

                // plotView11'ün Pie chart'ýný güncelle
                var plotModel4 = plotView11.Model;

                if (plotModel4.Series.Count > 0)
                {
                    var pieSeries4 = plotModel4.Series[0] as PieSeries;
                    if (pieSeries4 != null)
                    {
                        // PieChart dilimlerini sýfýrla
                        pieSeries4.Slices.Clear();

                        // Yeni dilimleri ekleyelim (boþluk oraný yeþil, doluluk oraný sarý)
                        pieSeries4.Slices.Add(new PieSlice("", emptyPercentage2) { IsExploded = false, Fill = OxyColors.Gray });  // Boþluk gri
                        pieSeries4.Slices.Add(new PieSlice("", percentage2) { IsExploded = false, Fill = OxyColors.Green });  // Dolu yeþil
                    }
                }

                // plotView11'ü güncelle
                plotView11.InvalidatePlot(true); // plotView11 Pie chart'ý yenileniyor
            }

        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void materialButton4_Click(object sender, EventArgs e)
        {
            // Temayý deðiþtirme
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;

            // Renk þemasýný deðiþtirme
            materialSkinManager.ColorScheme = new ColorScheme(
                ColorTranslator.FromHtml("#212121"), // action bar, deðiþken olmayan labellar.
                ColorTranslator.FromHtml("#171717"), // durum çubuðu.
                ColorTranslator.FromHtml("#0f0f0f"), // tabpage arkaplan rengi.
                ColorTranslator.FromHtml("#000000"), // tabpage yazýlarý, butonalar, deðiþken labellar.
                TextShade.WHITE
            );
        }
        private void materialButton5_Click(object sender, EventArgs e)
        {
            // Temayý deðiþtirme
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;

            // Renk þemasýný deðiþtirme
            materialSkinManager.ColorScheme = new ColorScheme(
                ColorTranslator.FromHtml("#ffffff"), // action bar, deðiþken olmayan labellar.
                ColorTranslator.FromHtml("#f0f0f0"), // durum çubuðu.
                ColorTranslator.FromHtml("#0f0f0f"), // tabpage arkaplan rengi.
                ColorTranslator.FromHtml("#ffffff"), // tabpage yazýlarý, butonalar, deðiþken labellar.
                TextShade.BLACK
            );
        }

        private void materialSwitch2_CheckedChanged(object sender, EventArgs e)
        {
            if (materialSwitch2.Checked)
            {
                plotView10.Visible = false;
                plotView11.Visible = true;
            }
            else
            {
                plotView10.Visible = true;
                plotView11.Visible = false;
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
