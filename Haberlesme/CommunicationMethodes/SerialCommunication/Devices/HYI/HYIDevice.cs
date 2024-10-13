using Haberlesme.CommunicationMethodes.SerialCommunication.SerialComm;
using Haberlesme.Nodes;
using Haberlesme.Nodes.HQNodes;
using Haberlesme.Nodes.VirtualNodes;
using Haberlesme.PackageTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haberlesme.CommunicationMethodes.SerialCommunication.Devices.HYI
{
    internal class HYIDevice : IHYIDevice
    {
        public IHQNode HQNode { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ISerialCommunication SerialPortHandler { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        List<IVirtualNode> ISerialCommDevice.ConnectedVirtualNodes { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public string SendPackageToNode(IPackage packageToSend)
        {
            


            return "success";
        }
    }
}
