using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeknofestVeriler;

namespace VolansYerIstasyonu
{
    internal class UcusSim
    {
        public static double enlemKayma = 0.00002;
        public static double boylamKayma = 0.00001;


        public static double yukseklikHesap(double zaman)
        {

            double temp = -2.1 * (zaman * (zaman - 100));

            return temp;
        }

        public static double hizHesap(double zaman)
        {

            double temp = -4.2 * (zaman) + 210;

            return temp;
        }
        public static void enlemHesapla(double zaman)
        {
            TeknofestVeriler.Veriler.RoketEnlem += (float)(zaman * enlemKayma);

        }
        public static void boylamHesapla(double zaman)
        {
            TeknofestVeriler.Veriler.RoketBoylam += (float)(zaman * zaman * boylamKayma);

        }
        public static void gorevYukuEnlemHesapla(double zaman)
        {
            TeknofestVeriler.Veriler.GorevYukuEnlem += (float)(zaman * enlemKayma * 2);

        }
        public static void gorevYukuBoylamHesapla(double zaman)
        {
            TeknofestVeriler.Veriler.GorevYukuBoylam += (float)(zaman * zaman * boylamKayma * 0.4);

        }

    }
}
