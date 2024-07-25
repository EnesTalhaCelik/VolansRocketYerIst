using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIOperations
{
    internal class TextboxOperations
    {
        public static string PortAcildi(SerialPort serialport)
        {
            return ($"{serialport.PortName} bağlantı noktası başarıyla açıldı");

        }

        public static string PortAcikMi(SerialPort serialPort)
        {
            if (serialPort.IsOpen)
            {
                return ($"{serialPort.PortName} bağlantı noktası ile başarıyla bağlantı kuruldu");
            }
            else
            {
                return ($"{serialPort.PortName} bağlantı noktası ile bağlantı kurulamadı");

            }
        }

        public static string DateTimeSigner(String someString)
        {
            return ($"{DateTime.Now}: {someString}");

        }
        
        public static string LoraSerialRecivedSuccess(Values.Roket roket,string paket)
        {              
            return ($"Lora portunda {paket} tanımlayıcılı paket başarıyla tespit edildi");
        }
        public static string LoraSerialRecivedReadError(Values.Roket roket, string paket)
        {
            return ($"Lora portuna gelen paket okunurken bir sorun ile karşılaşıldı tanımlayıcılı paket okunamadı");
        }
        public static string LoraSerialRecivedFail(Values.Roket roket, string paket)
        {
            return ($"Lora portuna gelen paket okunamadı");
        }
        public static string HYIRecivedSuccess()
        {
            return ($"HYI'dan veri başarıyla okundu");
        }

        public static string HYIRecivedFail()
        {
            return ($"HYI portundan gelen veri okunamadı");
        }

    }
}
