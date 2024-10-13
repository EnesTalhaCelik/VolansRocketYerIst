using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haberlesme.CommunicationMethodes.SerialCommunication.Devices.Lora
{
    internal interface ILoraMode
    {
        //loranın operasyon modlarını kontrol etmek için kullanılır
        bool sendMessage();
        bool reciveParam();

    }
}
