using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolansCommLib.CommMethodes.Communicators;
using VolansCommLib.NodeConnector;
using VolansCommLib.Nodes;

namespace VolansCommLib.CommMethodes
{
    internal interface ICommMethode
    {
        ICommunicator Communicator { get; set; } //her şey olabilir wifi, lora, serial comm etc
        INodeConnector NodeConnector { get; set; }
        void SendMessage();
        void OnMessageRecive();
        void InitCommunicator();


    }
}
