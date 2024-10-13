using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haberlesme.Sifreleme
{
    internal interface IEncryptionMethodeBase
    {
        byte[] mesajSifrele(ref byte[] x);

        byte[] mesajDesifre(ref byte[] x);
    }
}
