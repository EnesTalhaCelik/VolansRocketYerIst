using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolansCommLib.NodeConnector;
using VolansCommLib.Nodes;

namespace VolansCommLib.PackageHandlers.PackageHandlerClasses
{

    internal class HYIPackageHandler : IPackageHandler
    {

        byte[] basePackage = new byte[78];

        public INodeConnector NodeConnector { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public byte[] CreatePackage(INode ReciverNode, byte[] Package)
        {
            throw new NotImplementedException();
        }

        public byte[] CreatePackage(byte[] message)
        {
            if(message.Length == 72)
            {
                Array.Copy(message, 0, basePackage, 4, 72);
                return basePackage;

            }
            throw new NotImplementedException();

        }

        public void InitPackageHandler()
        {

            basePackage[0] = 0xFF;
            basePackage[1] = 0xFF;
            basePackage[2] = 0x54;
            basePackage[3] = 0x52;

            basePackage[76] = 0x0D;
            basePackage[77] = 0X0A;

        }

        public INode SearchNode(byte NodeAddress)
        {
            throw new NotImplementedException();
        }

        //mantık olarak byte array döndürmesi gerekmiyor mu ya da başka bir aksiyon mu almalıydı? ne yazdığımı unuttum amk
        public void SearchPackage(byte[] ArrayToSearch)
        {
            if (ArrayToSearch.Length == 78 && ArrayToSearch[0]== 0XFF && ArrayToSearch[1] == 0XFF &&
                                              ArrayToSearch[2] == 0x54 && ArrayToSearch[3] == 0x52&&
                                              ArrayToSearch[76 ] == 0x0D && basePackage[77] == 0x0A)
            {

            }
        }

        
    }
}
