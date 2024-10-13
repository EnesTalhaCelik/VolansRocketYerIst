using Haberlesme.CommunicationMethodes.SerialCommunication.Devices.Lora.LoraE32.LoraE32Modes;
using Haberlesme.CommunicationMethodes.SerialCommunication.SerialComm;
using Haberlesme.Nodes;
using Haberlesme.Nodes.HQNodes;
using Haberlesme.Nodes.VirtualNodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haberlesme.CommunicationMethodes.SerialCommunication.Devices.Lora.LoraE32
{
    internal class LoraE32 : ILoraBase
    {

        
        public byte transmissionMode { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ILoraMode LoraMode { get; set; } = new E32_MODE_NORMAL();

        
        public byte LoraStartCode { get; set; }
        public byte LoraChannel { get; set ; }
        public byte LoraAddress { get; set ; }

        public ISerialCommunication SerialPortHandler {get; set;}
        public List<INode> ConnectedVirtualNodes { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IHQNode HQNode { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public byte[] ChangeLoraParam(byte[] param)
        {
            throw new NotImplementedException();
        }
        //paket oluşturmak bu katmanın görevi değil
        //buraya veri gönderildiğinde otomatik olararak başına lora protokolleri eklenir ve mesaj gönderilir.


        public byte[] CreatePackage(byte[] dataToSend,ILoraBase ReciverDevice)
        {
            byte[] package = new byte[2+dataToSend.Length];

            package[0] = ReciverDevice.LoraStartCode;
            package[1] = ReciverDevice.LoraAddress;
            package[2] = ReciverDevice.LoraChannel;
            package.Concat(dataToSend);
            
            return package;
        }

        public string SendPackageToNode(byte[] packageToSend)
        {
            try
            {
                SerialPortHandler.SerialPort.Write(packageToSend, 0, packageToSend.Length);

            }
            catch (Exception ex) 
            {
                return(ex.Message.ToString());
            }
            return("Success");
        }
   

        public byte[] ReciveLoraParam()
        {
            throw new NotImplementedException();
        }
    }
}
