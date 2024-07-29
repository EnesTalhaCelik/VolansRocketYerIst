using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ports
{
    internal class HYIPort
    {

        internal static System.IO.Ports.SerialPort HYISerialPort = new System.IO.Ports.SerialPort();



        public static string Open()
        {
            try
            {
                HYISerialPort.Open();
                return ($"{HYISerialPort.PortName} seri portu başarıyla açılmıştır.");
            }
            catch (Exception ex)
            {
                //port parametreleri eksik hatası ekle
                return ($"{HYISerialPort.PortName} seri portu açılırken bir hata meydana geldi.{Environment.NewLine}Hata: {ex.Message}");
            }
        }
        //bunun bir de chip adı içeren versiyonu lazım
        public static string Open(Values.SerialPortValues portValues)
        {
            try
            {
                HYISerialPort.BaudRate = portValues.BaudRate;
                HYISerialPort.Parity = portValues.Parity;
                HYISerialPort.DataBits = portValues.DataBits;

                HYISerialPort.PortName = portValues.PortName;
                HYISerialPort.Open();
                return ($"{HYISerialPort.PortName} noktasındaki lora seri portu başarıyla açılmıştır.");
            }
            catch (Exception ex)
            {
                //port parametreleri eksik hatası ekle
                return ($"{HYISerialPort.PortName} noktasındaki lora seri portu açılırken bir hata meydana geldi.{Environment.NewLine}Hata: {ex.Message}");
            }
        }

        public static string Close()
        {
            try
            {
                HYISerialPort.Close();
                return ($"{HYISerialPort.PortName} noktasındaki lora seri portu başarıyla kapatıldı ");

            }
            catch (Exception ex)
            {
                //port parametreleri eksik hatası ekle
                return ($"{HYISerialPort.PortName} noktasındaki lora seri portu kapatılırken bir hata meydana geldi.{Environment.NewLine}Hata: {ex.Message}");
            }
        }


    }
}
