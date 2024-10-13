using Haberlesme.CommunicationMethodes.SerialCommunication.Devices;
using Haberlesme.HaberlesmeMetotlari;
using Haberlesme.PackageTypes;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haberlesme.CommunicationMethodes.SerialCommunication.SerialComm
{
    //Ayrı olarak hq node üzerinde açılır.
    internal interface ISerialCommunication : ICommMethodeBase
    {
        //serialporttan gelen verileri ayıklamak bu katmanın görevidir.
        ISerialCommDevice ConnectedDevice { get; set; }
        SerialPort SerialPort { get; set; }
        void OnSerialRecive();
        void SendMessage(IPackage package);


        //ISerialCommDevice ConnectedSerialDevice { get; set; }

        //sadece veri tutucu olarak kullanmak için ve paket oluşturmak için kullanılır
        //başka bir görevi yoktur.!!!!!
        //void DisconnectSerialDevice();
        //void ConnectSerialDevice(SerialPort serialPort);
    }
}
