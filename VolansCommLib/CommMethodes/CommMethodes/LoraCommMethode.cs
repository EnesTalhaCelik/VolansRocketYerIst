using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolansCommLib.CommMethodes.Communicators;
using VolansCommLib.NodeConnector;
using VolansCommLib.NodeConnector.LoraNodeConnector;

namespace VolansCommLib.CommMethodes.CommMethodes
{
    internal class LoraCommMethode : ICommMethode
    {
        public INodeConnector NodeConnector { get; set; } = new LoraE32NodeConnector();
        public ICommunicator Communicator { get; set; } = new SerialPortCommunicator();

        public void InitCommunicator()
        {
            this.Communicator = new SerialPortCommunicator(); 
            


        }

      

        public void OnMessageRecive()
        {


        }

        public void SendMessage()
        {
            throw new NotImplementedException();
        }
    }
}
