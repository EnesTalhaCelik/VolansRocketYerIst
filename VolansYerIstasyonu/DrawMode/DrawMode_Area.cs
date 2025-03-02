using GMap.NET.WindowsForms;
using GMap.NET;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolansYerIstasyonu.DrawMode
{
    internal class DrawMode_Area : IDrawMode
    {

        public void Draw(GMapOverlay overlay, List<PointLatLng> points)
        {
            overlay.Polygons.Clear();

            if (points.Count < 3) return;

            var polygon = new GMapPolygon(points, "scannedArea")
            {
                Fill = new SolidBrush(Color.FromArgb(50, Color.Yellow)),
                Stroke = new Pen(Color.Red, 2)
            };

            overlay.Polygons.Add(polygon);

            //Console.WriteLine("DrawModeArea is executing...");

        }
    }
}
