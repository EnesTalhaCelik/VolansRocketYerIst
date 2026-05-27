using GMap.NET;
using GMap.NET.WindowsForms;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace VolansYerIstasyonu.DrawMode
{
    /// <summary>
    /// Bir nokta listesinden ardışık segmentler oluşturur ve segment indeksine
    /// göre renklerini bir gradient üzerinden seçer.
    /// 
    /// Varsayılan: en eski segment KIRMIZI, en yeni segment MAVİ.
    /// Böylece hareket yönü görsel olarak takip edilebilir.
    ///
    /// KULLANIM:
    ///   new DrawMode_GradientLine().Draw(routesOverlay, positions);
    /// 
    /// NOT: Bu metot routesOverlay.Routes'u TAMAMEN TEMİZLER ve yeniden çizer.
    /// Çünkü gradient'i koruyabilmek için her segmentin rengi her yeni nokta
    /// geldiğinde yeniden hesaplanmalıdır.
    /// </summary>
    public class DrawMode_GradientLine
    {
        /// <summary>Başlangıç (en eski) segment rengi</summary>
        public Color BaslangicRengi { get; set; } = Color.Red;

        /// <summary>Son (en yeni) segment rengi</summary>
        public Color SonRengi { get; set; } = Color.Blue;

        /// <summary>Çizgi kalınlığı (piksel)</summary>
        public float CizgiKalinlik { get; set; } = 3f;

        public void Draw(GMapOverlay overlay, List<PointLatLng> noktalar)
        {
            if (overlay == null || noktalar == null) return;

            // En az 2 nokta yoksa segment çizilemez
            if (noktalar.Count < 2) return;

            // Önceki rotaları temizle (renkler yeniden hesaplanacak)
            overlay.Routes.Clear();

            int segmentSayisi = noktalar.Count - 1;

            for (int i = 0; i < segmentSayisi; i++)
            {
                // t: 0 (en eski) → 1 (en yeni)
                // segmentSayisi == 1 ise t = 1 (tek segment, son renk)
                float t = (segmentSayisi == 1)
                    ? 1f
                    : (float)i / (segmentSayisi - 1);

                Color segmentRengi = RenkInterpolasyon(BaslangicRengi, SonRengi, t);

                var segment = new List<PointLatLng>
                {
                    noktalar[i],
                    noktalar[i + 1]
                };

                // GMapRoute, isim parametresi alıyor; eşsiz olmasına dikkat
                var route = new GMapRoute(segment, $"seg_{i}")
                {
                    Stroke = new Pen(segmentRengi, CizgiKalinlik)
                };

                overlay.Routes.Add(route);
            }
        }

        /// <summary>
        /// İki renk arasında lineer interpolasyon (RGB uzayında).
        /// t: 0 = a, 1 = b, ortadakiler ara renk.
        /// </summary>
        private static Color RenkInterpolasyon(Color a, Color b, float t)
        {
            // t'yi [0, 1] aralığında klamp
            if (t < 0f) t = 0f;
            if (t > 1f) t = 1f;

            int r = (int)(a.R + (b.R - a.R) * t);
            int g = (int)(a.G + (b.G - a.G) * t);
            int bl = (int)(a.B + (b.B - a.B) * t);

            return Color.FromArgb(r, g, bl);
        }
    }
}
