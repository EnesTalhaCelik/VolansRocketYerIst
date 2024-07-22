using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeknofestVeriler;
using VolansYerIstasyonu.UserControls;

namespace VolansYerIstasyonu
{
    internal class UcusSim
    {
        public static double enlemKayma = 0.00002;
        public static double boylamKayma = 0.00001;


        public static double yukseklikHesap(double zaman)
        {

            double temp = -6 * (zaman * (zaman - 40));
            uc_AnaSayfa.Roket.RoketGpsIrtifa = (float)temp;
            return temp;
        }

        public static double hizHesap(double zaman)
        {

            double temp = -12 * (zaman) + 240;
            
            return temp;
        }
        public static void enlemHesapla(double zaman)
        {
            uc_AnaSayfa.Roket.RoketEnlem += (float)(zaman * enlemKayma);

        }
        public static void boylamHesapla(double zaman)
        {
            uc_AnaSayfa.Roket.RoketBoylam += (float)(zaman * zaman * boylamKayma);

        }
        public static void gorevYukuEnlemHesapla(double zaman)
        {
            uc_AnaSayfa.Roket.GorevYukuEnlem += (float)(zaman * enlemKayma * 2/4);

        }
        public static void gorevYukuBoylamHesapla(double zaman)
        {
            uc_AnaSayfa.Roket.GorevYukuBoylam += (float)(zaman * zaman * boylamKayma * 0.4/4);

        }

    }
}
