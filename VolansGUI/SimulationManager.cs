using System;
using System.Windows.Forms;
using OxyPlot.Series;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using GMap.NET;
using OxyPlot;

namespace VolansGUI
{
    public class SimulationManager
    {
        private readonly System.Windows.Forms.Timer simTimer;
        private double simTime;
        private readonly double timeStep;
        private readonly int maxPoints;

        // Simülasyon değerleri
        public float Enlem { get; private set; }
        public float Boylam { get;  set; }
        public float Irtifa { get; internal set; }
        public float Hiz { get; private set; }

        public bool IsRunning { get; private set; } = false;

        private readonly LineSeries lineSeries;
        private readonly LineSeries lineSeries2;
        private readonly Action refreshPlot1;
        private readonly Action refreshPlot2;

        private readonly GMapControl gmap;
        private readonly GMarkerGoogle marker;
        private readonly double baseLat;
        private readonly double baseLng;
        public SimulationManager(

        )
        {

        }
        public SimulationManager(
            LineSeries series1,
            LineSeries series2,
            Action refreshPlot1,
            Action refreshPlot2,
            GMapControl gmapControl,
            GMarkerGoogle rocketMarker,
            double startLat,
            double startLng
        )
        {
            simTimer = new System.Windows.Forms.Timer();
            simTimer.Interval = 100; // 100ms
            simTimer.Tick += SimTimer_Tick;

            simTime = 0;
            timeStep = 0.1;
            maxPoints = 100;

            this.lineSeries = series1;
            this.lineSeries2 = series2;
            this.refreshPlot1 = refreshPlot1;
            this.refreshPlot2 = refreshPlot2;
            this.gmap = gmapControl;
            this.marker = rocketMarker;
            this.baseLat = startLat;
            this.baseLng = startLng;
        }

        public void Start()
        {
            if (!IsRunning)
            {
               // simTimer.Start();
                //IsRunning = true;
            }
        }

        public void Stop()
        {
            if (IsRunning)
            {
                simTimer.Stop();
                IsRunning = false;
            }
        }

        public void Reset()
        {
            Stop();
            simTime = 0;
            Enlem = 0;
            Boylam = 0;
            Irtifa = 0;
            Hiz = 0;

            //lineSeries.Points.Clear();
            //lineSeries2.Points.Clear();
            //refreshPlot1?.Invoke();
            //refreshPlot2?.Invoke();

            //marker.Position = new PointLatLng(baseLat, baseLng);
            //gmap.Position = marker.Position;
        }

        protected virtual void SimTimer_Tick(object sender, EventArgs e)
        {
            simTime += timeStep;

            // Hesaplamalar
            Irtifa = (float)(-6 * simTime * (simTime - 40));
            Hiz = (float)(-12 * simTime + 240);
            Enlem += (float)(simTime * 0.00002);
            Boylam += (float)(simTime * simTime * 0.00001);

            // Grafik güncelle
            lineSeries.Points.Add(new DataPoint(simTime, Irtifa));
            if (lineSeries.Points.Count > maxPoints)
                lineSeries.Points.RemoveAt(0);
            refreshPlot1?.Invoke();

            // Eğimi gösteren sabit çizgi
            double slope = Math.Tan(45 * Math.PI / 180);
            lineSeries2.Points.Clear();
            for (double t = 0; t <= 10; t += timeStep)
            {
                double y = slope * t;
                lineSeries2.Points.Add(new DataPoint(t, y));
            }
            refreshPlot2?.Invoke();

            // Harita güncelle
            double newLat = baseLat + Enlem;
            double newLng = baseLng + Boylam;
            marker.Position = new PointLatLng(newLat, newLng);
            gmap.Position = marker.Position;
        }
    }
}
