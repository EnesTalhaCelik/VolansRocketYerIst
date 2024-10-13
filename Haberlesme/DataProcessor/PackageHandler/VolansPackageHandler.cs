using Haberlesme.Nodes;
using Haberlesme.PackageTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Haberlesme.DataProcessor.PackageHandler
{
    internal class VolansPackageHandler
    {
        INode ConnectedNode;



        IPackage FindPackage(byte[] dataChunk, byte[] startCode,List<INode> NodeSearchList)
        {
            
            byte[] searchArray = new byte[startCode.Length];
            byte AddressCode,PackageCode;
            for (int i = 0; i <= dataChunk.Length - startCode.Length; i++)
            {
                Array.Copy(sourceArray: dataChunk,sourceIndex: i,destinationArray: searchArray,destinationIndex:0,startCode.Length);

                if(searchArray==startCode)
                {
                    //bu noktada paket tespit edildi.
                    AddressCode = dataChunk[i + startCode.Length];
                    PackageCode = dataChunk[i + startCode.Length+1];
                    foreach (INode node in NodeSearchList) 
                    {

                        if(AddressCode== node.NetworkInfo.NetworkAddressCode)
                        {
                            //bu noktada bağlı node bulundu

                            foreach(IPackage package in node.Packages)
                            {
                                if (package.Identifier.Equals(PackageCode))
                                {
                                    if(package.PackageLength > dataChunk.Length - i - 1)
                                    {
                                        //kötü paket hatası
                                        throw new NotImplementedException();
                                    }

                                    IPackage package1 = package;
                                    //kalan kısım için alacaklı fonksiyonu çağır?
                                    //şuan bu büyük ihtimalle referans üzerine çalışıyr bunu ayarla
                                    Array.Copy(sourceArray:dataChunk, sourceIndex: i+2,destinationArray:package1.Data,destinationIndex:0,package1.PackageLength);
                                    node.Packages[0] = package1;
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
