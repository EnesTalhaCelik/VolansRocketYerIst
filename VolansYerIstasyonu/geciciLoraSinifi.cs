using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Windows.Forms;
using DocumentFormat.OpenXml.InkML;
using static GMap.NET.Entity.OpenStreetMapGeocodeEntity;
namespace VolansYerIstasyonu
{
    internal class geciciLoraSinifi
    {
        /*
        //buradaki kodlar daha sonra lora controll dll ine çevirilip eklenecek
        //gerekli metotlar : get param (çekerken baud rate i 9600 e çekmen lazım)
        //set param
        //send message
        
            fixed message
            broadcast message

        recive message
        mod switchleri ?
        şimdilik yeterli daha devamı gelecek
        */
        static void getLoraParams()
        {

        }


        static void setLoraParams(SerialPort LoraSP, int baudRate, byte channel, int address, int addh, int AirDataRate,Parity parity)
        {

        }

        //burada metotları farklı durumlar için overlaod ediyorum.
        //string gönder
        
        //int gönder
        static void sendFixedMessage(SerialPort LoraSP, int message, byte address, byte channel, byte header)
        {
            try
            {
                byte[] gonderilecekMesaj = BitConverter.GetBytes(message);
                byte[] mesajlar = { header, address, channel };
                LoraSP.Write(mesajlar,0,3);
                LoraSP.Write(gonderilecekMesaj, 0, 4);
            }
            catch (Exception x)
            {
                Console.Write("Bir Hata meydana geldi! : ");
                Console.WriteLine(x);
            }
        }
        //int array gönder
        static void sendFixedMessage(SerialPort LoraSP, int[] message, byte address, byte channel, byte header)
        {
            try
            {
                byte[] gonderilecekMesaj = new byte[message.Length];
                for (int i = 0; i < message.Length; i++)
                {
                    gonderilecekMesaj[i] = ((byte)message[i]);

                }
                byte[] mesajlar = { header, address, channel  };
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
        static void sendFixedMessage(SerialPort LoraSP, float message, byte address, byte channel, byte header)
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

        static string loraRecivePackage(SerialPort LoraSP)
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
        static void paketAyikla(string gelenMesaj, string[] AyrilacakYer,char ayiklanacakKarakter )
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
        static int paketKontrol( byte[] gelenMesaj)
        {
            try
            {
                
                if (gelenMesaj[0] == 0x7B && gelenMesaj[gelenMesaj.Length - 1] == 0x7D && (gelenMesaj[1] != 0x7B ))
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
