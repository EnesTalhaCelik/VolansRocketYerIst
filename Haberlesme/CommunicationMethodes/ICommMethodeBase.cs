using Haberlesme.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haberlesme.HaberlesmeMetotlari
{
    internal interface ICommMethodeBase
    {
        //bunlar private olmalı çünkü asıl mesaj gönderme node üzerinden olmalı ve şifreleyip gönderilmeli!!

        void SendPackage(byte[] paket);
        Queue<byte[]> IncomingData { get; set; }

        Queue<byte[]> DataSendQueue { get; set; }



        //veri alma fonksiyonu olmalı+
        //gelen verileri alma fonksiyonu olmalı+
        //gelen veriler arasında gezinme otomatik olarak silme vs olmalı+


    }
}
