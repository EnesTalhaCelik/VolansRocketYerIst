using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace VolansCommunication
{
    internal class LoraE32
    {
        internal enum LORA_MODE
        {
            STANDBY,
            FLIGHT
        }
               
        struct LoraE32_Transmit_Buffer()
        {
            internal byte[] bufferTransmit = new byte[1024];
            internal uint BUFFER_EMPTY_INDEX;

        }

        struct LoraE32_Received_Buffer()
        {
           internal byte[] bufferReceived = new byte[1024];
           internal uint BUFFER_EMPTY_INDEX;

        }


        internal struct LoraE32_INFO()//fix name later
        {
            internal byte LORA_CHANNEL = 0;
            internal byte LORA_ADDRESS = 0;
            internal byte LORA_BaudRate;
            internal Parity LORA_Parity;
            internal string portname;
            internal SerialPort LoraPort = new SerialPort();
            internal LORA_MODE Operating_Mode = LORA_MODE.STANDBY;
            //more info can be controlled like transmission mode but do we really need it ?   

        }

        

        
            LoraE32_Received_Buffer LORA_BUFFER_RECEIVED;
            LoraE32_Transmit_Buffer LORA_BUFFER_TRANSMIT;
            LoraE32_INFO LORA_INFO;


        
        void SetOperatingMode(byte mode)
        {
            if (mode == 1)
            {
                LORA_INFO.Operating_Mode = LORA_MODE.FLIGHT;
                LORA_INFO.LoraPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler_Flight);


            }
            else if (mode == 0) {
                LORA_INFO.Operating_Mode = LORA_MODE.STANDBY;
                LORA_INFO.LoraPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler_Standby);

            }
            else 
                return;


            
        }

      
        // #check #fix
            string LORA_GET_PARAMS()
            {
            if (!LORA_INFO.LoraPort.IsOpen)
                return ("Lora port must be open to use this methode.");

            LORA_INFO.LoraPort.WriteLine("lora get params");

            if (LORA_INFO.Operating_Mode == LORA_MODE.FLIGHT) {

                return ("You cannot get lora parameters while in flight mode.");
            
            }

            //check recived message buffer add flight mode / stamndby mode this methode can oly work in standby mode
            byte l_Address = 0;
            byte l_channel = 0;


            return ($"Lora Params Got Succesfully.{Environment.NewLine}Channel : {l_channel}, Address : {l_Address} ");

            }

            string LORA_INIT()
            {
            //Fix or add error handling maybe.
            //#fix if tx or rx should be first
            //check for initialization errors dynamic allocation may couse some problems


                LORA_INFO.LoraPort.PortName = LORA_INFO.portname;
                LORA_INFO.LoraPort.Parity = LORA_INFO.LORA_Parity;
                LORA_INFO.LoraPort.BaudRate = LORA_INFO.LORA_BaudRate;

            try
            {
                LORA_INFO.LoraPort.Open();

            }
            catch (Exception e) {
                return e.Message;

            }
            return ($"Serialport has been opened successfuly, Port name : {LORA_INFO.LoraPort.PortName}, Baud Rate :  {LORA_INFO.LoraPort.BaudRate}");
        }




            //maybe implement a mutex?
            void BUFFER_TRANSMIT_APPEND(byte value)
            {
                if (LORA_BUFFER_TRANSMIT.BUFFER_EMPTY_INDEX < LORA_BUFFER_TRANSMIT.bufferTransmit.Length - 1)
                {
                    LORA_BUFFER_TRANSMIT.bufferTransmit[LORA_BUFFER_TRANSMIT.BUFFER_EMPTY_INDEX] = value;
                    LORA_BUFFER_TRANSMIT.BUFFER_EMPTY_INDEX++;
                }

            }
            void BUFFER_TRANSMIT_CLEAN()
            {
                LORA_BUFFER_TRANSMIT.BUFFER_EMPTY_INDEX = 0;
                //you dont need to manually reset the buffer. when empty index set to zero it becomes automaticly cleaned.
            }

            uint LORA_TRANSMIT_BUFFER()
            {
                uint transmittedBytes = LORA_BUFFER_TRANSMIT.BUFFER_EMPTY_INDEX + 1;
                //LORA_SERIAL.write(LORA_BUFFER_TRANSMIT.bufferTransmit, LORA_BUFFER_TRANSMIT.BUFFER_EMPTY_INDEX + 1);

                BUFFER_TRANSMIT_CLEAN();
                return transmittedBytes;

            }
            //void LORA_CHANGE_ADDRESS(); //If this is going to be a feature we need to change the design of the board

            uint LORA_GET_RECIEVED()
            {
            /*
                if (LORA_SERIAL.available() > 0)
                {
                    //check delay
                    Thread.Sleep(300);
                   // byte available = LORA_SERIAL.available();
                    LORA_BUFFER_RECEIVED.BUFFER_EMPTY_INDEX += available;
                    //fix overflow issues

                   // LORA_SERIAL.readBytes(LORA_BUFFER_RECEIVED.BUFFER_RECEIVED, available);
                   // return available;
                }*/
            return 0;
            }


        private void DataReceivedHandler_Flight(object sender, SerialDataReceivedEventArgs e)
        {


        }


        private void DataReceivedHandler_Standby(object sender, SerialDataReceivedEventArgs e)
        {
           


        }




    }
    }
