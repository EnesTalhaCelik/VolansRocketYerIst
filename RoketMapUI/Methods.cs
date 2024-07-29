using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoketMapUI
{
    public class Methods
    {
        public static void InitMapUI(string anaAvColor,string gorevYukuColor,string yedekAvColor,string anaAvPath,
            string gorevYukuPath,string yedekAvPath)
        {
            UIAnaAv.InitMap(color: anaAvColor, someIconPath: anaAvPath);
            UIGorevYuku.InitMap(color: gorevYukuColor, someIconPath: gorevYukuPath);
            UIYedekAv.InitMap(color: yedekAvColor, someIconPath: yedekAvPath);
        }

    }
}
