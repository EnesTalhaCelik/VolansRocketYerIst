using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolansCommLib.NetworkInfoHolder.NetworkInfoHolders
{


    internal class LoraNetworkInfoHolder : ILoraNetworkInfoHolder
    {
        public byte LoraAddress => throw new NotImplementedException();

        public byte LoraStartCode => throw new NotImplementedException();

        public byte LoraChannel => throw new NotImplementedException();

        public string NetworkNodeName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public byte[] StartCode => throw new NotImplementedException();

        public byte NetworkAddressCode => throw new NotImplementedException();

        public byte[] ReturnNetworkInfo()
        {
            byte[] buffer = new byte[4+StartCode.Length];
            buffer[0] = LoraStartCode;
            buffer[1] = LoraAddress;
            buffer[2] = LoraChannel;
            Array.Copy(StartCode,0, buffer,3,StartCode.Length);
            buffer[buffer.Length-1] = NetworkAddressCode;
            return buffer;
        }
    }
}
