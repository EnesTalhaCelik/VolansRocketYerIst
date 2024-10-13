using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Haberlesme.PackageTypes;
namespace Haberlesme.Nodes.HQNodes
{
    internal interface IHQNode:INode
    {
        byte[] AcceptedStartCode { get; set; }
        void SendMessage(INode SendNode,IPackage DataPackage);

        

    }
}
