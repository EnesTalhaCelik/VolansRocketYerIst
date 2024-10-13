using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haberlesme.Nodes.VirtualNodes
{

    //A virtual node is used for representing communicatable LAN nodes on program.
    //Differance between a HQ node and a virtual node is that a virtual node does not contain send and recive methodes
    //essentially their whole purpose is storing information.
    internal interface IVirtualNode:INode
    {




    }
}
