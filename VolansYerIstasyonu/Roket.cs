using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolansYerIstasyonu
{
    public class Roket
    {

        private byte baslangicKey;
        private byte tanimlayiciAnaAv, tanimlayiciKademe, tanimlayiciYedekAv, tanimlayiciGorevYuku;
        private byte tanimlayiciPingAnaAv, tanimlayiciPingGorevY, tanimlayiciPingYedekAv, tanimlayiciPingPong,
                     tanimlayiciGoreviBaslat,tanimlayiciStatus;

        private byte crc; //paket gönderirken bunu kullanacağız
        private byte paketSayac;
        private byte takimID = 7;
        private byte durum = 1; //yedek av. paraşütü açaca mı

        private float jiroskopX = 1f;
        private float jiroskopY = 2f;
        private float jiroskopZ = 3f;
        private float ivmeX = 1.2f;
        private float ivmeY = 1.3f;
        private float ivmeZ = 9.77779f;
        private float aci = 98.85f;
        private byte crcAnaAv;
        private byte paketSayacAnaAv;

        private float roketEnlem = 40.716582514101354f;
        private float roketGpsIrtifa = 3000;
        private float basincIrtifa = 2999;
        private float roketBoylam = 31.52471117735377f;
        private float roketNem, roketSicaklik, roketBasinc;

        private byte gorevYukuPaketSayac;
        private float gorevYukuBoylam = 27;
        private float gorevYukuEnlem = 15;
        private float gorevYukuGpsIrtifa = 1000;
        private byte crcGorevYuku;
        private byte CPM;
        private float gorevYukuNem, gorevYukuSicaklik, gorevYukuBasinc;

        private float jiroskopXYedek = 1f;
        private float jiroskopYYedek = 2f;
        private float jiroskopZYedek = 3f;
        private float ivmeXYedek = 1.2f;
        private float ivmeYYedek = 1.3f;
        private float ivmeZYedek = 9.77779f;
        private float aciYedek = 98.85f;
        private float roketBoylamYedek = 31.524978876f;
        private float roketEnlemYedek = 40.716131152f;
        private float roketGpsIrtifaYedek = 3000;
        private float basincIrtifaYedek = 2999;
        private int crcYedek;
        private byte paketSayacYedek;
        private float YedekNem, YedekSicaklik, YedekBasinc;

        private float kademeBoylam = 0;
        private float kademeEnlem = 0;
        private float kademeGpsIrtifa = 0;

      
        public Roket(byte takimID, byte tanimlayiciRoket,byte tanimlayiciKademe,byte tanimlayiciYedek,byte tanimlayiciGorevYuku)
        {
            this.takimID = takimID;
            this.tanimlayiciAnaAv = tanimlayiciRoket;
            this.tanimlayiciKademe = tanimlayiciKademe;
            this.tanimlayiciYedekAv = tanimlayiciYedek;
            this.tanimlayiciGorevYuku = tanimlayiciGorevYuku;        }
        public Roket(byte takimID, byte tanimlayiciRoket,  byte tanimlayiciYedek, byte tanimlayiciGorevYuku)
        {
            this.takimID = takimID;
            this.tanimlayiciAnaAv = tanimlayiciRoket;
            this.tanimlayiciYedekAv = tanimlayiciYedek;
            this.tanimlayiciGorevYuku = tanimlayiciGorevYuku;
        }

        public byte Crc { get => crc; set => crc = value; }
        public byte TakimID { get => takimID; set => takimID = value; }
        public byte Durum { get => durum; set => durum = value; }
        public float JiroskopX { get => jiroskopX; set => jiroskopX = value; }
        public float JiroskopY { get => jiroskopY; set => jiroskopY = value; }
        public float JiroskopZ { get => jiroskopZ; set => jiroskopZ = value; }
        public float IvmeX { get => ivmeX; set => ivmeX = value; }
        public float IvmeY { get => ivmeY; set => ivmeY = value; }
        public float IvmeZ { get => ivmeZ; set => ivmeZ = value; }
        public float Aci { get => aci; set => aci = value; }
        public byte CrcRoket { get => crcAnaAv; set => crcAnaAv = value; }
        public byte PaketSayac { get => paketSayac; set => paketSayac = value; }
        public float RoketEnlem { get => roketEnlem; set => roketEnlem = value; }
        public float RoketGpsIrtifa { get => roketGpsIrtifa; set => roketGpsIrtifa = value; }
        public float BasincIrtifa { get => basincIrtifa; set => basincIrtifa = value; }
        public float RoketBoylam { get => roketBoylam; set => roketBoylam = value; }
        public float RoketNem { get => roketNem; set => roketNem = value; }
        public float RoketSicaklik { get => roketSicaklik; set => roketSicaklik = value; }
        public float RoketBasinc { get => roketBasinc; set => roketBasinc = value; }
        public float GorevYukuBoylam { get => gorevYukuBoylam; set => gorevYukuBoylam = value; }
        public float GorevYukuEnlem { get => gorevYukuEnlem; set => gorevYukuEnlem = value; }
        public float GorevYukuGpsIrtifa { get => gorevYukuGpsIrtifa; set => gorevYukuGpsIrtifa = value; }
        public byte CrcGorevYuku { get => crcGorevYuku; set => crcGorevYuku = value; }
        public byte CPM1 { get => CPM; set => CPM = value; }
        public float GorevYukuNem { get => gorevYukuNem; set => gorevYukuNem = value; }
        public float GorevYukuSicaklik { get => gorevYukuSicaklik; set => gorevYukuSicaklik = value; }
        public float GorevYukuBasinc { get => gorevYukuBasinc; set => gorevYukuBasinc = value; }
        public float JiroskopXYedek { get => jiroskopXYedek; set => jiroskopXYedek = value; }
        public float JiroskopYYedek { get => jiroskopYYedek; set => jiroskopYYedek = value; }
        public float JiroskopZYedek { get => jiroskopZYedek; set => jiroskopZYedek = value; }
        public float IvmeXYedek { get => ivmeXYedek; set => ivmeXYedek = value; }
        public float IvmeYYedek { get => ivmeYYedek; set => ivmeYYedek = value; }
        public float IvmeZYedek { get => ivmeZYedek; set => ivmeZYedek = value; }
        public float AciYedek { get => aciYedek; set => aciYedek = value; }
        public float RoketBoylamYedek { get => roketBoylamYedek; set => roketBoylamYedek = value; }
        public float RoketEnlemYedek { get => roketEnlemYedek; set => roketEnlemYedek = value; }
        public float RoketGpsIrtifaYedek { get => roketGpsIrtifaYedek; set => roketGpsIrtifaYedek = value; }
        public float BasincIrtifaYedek { get => basincIrtifaYedek; set => basincIrtifaYedek = value; }
        public int CrcYedek { get => crcYedek; set => crcYedek = value; }
        public byte PaketSayacYedek { get => paketSayacYedek; set => paketSayacYedek = value; }
        public float KademeBoylam { get => kademeBoylam; set => kademeBoylam = value; }
        public float KademeEnlem { get => kademeEnlem; set => kademeEnlem = value; }
        public float KademeGpsIrtifa { get => kademeGpsIrtifa; set => kademeGpsIrtifa = value; }
        public byte TanimlayiciAnaAv{ get => tanimlayiciAnaAv; set => tanimlayiciAnaAv = value; }
        public byte TanimlayiciKademe { get => tanimlayiciKademe; set => tanimlayiciKademe = value; }
        public byte TanimlayiciYedek { get => tanimlayiciYedekAv; set => tanimlayiciYedekAv = value; }
        public byte TanimlayiciGorevYuku { get => tanimlayiciGorevYuku; set => tanimlayiciGorevYuku = value; }
        public float YedekNem1 { get => YedekNem; set => YedekNem = value; }
        public float YedekSicaklik1 { get => YedekSicaklik; set => YedekSicaklik = value; }
        public float YedekBasinc1 { get => YedekBasinc; set => YedekBasinc = value; }
        public byte BaslangicKey { get => baslangicKey; set => baslangicKey = value; }
        public byte PaketSayacAnaAv { get => paketSayacAnaAv; set => paketSayacAnaAv = value; }
        public byte GorevYukuPaketSayac { get => gorevYukuPaketSayac; set => gorevYukuPaketSayac = value; }
        public byte TanimlayiciPingAnaAv { get => tanimlayiciPingAnaAv; set => tanimlayiciPingAnaAv = value; }
        public byte TanimlayiciPingGorevY { get => tanimlayiciPingGorevY; set => tanimlayiciPingGorevY = value; }
        public byte TanimlayiciPingYedekAv { get => tanimlayiciPingYedekAv; set => tanimlayiciPingYedekAv = value; }
        public byte TanimlayiciPingPong { get => tanimlayiciPingPong; set => tanimlayiciPingPong = value; }
        public byte TanimlayiciGoreviBaslat { get => tanimlayiciGoreviBaslat; set => tanimlayiciGoreviBaslat = value; }
        public byte TanimlayiciStatus { get => tanimlayiciStatus; set => tanimlayiciStatus = value; }
    }
}
