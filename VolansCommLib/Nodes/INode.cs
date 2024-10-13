using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using VolansCommLib.NetworkInfoHolder;

namespace VolansCommLib.Nodes
{
    internal interface INode
    {
        
        INetworkInfoHolder NetworkInfo { get; set; }
        

        Dictionary<byte, Action<byte[]>> AcceptedPackageDict { get; }
    }


}
