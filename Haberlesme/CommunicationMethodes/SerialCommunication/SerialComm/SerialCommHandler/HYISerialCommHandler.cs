using Haberlesme.Paketler;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haberlesme.CommunicationMethodes.SerialCommunication.SerialComm.SerialCommHandler
{
    internal class HYISerialCommHandler : ISerialCommunication
    {
        
        public SerialPort SerialPort { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Queue<byte[]> IncomingData { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Queue<byte[]> DataSendQueue { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void OnSerialRecive()
        {
            throw new NotImplementedException();
        }

        public void PackageDetected()
        {
            throw new NotImplementedException();
        }

        public IPackageBase RecivePackage()
        {
            throw new NotImplementedException();
        }

        public void SendPackage(byte[] paket)
        {
            throw new NotImplementedException();
        }
    }
}
