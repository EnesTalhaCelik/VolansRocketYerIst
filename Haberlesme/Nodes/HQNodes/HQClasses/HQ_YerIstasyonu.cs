using Haberlesme.HaberlesmeMetotlari;
using Haberlesme.Paketler;
using Haberlesme.Sifreleme;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haberlesme.Nodes.HQNodes.HQClasses
{
    internal class HQ_YerIstasyonu : IHQ_YerIstasyonu
    {
        public List<ICommMethodeBase> CommMethode { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IEncryptionMethodeBase sifrelemeProtokolu { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public NetworkInfoHolder.NetworkInfoHolder NetworkInfo { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void SendMessage(INode SendNode, IPackageBase DataPackage)
        {
            throw new NotImplementedException();
        }
    }
}
