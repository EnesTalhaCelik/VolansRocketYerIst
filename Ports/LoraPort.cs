using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ports
{
    public class LoraPort 
    {

        internal static System.IO.Ports.SerialPort LoraSerialPort = new System.IO.Ports.SerialPort();



        public static string Open()
        {
            try
            {
                LoraSerialPort.Open();
                return ($"{LoraSerialPort.PortName} seri portu başarıyla açılmıştır.");
            }
            catch (Exception ex)
            {
                //port parametreleri eksik hatası ekle
                return ($"{LoraSerialPort.PortName} seri portu açılırken bir hata meydana geldi.{Environment.NewLine}Hata: {ex.Message}");
            }
        }
        //bunun bir de chip adı içeren versiyonu lazım
        public static string Open(Values.SerialPortValues portValues)
        {
            try
            {
                LoraSerialPort.BaudRate = portValues.BaudRate;
                LoraSerialPort.Parity = portValues.Parity;
                LoraSerialPort.DataBits = portValues.DataBits;

                LoraSerialPort.PortName = portValues.PortName;
                LoraSerialPort.Open();
                return ($"{LoraSerialPort.PortName} noktasındaki lora seri portu başarıyla açılmıştır.");
            }
            catch (Exception ex)
            {
                //port parametreleri eksik hatası ekle
                return ($"{LoraSerialPort.PortName} noktasındaki lora seri portu açılırken bir hata meydana geldi.{Environment.NewLine}Hata: {ex.Message}");
            }
        }

        public static string Close()
        {
            try
            {
                LoraSerialPort.Close();
                return ($"{LoraSerialPort.PortName} noktasındaki lora seri portu başarıyla kapatıldı ");

            }
            catch (Exception ex)
            {
                //port parametreleri eksik hatası ekle
                return ($"{LoraSerialPort.PortName} noktasındaki lora seri portu kapatılırken bir hata meydana geldi.{Environment.NewLine}Hata: {ex.Message}");
            }
        }


    }
}
