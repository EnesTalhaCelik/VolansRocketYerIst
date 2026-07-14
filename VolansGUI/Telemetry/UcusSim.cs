using System;

namespace VolansGUI.Telemetry
{
    /// <summary>
    /// Basit uçuş simülasyonu. İrtifa/hız için parabol/lineer profil, enlem/boylam
    /// için birikimli kayma üretir. Verilen Roket nesnesini günceller.
    /// </summary>
    public static class UcusSim
    {
        public const double EnlemKayma = 0.00002;
        public const double BoylamKayma = 0.00001;

        /// <summary>t·(t-40) parabolü: 0..40 sn arası pozitif irtifa.</summary>
        public static double YukseklikHesap(double zaman) => -6 * (zaman * (zaman - 40));

        public static double HizHesap(double zaman) => -12 * zaman + 240;

        public static void RoketiGuncelle(Roket roket, double zaman)
        {
            roket.RoketGpsIrtifa = (float)YukseklikHesap(zaman);
            roket.RoketEnlem += zaman * EnlemKayma;
            roket.RoketBoylam += zaman * zaman * BoylamKayma;
            roket.GorevYukuEnlem += zaman * EnlemKayma * 2 / 4;
            roket.GorevYukuBoylam += zaman * zaman * BoylamKayma * 0.4 / 4;
        }
    }
}
