using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TeknofestVeriler
{
    public static class Veriler
    {
        private static float jiroskopX = 1f;
        private static float jiroskopY = 2f;
        private static float jiroskopZ = 3f;
        private static float ivmeX = 1.2f;
        private static float ivmeY = 1.3f;
        private static float ivmeZ = 9.77779f;
        private static float aci = 98.85f;
        private static float gorevYukuBoylam = 27;
        private static float gorevYukuEnlem = 15;
        private static float gorevYukuGpsIrtifa = 1000;
        private static float roketBoylam = 31.524978876f;
        private static float roketEnlem = 40.716131152f;
        private static float roketGpsIrtifa = 3000; //gps verisinden gelen irtifa değeridir.
        private static float basincIrtifa = 2999;   //basınç sensöründen gelen irtifa değeridir
        private static float kademeBoylam = 0;
        private static float kademeEnlem = 0;
        private static float kademeGpsIrtifa = 0;
        private static byte durum = 1;
        private static int crc;
        private static byte takimID = 7;
        private static byte paketSayac;


        public static float JiroskopX   
        {
            get { return jiroskopX; }   
            set { jiroskopX = value; }  
        }
        public static float JiroskopY
        {
            get { return jiroskopY; }
            set { jiroskopY = value; }
        }
        public static float JiroskopZ
        {
            get { return jiroskopZ; }
            set { jiroskopZ = value; }
        }
        public static float IvmeX
        {
            get { return ivmeX; }
            set { ivmeX = value; }
        }
        public static float IvmeY
        {
            get { return ivmeY; }
            set { ivmeY = value; }
        }
        public static float IvmeZ
        {
            get { return ivmeZ; }
            set { ivmeZ = value; }
        }
        public static float Aci
        {
            get { return aci; }
            set { aci = value; }
        }
        public static float GorevYukuBoylam
        {
            get { return gorevYukuBoylam; }
            set { gorevYukuBoylam = value; }
        }
        public static float GorevYukuEnlem
        {
            get { return gorevYukuEnlem; }
            set { gorevYukuEnlem = value; }
        }
        public static float GorevYukuGpsIrtifa
        {
            get { return gorevYukuGpsIrtifa; }
            set { gorevYukuGpsIrtifa = value; }
        }
        public static float RoketBoylam
        {
            get { return roketBoylam; }
            set { roketBoylam = value; }
        }
        public static float RoketEnlem
        {
            get { return roketEnlem; }
            set { roketEnlem= value; }
        }
        public static float RoketGpsIrtifa
        {
            get { return roketGpsIrtifa; }
            set { roketGpsIrtifa = value; }
        }
        public static float BasincIrtifa
        {
            get { return basincIrtifa; }
            set { basincIrtifa = value; }
        }
        public static float KademeBoylam
        {
            get { return kademeBoylam; }
            set { kademeBoylam = value; }
        }
        public static float KademeEnlem
        {
            get { return kademeEnlem; }
            set { kademeEnlem = value; }
        }
        public static float KademeGpsIrtifa
        {
            get { return kademeGpsIrtifa; }
            set { kademeGpsIrtifa = value; }
        }
        public static byte Durum
        {
            get { return durum; }
            set { durum = value; }
        }
        public static int CRC
        {
            get { return crc; }
            set { crc = value; }
        }
        public static byte TakimID
        {
            get { return takimID; }
            set { takimID = value; }
        }
        public static byte PaketSayac
        {
            get { return paketSayac; }
            set { paketSayac = value; }
        }

    }

}
