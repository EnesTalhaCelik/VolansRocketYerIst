using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haberlesme.Sifreleme
{
    internal class Sifreleme_YOK : IEncryptionMethodeBase
    {
        public byte[] mesajDesifre(ref byte[] x)
        {
            return x;
        }

        public byte[] mesajSifrele(ref byte[] x)
        {
            return x;
        }
    }
}
