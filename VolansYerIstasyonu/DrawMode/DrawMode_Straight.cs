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
    internal class DrawMode_Straight : IDrawMode
    {
        public void Draw(GMapOverlay overlay, List<PointLatLng> points)
        {
            overlay.Routes.Clear(); 

            if (points.Count < 2) return; 

            var route = new GMapRoute(points, "path")
            {
                Stroke = new Pen(Color.Red, 2) 
            };

            overlay.Routes.Add(route);
        }
    }
}
