using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolansCommLib.NetworkInfoHolder;
using VolansCommLib.NodeConnector;

namespace VolansCommLib.Nodes.NodeClasses
{
    internal class VolansHYINode : IVirtualNode
    {

        byte PackageCounter = 0;
        public byte TakımId = 0;

        void IncreasePackageCounter()
        {
            if (PackageCounter != 255)
            {
                PackageCounter++;
            }
            else
            {
                PackageCounter = 0;
            }
        }

        public INetworkInfoHolder NetworkInfo { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Dictionary<byte, Action<byte[]>> AcceptedPackageDict => throw new NotImplementedException();

        public INodeConnector ConnectedNodeConnector { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public byte[] GetSensorInfo()
        {
            throw new NotImplementedException();
        }

        public void SendMessage()
        {
            
        }

        public void SendMessage(byte[] message)
        {
            
            try
            {


            }
            catch (Exception e) 
            {
                
            }
            }

        public void VerifyConnection()
        {
            throw new NotImplementedException();
        }
    }
}
