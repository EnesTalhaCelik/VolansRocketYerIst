using Haberlesme.HaberlesmeMetotlari;
using Haberlesme.PackageTypes;
using Haberlesme.Sifreleme;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haberlesme.Nodes
{
    internal class Node_HYI : INode
    {
        public byte[] PaketBaslangicKodu { get;} = { 0xff, 0xff };

        public byte[] NetworkAddressCode   { get;} = { 0x54, 0x52 };

        public string NodeName { get ; set ; }
        public ICommMethodeBase CommMethode { get; set; } 
        public IEncryptionMethodeBase sifrelemeProtokolu { get; set; } = new Sifreleme_YOK();
        public NetworkInfoHolder.NetworkInfoHolder NetworkInfo { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IPackage[] Packages { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        List<ICommMethodeBase> INode.CommMethode { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
