using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolansCommLib.NetworkInfoHolder;
using VolansCommLib.NodeConnector;
using VolansCommLib.NodeConnector.LoraNodeConnector;
using VolansCommLib.Operations;

namespace VolansCommLib.Nodes.NodeClasses
{
    internal class VolansMainNode : IVolansA2MainNode
    {
        public INetworkInfoHolder NetworkInfo { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Dictionary<byte, Action<byte[]>> AcceptedPackageDict => throw new NotImplementedException();
        
        public LoraE32NodeConnector E32NodeConnector { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public HYINodeConnector HYINodeConnector { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public List<IOperation> RunningOperations => throw new NotImplementedException();

        public void InitNewHYIConnector()
        {

            HYINodeConnector = new HYINodeConnector();

        }
        



        public void InitNewLoraConnector()
        {
            E32NodeConnector = new LoraE32NodeConnector();
            
        }
    }
}
