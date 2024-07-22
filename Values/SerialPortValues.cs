using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Values
{
    public class SerialPortValues
    {

        public int BaudRate  { get; set; }
        public byte DataBits { get; set; }
        public Parity Parity {  get; set; }
        public string PortName { get; set; }

        


    }
}
