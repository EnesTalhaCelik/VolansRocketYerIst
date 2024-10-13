using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VolansCommLib.CommMethodes.Communicators
{
    internal class SerialPortCommunicator:ICommunicator
    {
        public string PortName { get; set; }
        public int BaudRate { get; set; } 
        public int DataBits{ get; set; }
        public Parity PortParity { get; set; } = Parity.None;

    public ICommMethode ConnectedCommMethode { get; set; }

        SerialPort CommunicatorSerialPort { get; set; } = new SerialPort();

            public bool IsReadyToCommunicate ()
            {

            return CommunicatorSerialPort.IsOpen;

            }

        internal string StartCommunicatior()
        {
            CommunicatorSerialPort.DataReceived += CommunicatorSerialPort_DataReceived;
            CommunicatorSerialPort.PortName = PortName;
            CommunicatorSerialPort.BaudRate = BaudRate;
            CommunicatorSerialPort.DataBits = DataBits;
            CommunicatorSerialPort.Parity = PortParity;
            try
            {
                
                CommunicatorSerialPort.Open();
            }
            catch (Exception ex) 
            {
                return ($"Error : {ex.Message}");
            }

            return "Opened CommunicatorSerialPort with Success";
        }

        internal void CommunicatorSerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Thread.Sleep(10);
            byte[] buffer= new byte[CommunicatorSerialPort.BytesToRead];
            CommunicatorSerialPort.Read(buffer,0,CommunicatorSerialPort.BytesToRead);
            ConnectedCommMethode.NodeConnector.PackageHandler.SearchPackage(buffer);
        }

        internal string CloseSerialPort() 
        {
            try
            {
                CommunicatorSerialPort.DiscardInBuffer();
                CommunicatorSerialPort.Close();
            } 
            catch(Exception ex) 
            { 
               return($"Error : {ex.Message}");
            }
            return "Closed CommunicatorSerialPort with Success";
        }

       
        void SetCommunicatorParams()
        {


        }





    }
}
