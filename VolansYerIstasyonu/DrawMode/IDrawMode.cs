using GMap.NET.WindowsForms;
using GMap.NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolansYerIstasyonu.DrawMode
{
    internal interface IDrawMode
    {
        void Draw(GMapOverlay overlay, List<PointLatLng> points);
    }
}
