using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolansCommLib.Nodes;
using VolansCommLib.PackageHandlers;
using VolansCommLib.CommMethodes;
using VolansCommLib.NetworkInfoHolder;
namespace VolansCommLib.NodeConnector
{
    internal interface INodeConnector
    {
        List<IVirtualNode> ConnectedNodes { get; set; }
        IPackageHandler PackageHandler { get;  }
        ICommMethode CommMethode { get; set; }

        IMainNode mainNode { get;  }

        string NodeConnectorName { get; set; }
        //yeni noda a bağlanınca listede var olup oladığını kontrol ettirt.
        void ConnectToANode(IVirtualNode node);
        void DisconnectNode();
        void SendMessageToNode(byte[] messageToSend,IVirtualNode destinationNode);
        void SendMessageToNode(byte[] messageToSend, INetworkInfoHolder nodeNetworkInfo);

        //noddakini sil tryConnect ?
        

        //void LookForNodes(); auto connect!!! making connection to rocket easy

    }
}
