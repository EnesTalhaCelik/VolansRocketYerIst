using Haberlesme.DataProcessor.PackageHandler;
using Haberlesme.Haberlesme_Metotlari.Kablosuz_Haberlesme;
using Haberlesme.Haberlesme_Metotlari.Kablosuz_Haberlesme.Lora_Haberlesme;
using Haberlesme.Haberlesme_Metotlari.Kablosuz_Haberlesme.Lora_Haberlesme.Transmission_Modes;
using Haberlesme.HaberlesmeMetotlari;
using Haberlesme.Sifreleme;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haberlesme.Nodes
{
    internal class Node_HQ : INode
    {
        public byte[] PaketBaslangicKodu => throw new NotImplementedException();

        public byte[] NetworkAddressCode => throw new NotImplementedException();

        public string NodeName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public List<ICommMethodeBase> CommMethode { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IEncryptionMethodeBase sifrelemeProtokolu { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IBasePackageHandler PackageHandler { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}