using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolansCommLib.NodeConnector;
using VolansCommLib.Operations;

namespace VolansCommLib.Nodes
{
    internal interface IMainNode:INode
    {
        List<IOperation> RunningOperations { get; }

        



    }
}
