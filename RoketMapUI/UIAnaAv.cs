using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace RoketMapUI
{
    public class UIAnaAv : MapUIBase
    {
        internal static void InitMap(string color, string someIconPath)
        {
            pathColor = color;
            mapIconPath = someIconPath;
        }

    }
}