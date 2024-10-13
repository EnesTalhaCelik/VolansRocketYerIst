using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolansCommLib.NetworkInfoHolder;
using VolansCommLib.NodeConnector;

namespace VolansCommLib.Nodes.NodeClasses
{
    internal class VolansPayloadNode : IVirtualNode  
    {
        public INetworkInfoHolder NetworkInfo { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Dictionary<byte, Action<byte[]>> AcceptedPackageDict => throw new NotImplementedException();

        public INodeConnector ConnectedNodeConnector { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public byte[] GetSensorInfo()
        {
            throw new NotImplementedException();
        }

        public void SendMessage(byte[] message)
        {
            throw new NotImplementedException();
        }

        public void VerifyConnection()
        {
            throw new NotImplementedException();
        }

        struct SensorValues
        {

            
            float AltitudeGPS;
            float RocketLatitude;
            float RocketLongitude;

            long CPM;
           

        }


    }
}
