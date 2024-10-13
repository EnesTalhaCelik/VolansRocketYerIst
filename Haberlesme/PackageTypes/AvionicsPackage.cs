using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haberlesme.PackageTypes
{
    internal class AvionicsPackage : IPackage
    {

        public byte IncomingAddress { get; }

        public byte Identifier { get; }

        public byte PackageLength { get; } = 62;

        public bool IsCompleted { get; set; } = false;
        public byte[] Data { get; set; }

        public AvionicsPackage(byte PackageSize,byte Identifier)
        {
            this.PackageLength = PackageSize;
            this.Identifier = Identifier;
            InitDataArray();
        }

        public void InitDataArray()
        {
            Data = new byte[this.PackageLength];
        }
    }


}
