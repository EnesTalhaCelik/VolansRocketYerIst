using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SeriHaberlesme
{

    internal static class paketOlusturHYI
    {
        public static SerialPort hyiSerialPort = new SerialPort();
        public static byte[] yazilacakVeriler = new byte[78];

        private static byte checksumHesapla()
        {
            int check_sum = 0;
            for (int i = 4; i < 75; i++)
            {
                check_sum += yazilacakVeriler[i];
            }
            int sonuc = check_sum % 256;

            return ((byte)sonuc);
        }

        public static void PaketOlustur(byte paketsayac, float basincIrtifa, float roketGpsIrtifa , float roketEnlem , float roketBoylam, float gorevyukuGpsIrtifa, float gorevyukuEnlem , float gorevyukuBoylam, float jiroskopX, float jiroskopY, float jiroskopZ, float ivmeX, float ivmeY, float ivmeZ, float aci,byte durum)
        {
            //Gönderilecek 6. veri
            if (paketsayac != 255)
            {
                yazilacakVeriler[5] = paketsayac;
                paketsayac += 1;
            }
            else
            {
                //paket sayacı 255 e eşitter demektir ve sıfırlanması gerekir.
                yazilacakVeriler[5] = paketsayac;
                paketsayac = 0;

            }
            //Gönderilecek 7. / 10. veri
            byte[] geciciBuffer = BitConverter.GetBytes(basincIrtifa);
            byteArrayToYazilacakVeriler(geciciBuffer, 6);
            // Gönderilecek 11. / 14. veri
            geciciBuffer = BitConverter.GetBytes(roketGpsIrtifa);
            byteArrayToYazilacakVeriler(geciciBuffer, 10);
            // Gönderilecek 15. / 18. veri
            geciciBuffer = BitConverter.GetBytes(roketEnlem);
            byteArrayToYazilacakVeriler(geciciBuffer, 14);
            // Gönderilecek 19. / 22. veri
            geciciBuffer = BitConverter.GetBytes(roketBoylam);
            byteArrayToYazilacakVeriler(geciciBuffer, 18);
            // Gönderilecek 23. / 26. veri
            geciciBuffer = BitConverter.GetBytes(gorevyukuGpsIrtifa);
            byteArrayToYazilacakVeriler(geciciBuffer, 22);
            // Gönderilecek 27. / 30 veri
            geciciBuffer = BitConverter.GetBytes(gorevyukuEnlem);
            byteArrayToYazilacakVeriler(geciciBuffer, 26);
            // Gönderilecek 31. / 34. veri
            geciciBuffer = BitConverter.GetBytes(gorevyukuBoylam);
            byteArrayToYazilacakVeriler(geciciBuffer, 30);
            //bu noktadan sonraki 12 veri kademeye ait veriler olduğunan 0 girilir
            //init fonksiyonunda 35. veriden 46. veriye kadar 0x00 yazdırıldı
            // Gönderilecek 47. / 50. veri
            geciciBuffer = BitConverter.GetBytes(jiroskopX);
            byteArrayToYazilacakVeriler(geciciBuffer, 46);
            // Gönderilecek 51. / 54. veri
            geciciBuffer = BitConverter.GetBytes(jiroskopY);
            byteArrayToYazilacakVeriler(geciciBuffer, 50);
            // Gönderilecek 55. / 58. veri
            geciciBuffer = BitConverter.GetBytes(jiroskopZ);
            byteArrayToYazilacakVeriler(geciciBuffer, 54);
            // Gönderilecek 59. / 62. veri
            geciciBuffer = BitConverter.GetBytes(ivmeX);
            byteArrayToYazilacakVeriler(geciciBuffer, 58);
            // Gönderilecek 63. / 66. veri
            geciciBuffer = BitConverter.GetBytes(ivmeY);
            byteArrayToYazilacakVeriler(geciciBuffer, 62);
            // Gönderilecek 67. / 70. veri
            geciciBuffer = BitConverter.GetBytes(ivmeZ);
            byteArrayToYazilacakVeriler(geciciBuffer, 66);
            // Gönderilecek 71. / 74. veri
            geciciBuffer = BitConverter.GetBytes(aci);
            byteArrayToYazilacakVeriler(geciciBuffer, 70);
            //bundan sonra göndereceğimiz veriler için byte çevirme işlemi yapmamıza gerek yok
            yazilacakVeriler[74] = durum;
            yazilacakVeriler[75] = checksumHesapla();

        }


        static void byteArrayToYazilacakVeriler(byte[] verilerByte, int yazilacakIndex)
        {
            for (int i = 0; i < 4; i++)
            {
                yazilacakVeriler[yazilacakIndex + i] = verilerByte[i];

            }

        }

        public static void InitYazilacakVeriler(byte takimID)
        {
            paketOlusturHYI.yazilacakVeriler[0] = 0xFF;
            paketOlusturHYI.yazilacakVeriler[1] = 0xFF;
            paketOlusturHYI.yazilacakVeriler[2] = 0x54;
            paketOlusturHYI.yazilacakVeriler[3] = 0x52;
            paketOlusturHYI.yazilacakVeriler[4] = ((byte)takimID);

            //35 . veriden 46. veriye kadar 0x00 yazdırıyoruz
            for (int i = 34; i < 46; i++)
            {
                paketOlusturHYI.yazilacakVeriler[i] = 0x00;
            }
            paketOlusturHYI.yazilacakVeriler[76] = 0x0D;
            paketOlusturHYI.yazilacakVeriler[77] = 0x0A;
        }

    }



}
