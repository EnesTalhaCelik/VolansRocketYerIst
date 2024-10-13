using Haberlesme.CommunicationMethodes.SerialCommunication.Devices.Lora.LoraE32;
using Haberlesme.CommunicationMethodes.SerialCommunication.SerialComm;
using Haberlesme.Nodes;
using Haberlesme.Nodes.HQNodes;
using Haberlesme.Nodes.VirtualNodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Haberlesme.PackageTypes; 
namespace Haberlesme.CommunicationMethodes.SerialCommunication.Devices
{
    internal interface ISerialCommDevice
    {
        
        List<IVirtualNode> ConnectedVirtualNodes { get; set; }
        IHQNode HQNode { get; set; }
        //byte[] CreatePackage(byte[] dataToSend,ISerialCommDevice ReciverDevice);
        ISerialCommunication SerialPortHandler { get; set; }
        string SendPackageToVirtualNode(IPackage packageToSend);
        void ConnectToVirtualNode();
        void DisconnectFromVirtualNode();
        void InitDevice();

    }
}
