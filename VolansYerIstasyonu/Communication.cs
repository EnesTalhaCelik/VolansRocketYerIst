using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolansYerIstasyonu
{
    internal class Communication
    {
        public static void veriAykila(SerialPort veriPort, Roket someRoket)
        {
            //başlangıç key control
            if (veriPort.ReadByte() != someRoket.BaslangicKey)
            {
                return;
            }
            //burda gelen mesajın hangi kısımdan geldiğini kontrol
            //etmeliyiz.

            byte[] buffer = new byte[veriPort.BytesToRead];
            veriPort.Read(buffer, 0, buffer.Length);


            if ( buffer[0] == someRoket.TanimlayiciGorevYuku)
            {
                

                //caseleri sonra tekrar ayarla
                //roketten geliyorsa
                someRoket.GorevYukuPaketSayac = buffer[1];
                someRoket.GorevYukuBasinc = BitConverter.ToSingle(buffer, 2);  //ileride sadece basıncı alıp basınç irtifayı burada hesaplatabiliriz
                someRoket.GorevYukuGpsIrtifa = BitConverter.ToSingle(buffer, 6);
                someRoket.GorevYukuEnlem = BitConverter.ToSingle(buffer, 10);
                someRoket.GorevYukuBoylam = BitConverter.ToSingle(buffer, 14);
                someRoket.GorevYukuSicaklik = BitConverter.ToSingle(buffer, 18);
                someRoket.GorevYukuNem = BitConverter.ToSingle(buffer, 22);
                someRoket.GorevYukuBasinc = BitConverter.ToSingle(buffer, 26);
                someRoket.CPM1 = buffer[30];
                someRoket.CrcGorevYuku = buffer[31];
            }
              else if (buffer[0] == someRoket.TanimlayiciAnaAv)
            {
                //caseleri sonra tekrar ayarla
                //roketten geliyorsa
                someRoket.PaketSayacAnaAv = buffer[1];
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
                someRoket.CrcRoket = buffer[51];
            }
            else if (buffer[0] == someRoket.TanimlayiciYedek){
                //caseleri sonra tekrar ayarla
                //roketten geliyorsa
                someRoket.PaketSayacYedek = buffer[1];
                someRoket.YedekBasinc1 = BitConverter.ToSingle(buffer, 2);  //ileride sadece basıncı alıp basınç irtifayı burada hesaplatabiliriz
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
                someRoket.CrcYedek = buffer[51];
            }
            else if (buffer[0] == someRoket.TanimlayiciStatus)
            {
                //yanıt beklenen işler, zaman aşımı vs fuck this shit.
                //1 bahsi geçen göörev iş nedir, hashcode? burada bir hata kodu da olabilir! debug! rokette meytdana gelen bir sorun!
                //2 gerçekleştirilebildi mi (örneğin crc kodu doğru mu) 
                

            }









        }


        }
}
