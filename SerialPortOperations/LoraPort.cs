using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
namespace SerialPortOperations
{
    public class LoraPort
    {

        private static System.IO.Ports.SerialPort LoraSerialPort = new System.IO.Ports.SerialPort();



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



    }
}
