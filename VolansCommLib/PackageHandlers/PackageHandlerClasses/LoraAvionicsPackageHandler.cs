using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolansCommLib.NodeConnector;
using VolansCommLib.Nodes;
using VolansCommLib.Methodes;

namespace VolansCommLib.PackageHandlers.PackageHandlerClasses
{
    internal class LoraAvionicsPackageHandler : IPackageHandler
    {
        public INodeConnector NodeConnector { get; set ; }

        public byte[] CreatePackage(INode ReciverNode, byte[] Package)
        {

            byte[] buffer = new byte[Package.Length+5];
            byte[] NetworkInfo = ReciverNode.NetworkInfo.ReturnNetworkInfo();
            Array.Copy(NetworkInfo, buffer, 5);
            Array.Copy(Package, 5, buffer, 0, Package.Length);
            return buffer;
        }

        public INode SearchNode(byte NodeAddress)
        {
            foreach(INode node in NodeConnector.ConnectedNodes)
            {
                if(NodeAddress == node.NetworkInfo.NetworkAddressCode)
                {
                    return node;
                }
            }

            //throw
            throw new NotImplementedException();
        }

        public void SearchPackage(byte[] ArrayToSearch)
        {

           int index = Methodes.SearchMethodes.SearchByteArray(ArrayToSearch, NodeConnector.mainNode.NetworkInfo.StartCode);
            if (index != -1)
            {
                
                INode node = SearchNode(ArrayToSearch[index + NodeConnector.mainNode.NetworkInfo.StartCode.Length]);
                node.AcceptedPackageDict[ArrayToSearch[index + NodeConnector.mainNode.NetworkInfo.StartCode.Length + 1]](ArrayToSearch);
                return;
            }
            throw new NotImplementedException();


        }
        

        
    }
}
