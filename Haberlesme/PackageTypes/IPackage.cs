using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haberlesme.PackageTypes
{
    internal interface IPackage
    {

        byte IncomingAddress { get; }
        byte Identifier { get; }
        byte PackageLength { get; }
        bool IsCompleted { get; }
        byte[] Data { get; set; }

        void InitDataArray();

    }
}
