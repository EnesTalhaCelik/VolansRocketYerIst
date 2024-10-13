using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolansCommLib.NodeConnector;

namespace VolansCommLib.Nodes
{
    internal interface IVirtualNode:INode
    {
        void SendMessage(byte[] message);
        void VerifyConnection();
        INodeConnector ConnectedNodeConnector { get; set; }
        byte[] GetSensorInfo();

    }
}
