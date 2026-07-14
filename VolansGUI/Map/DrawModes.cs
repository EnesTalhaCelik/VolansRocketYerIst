using System.Collections.Generic;
using System.Drawing;
using GMap.NET;
using GMap.NET.WindowsForms;

namespace VolansGUI.Map
{
    public interface IDrawMode
    {
        void Draw(GMapOverlay overlay, List<PointLatLng> points);
    }

    /// <summary>İki nokta arasında düz kırmızı çizgi.</summary>
    public class DrawMode_Straight : IDrawMode
    {
        public void Draw(GMapOverlay overlay, List<PointLatLng> points)
        {
            overlay.Routes.Clear();
            if (points.Count < 2) return;
            overlay.Routes.Add(new GMapRoute(points, "path")
            {
                Stroke = new Pen(Color.Red, 2)
            });
        }
    }

    /// <summary>3+ noktadan taranan alan poligonu.</summary>
    public class DrawMode_Area : IDrawMode
    {
        public void Draw(GMapOverlay overlay, List<PointLatLng> points)
        {
            overlay.Polygons.Clear();
            if (points.Count < 3) return;
            overlay.Polygons.Add(new GMapPolygon(points, "scannedArea")
            {
                Fill = new SolidBrush(Color.FromArgb(50, Color.Yellow)),
                Stroke = new Pen(Color.Red, 2)
            });
        }
    }

    /// <summary>
    /// Nokta listesinden ardışık segmentler oluşturur; en eski KIRMIZI, en yeni MAVİ.
    /// Gradient korunabilmesi için her çağrıda Routes tamamen yeniden çizilir.
    /// </summary>
    public class DrawMode_GradientLine
    {
        public Color BaslangicRengi { get; set; } = Color.Red;
        public Color SonRengi { get; set; } = Color.Blue;
        public float CizgiKalinlik { get; set; } = 3f;

        public void Draw(GMapOverlay overlay, List<PointLatLng> noktalar)
        {
            if (overlay == null || noktalar == null || noktalar.Count < 2) return;
            overlay.Routes.Clear();

            int segmentSayisi = noktalar.Count - 1;
            for (int i = 0; i < segmentSayisi; i++)
            {
                float t = (segmentSayisi == 1) ? 1f : (float)i / (segmentSayisi - 1);
                Color renk = RenkInterpolasyon(BaslangicRengi, SonRengi, t);
                var segment = new List<PointLatLng> { noktalar[i], noktalar[i + 1] };
                overlay.Routes.Add(new GMapRoute(segment, $"seg_{i}")
                {
                    Stroke = new Pen(renk, CizgiKalinlik)
                });
            }
        }

        private static Color RenkInterpolasyon(Color a, Color b, float t)
        {
            if (t < 0f) t = 0f;
            if (t > 1f) t = 1f;
            int r = (int)(a.R + (b.R - a.R) * t);
            int g = (int)(a.G + (b.G - a.G) * t);
            int bl = (int)(a.B + (b.B - a.B) * t);
            return Color.FromArgb(r, g, bl);
        }
    }
}
