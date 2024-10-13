using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haberlesme.CommunicationMethodes.SerialCommunication.Devices.Lora
{
    internal interface ILoraBase:ISerialCommDevice
    {
        //operasyon modları
        //loranın modlarını takip etmek için kullanılır.
        byte LoraStartCode { get; set; }
        byte LoraChannel { get; set; }
        byte LoraAddress { get; set; }

        byte transmissionMode { get; set; } //gerekirse ayrı sınıf aç
        ILoraMode LoraMode { get; set; }
        byte[] CreatePackage(byte[] dataToSend, ILoraBase ReciverDevice);
        byte[] ReciveLoraParam();       
        byte[] ChangeLoraParam(byte[] param);
        //lora modları manuel olarak değiştirilir.
    }
}
