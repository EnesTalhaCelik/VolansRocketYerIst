using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using Values;

namespace SerialPortOperations
{
    internal class SerialPort
    {
        public static string Open(System.IO.Ports.SerialPort somePort)
        {
            try
            {
                somePort.Open();
                return($"{somePort.PortName} seri portu başarıyla açılmıştır.");
            }
            catch( Exception ex ) 
            {

                return ($"{somePort.PortName} seri portu açılırken bir hata meydana geldi.{Environment.NewLine}Hata: {ex.Message}");
            }
        }

 
        
        public static void AssignParameters(Values.SerialPortValues SerialPortValues, System.IO.Ports.SerialPort serialPort)
        {
            serialPort.BaudRate = SerialPortValues.BaudRate;
            serialPort.Parity = SerialPortValues.Parity;
            serialPort.DataBits = SerialPortValues.DataBits;
            serialPort.PortName = SerialPortValues.PortName;
        }
        public static void AssignParameter(Values.SerialPortValues SerialPortValues, System.IO.Ports.SerialPort serialPort)
        {
            serialPort.BaudRate = SerialPortValues.BaudRate;
            serialPort.Parity = SerialPortValues.Parity;
            serialPort.DataBits = SerialPortValues.DataBits;
            serialPort.PortName = SerialPortValues.PortName;
        }



    }
}
