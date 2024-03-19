using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;


namespace SeriHaberlesme
{
    public static class PortConfig
    {
        public static int[] baudRates = { 110, 300, 600, 1200, 2400, 4800, 9600, 14400, 19200, 38400, 57600, 115200, 128000, 256000  };
        static System.IO.Ports.Parity[] pairityTypes = { Parity.None, Parity.Odd, Parity.Even, Parity.Mark, Parity.Space };
    }
}
