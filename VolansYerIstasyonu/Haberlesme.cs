using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolansYerIstasyonu
{
    internal class Haberlesme
    {
        //haberleşme sisteminde paket bitiş indikatörü kaldırıılacak
        //paket başlangıç indikatörü ve adres indikatörü aynen kalacak
        
        //Haberleşmeyi sağlamak için mutlaka bir başlangıç anahtarı belirlenmesi gerekiyor. 

        byte baslangicAnahtari;

        public Haberlesme(byte baslangicAnahtari) { 
        
            
            this.baslangicAnahtari = baslangicAnahtari;

        
        }

        //Roketten gelen veriler HYI'ya gönderilme sırasına göre gelecek
        public void veriAykila(SerialPort veriPort,Roket someRoket)
        {


            if (veriPort.ReadByte() != baslangicAnahtari) {
                return;
            }
            //burda gelen mesajın hangi kısımdan geldiğini kontrol
            //etmeliyiz.

            byte[] buffer = new byte[veriPort.BytesToRead];
            veriPort.Read(buffer, 0, buffer.Length);

            switch (buffer[0])
            {
                case 0:
                    //caseleri sonra tekrar ayarla
                    //roketten geliyorsa
                    someRoket.PaketSayac = buffer[1];
                    someRoket.RoketBasinc = BitConverter.ToSingle(buffer, 2);  //ileride sadece basıncı alıp basınç irtifayı burada hesaplatabiliriz
                    someRoket.RoketGpsIrtifa = BitConverter.ToSingle(buffer, 6);
                    someRoket.RoketEnlem = BitConverter.ToSingle(buffer, 10);
                    someRoket.RoketBoylam = BitConverter.ToSingle(buffer, 14);
                    someRoket.JiroskopX = BitConverter.ToSingle(buffer, 18);
                    someRoket.JiroskopY = BitConverter.ToSingle(buffer, 22);
                    someRoket.JiroskopZ = BitConverter.ToSingle(buffer, 26);
                    someRoket.IvmeX = BitConverter.ToSingle(buffer, 30);
                    someRoket.IvmeY = BitConverter.ToSingle(buffer, 34);
                    someRoket.IvmeZ = BitConverter.ToSingle(buffer, 38);
                    someRoket.Aci = BitConverter.ToSingle(buffer, 42);
                    someRoket.Durum = buffer[46];
                    someRoket.RoketSicaklik = BitConverter.ToSingle(buffer, 47);
                    break;
                case 1:
                    //caseleri sonra tekrar ayarla
                    //roketten geliyorsa
                    someRoket.PaketSayacYedek = buffer[1];
                    someRoket.YedekBasinc1= BitConverter.ToSingle(buffer, 2);  //ileride sadece basıncı alıp basınç irtifayı burada hesaplatabiliriz
                    someRoket.RoketGpsIrtifaYedek = BitConverter.ToSingle(buffer, 6);
                    someRoket.RoketEnlemYedek = BitConverter.ToSingle(buffer, 10);
                    someRoket.RoketBoylamYedek = BitConverter.ToSingle(buffer, 14);
                    someRoket.JiroskopXYedek = BitConverter.ToSingle(buffer, 18);
                    someRoket.JiroskopYYedek = BitConverter.ToSingle(buffer, 22);
                    someRoket.JiroskopZYedek = BitConverter.ToSingle(buffer, 26);
                    someRoket.IvmeXYedek = BitConverter.ToSingle(buffer, 30);
                    someRoket.IvmeYYedek = BitConverter.ToSingle(buffer, 34);
                    someRoket.IvmeZYedek = BitConverter.ToSingle(buffer, 38);
                    someRoket.AciYedek = BitConverter.ToSingle(buffer, 42);
                    someRoket.Durum = buffer[46];
                    someRoket.YedekSicaklik1 = BitConverter.ToSingle(buffer, 47);
                    break;
                case 2:
                    //caseleri sonra tekrar ayarla
                    //roketten geliyorsa
                    //someRoket.PaketSayacYedek = buffer[1];
                    someRoket.GorevYukuBasinc = BitConverter.ToSingle(buffer, 2);  //ileride sadece basıncı alıp basınç irtifayı burada hesaplatabiliriz
                    someRoket.GorevYukuGpsIrtifa = BitConverter.ToSingle(buffer, 6);
                    someRoket.GorevYukuEnlem = BitConverter.ToSingle(buffer, 10);
                    someRoket.GorevYukuBoylam = BitConverter.ToSingle(buffer, 14);
                    someRoket.GorevYukuSicaklik = BitConverter.ToSingle(buffer, 18);
                    someRoket.GorevYukuNem = BitConverter.ToSingle(buffer, 22);
                    someRoket.GorevYukuBasinc = BitConverter.ToSingle(buffer, 26);
                    someRoket.CPM1 = buffer[30];
                    break;



            }


             void sendFixedMessage(SerialPort LoraSP, String message, byte address, byte channel, byte header)
            {
                try
                {
                    char[] gonderilecekMesajlar = message.ToCharArray();
                    byte[] mesajlar = { header, address, channel };
                    LoraSP.Write(mesajlar, 0, mesajlar.Length);
                    LoraSP.Write(gonderilecekMesajlar, 0, gonderilecekMesajlar.Length);
                }
                catch (Exception x)
                {
                    Console.Write("Bir Hata meydana geldi! : ");
                    Console.WriteLine(x);
                }
            }



            //int array gönder
            void sendFixedMessage2(SerialPort LoraSP, int[] message, byte address, byte channel, byte header)
            {
                try
                {
                    byte[] gonderilecekMesaj = new byte[message.Length];
                    for (int i = 0; i < message.Length; i++)
                    {
                        gonderilecekMesaj[i] = ((byte)message[i]);

                    }
                    byte[] mesajlar = { header, address, channel };
                    LoraSP.Write(mesajlar, 0, 3);
                    LoraSP.Write(gonderilecekMesaj, 0, gonderilecekMesaj.Length);
                }
                catch (Exception x)
                {
                    Console.Write("Bir Hata meydana geldi! : ");
                    Console.WriteLine(x);
                }
            }

            //fixed float gönder
            void sendFixedMessage3(SerialPort LoraSP, float message, byte address, byte channel, byte header)
            {
                try
                {
                    byte[] gonderilecekMesaj = BitConverter.GetBytes(message);
                    byte[] mesajlar = { header, address, channel };
                    LoraSP.Write(mesajlar, 0, 3);
                    LoraSP.Write(gonderilecekMesaj, 0, 4);
                }
                catch (Exception x)
                {
                    Console.Write("Bir Hata meydana geldi! : ");
                    Console.WriteLine(x);
                }
            }

             string loraRecivePackage(SerialPort LoraSP)
            {
                try
                {
                    String cevapKutusu = LoraSP.ReadLine();
                    return (cevapKutusu);
                }
                catch (Exception x)
                {

                    Console.Write("Bir Hata meydana geldi! : ");
                    Console.WriteLine(x);
                    return ("MESAJ ALINIRKEN BİR HATA MEYDANA GELDİ : " + x);

                }

            }
             void paketAyikla(string gelenMesaj, string[] AyrilacakYer, char ayiklanacakKarakter)
            {
                try
                {
                    AyrilacakYer = gelenMesaj.Split(ayiklanacakKarakter);
                }
                catch (Exception x)
                {
                    Console.Write("Bir Hata meydana geldi! : ");
                    Console.WriteLine(x);
                }

            }
           
            
            int paketKontrol(byte[] gelenMesaj)
            {
                try
                {

                    if (gelenMesaj[0] == 0x7B && gelenMesaj[gelenMesaj.Length - 1] == 0x7D && (gelenMesaj[1] != 0x7B))
                    {
                        byte girisKodu = gelenMesaj[1];
                        switch (girisKodu)
                        {
                            case 0x75:


                                //şuanda test yapılıyor.
                                TeknofestVeriler.Veriler.BasincIrtifa = BitConverter.ToSingle(gelenMesaj, 2);
                                TeknofestVeriler.Veriler.JiroskopX = BitConverter.ToSingle(gelenMesaj, 6);
                                TeknofestVeriler.Veriler.JiroskopY = BitConverter.ToSingle(gelenMesaj, 10);
                                TeknofestVeriler.Veriler.JiroskopZ = BitConverter.ToSingle(gelenMesaj, 14);



                                break;
                        }

                    }
                    return 1;
                }
                catch (Exception x)
                {
                    Console.Write("Bir Hata meydana geldi! : ");
                    Console.WriteLine(x);
                    return 0;
                }

            }







        }




    }
}
