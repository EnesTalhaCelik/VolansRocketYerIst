using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using VolansCommLib.NetworkInfoHolder;
using VolansCommLib.NetworkInfoHolder.NetworkInfoHolders;
using VolansCommLib.NodeConnector;

namespace VolansCommLib.Nodes.NodeClasses
{
    internal class VolansAvionicsNode : IVirtualNode
    {
        const int SENSOR_PACKAGE_LENGHT = 5;
        const byte SENSOR_PACKAGE_CODE = 24;
        const byte CONNECTION_VERIFICATION_CODE = 32;  

        struct LastRecivedSensorValues
        {
            float AltitudePressure;
            float AltitudeGPS;
            float RocketLatitude;
            float RocketLongitude;
            float GyroscopeX;
            float GyroscopeY;
            float GyroscopeZ;
            float AccelerationX;
            float AccelerationY;
            float AccelerationZ;
            float Angle;
            byte ParachuteState;
        }

        LastRecivedSensorValues LastRecivedSensorValue = new LastRecivedSensorValues();
         public Dictionary<byte, Action<byte[]>> AcceptedPackageDict { get; } = new Dictionary<byte, Action<byte[]>>();


        public VolansAvionicsNode()
        {


            InitAcceptedPackageDict();
        }

        public void InitAcceptedPackageDict()
        {
            AcceptedPackageDict.Add(SENSOR_PACKAGE_CODE, DataRecived_SensorPackage);
        }

        public void DataRecived_SensorPackage(byte[] sensorPackage)
        {
            if(sensorPackage.Length < SENSOR_PACKAGE_LENGHT)
            {
                throw new NotImplementedException();
            }
            byte[] buffer = sensorPackage.Take(SENSOR_PACKAGE_LENGHT).ToArray();
            LastRecivedSensorValue = ByteArrayToStruct<LastRecivedSensorValues>(buffer);

        }
        public void DataRecived_PingTestSuccessfull()
        {
        //DO SHIT!
        }


        public INetworkInfoHolder NetworkInfo { get; set ; } = new LoraNetworkInfoHolder();




        public INodeConnector ConnectedNodeConnector { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void VerifyConnection()
        { 
            throw new NotImplementedException();

        }

        public void SendMessage(byte[] message)
        {
            throw new NotImplementedException();
        }


        private static T ByteArrayToStruct<T>(byte[] byteArray) where T : struct
        {
            GCHandle handle = GCHandle.Alloc(byteArray, GCHandleType.Pinned);
            try
            {
                IntPtr ptr = handle.AddrOfPinnedObject();
                return Marshal.PtrToStructure<T>(ptr);
            }
            finally
            {
                handle.Free();
            }
        }

    }

    

}
