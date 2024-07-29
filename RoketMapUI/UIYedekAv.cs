using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoketMapUI
{
    internal class UIYedekAv : MapUIBase
    {
        internal static void InitMap(string color, string someIconPath)
        {
            pathColor = color;
            mapIconPath = someIconPath;
        }
    }
}
