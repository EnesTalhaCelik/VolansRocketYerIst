using Haberlesme.CommunicationMethodes.SerialCommunication.Devices;
using Haberlesme.Nodes;
using Haberlesme.PackageTypes;
using Haberlesme.Paketler;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haberlesme.CommunicationMethodes.SerialCommunication.SerialComm.SerialCommHandler
{
    internal class LoraSerialCommHandler : ISerialCommunication
    {
        //200 ms de bir veri alacak

        ISerialCommDevice ConnectedDevice { get; set; }
        public SerialPort SerialPort { get ; set; }
        public Queue<byte[]> IncomingData { get; set; } = new Queue<byte[]>();
        public Queue<byte[]> DataSendQueue { get; set; }

        public void OnSerialRecive()
        {
            //biraz beklemeli , asenkron yapmalısın
            //!!ya vri bombardımanı olursa 
            System.Threading.Thread.Sleep(1);
            byte[] buffer = new byte[SerialPort.BytesToRead];

            //revisit !!
            //lora param
            //TRY Catch exceptions!!!
            FindPackage(buffer, ConnectedDevice.HQNode.AcceptedStartCode, ConnectedDevice.ConnectedVirtualNodes);

            throw new NotImplementedException();
        }
        
        

        public void SendPackage(byte[] paket)
        {
            throw new NotImplementedException();
        }



        IPackage FindPackage(byte[] dataChunk, byte[] startCode, List<INode> NodeSearchList)
        {

            byte[] searchArray = new byte[startCode.Length];
            byte AddressCode, PackageCode;
            for (int i = 0; i <= dataChunk.Length - startCode.Length; i++)
            {
                Array.Copy(sourceArray: dataChunk, sourceIndex: i, destinationArray: searchArray, destinationIndex: 0, startCode.Length);

                if (searchArray == startCode)
                {
                    //bu noktada paket tespit edildi.
                    AddressCode = dataChunk[i + startCode.Length];
                    PackageCode = dataChunk[i + startCode.Length + 1];
                    foreach (INode node in NodeSearchList)
                    {

                        if (AddressCode == node.NetworkInfo.NetworkAddressCode)
                        {
                            //bu noktada bağlı node bulundu

                            foreach (IPackage package in node.Packages)
                            {
                                if (package.Identifier.Equals(PackageCode))
                                {
                                    if (package.PackageLength > dataChunk.Length - i - 1)
                                    {
                                        //kötü paket hatası
                                        throw new NotImplementedException();
                                    }

                                    IPackage package1 = package;
                                    //kalan kısım için alacaklı fonksiyonu çağır?
                                    //şuan bu büyük ihtimalle referans üzerine çalışıyr bunu ayarla
                                    Array.Copy(sourceArray: dataChunk, sourceIndex: i + 2, destinationArray: package1.Data, destinationIndex: 0, package1.PackageLength);
                                    return package1;

                                }
                            }
                            //Paket kodu bulunamadı hatası
                            throw new NotImplementedException();

                        }



                    }
                    //adresler uyuşmuyor hatası
                    throw new NotImplementedException();
                }
            }
            //paket tespit edilemedi hatası
            throw new NotImplementedException();

        }




    }
}
