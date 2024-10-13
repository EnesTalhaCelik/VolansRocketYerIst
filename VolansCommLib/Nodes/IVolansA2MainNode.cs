using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolansCommLib.NodeConnector;
using VolansCommLib.NodeConnector.LoraNodeConnector;

namespace VolansCommLib.Nodes
{
    internal interface IVolansA2MainNode:IMainNode
    {
        LoraE32NodeConnector E32NodeConnector { get; set; }

        HYINodeConnector HYINodeConnector { get; set; }



    }
}
