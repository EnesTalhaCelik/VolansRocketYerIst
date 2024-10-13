using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolansCommLib.CommMethodes;
using VolansCommLib.NetworkInfoHolder;
using VolansCommLib.Nodes;
using VolansCommLib.Nodes.NodeClasses;
using VolansCommLib.PackageHandlers;

namespace VolansCommLib.NodeConnector
{
    internal class HYINodeConnector : INodeConnector
    {
        public List<IVirtualNode> ConnectedNodes { get; set; }

        public IPackageHandler PackageHandler => throw new NotImplementedException();

        public ICommMethode CommMethode { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IMainNode mainNode => throw new NotImplementedException();

        public string NodeConnectorName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void ConnectToANode(IVirtualNode node)
        {

            if (node is VolansHYINode)
                ConnectedNodes[0] = node;
            //only one HYI nnode can connect to this connector!
        }

        /// <summary>
        /// You can only use this function to disconnect HYI Node
        /// </summary>
        public void DisconnectNode()
        {
            ConnectedNodes.Clear();
        }


        /// <summary>
        /// You can only use this function to disconnect HYI Node
        /// Becouse there can be only one connected HYI Node
        /// </summary>
        public void SendMessageToNode(byte[] arr)
        {
            if(ConnectedNodes.Count > 0)
            {
                ConnectedNodes[0].SendMessage(arr);
            }
        }

        /// <summary>
        /// We are not planning to use this methode. We prefer to send message through directly node interface. Generally we use node connector as a dictionary to node info
        /// and host a communication methode.
        /// </summary>

        public void SendMessageToNode(byte[] messageToSend, IVirtualNode destinationNode)
        {
            throw new NotImplementedException();
        }

        public void SendMessageToNode(byte[] messageToSend, INetworkInfoHolder nodeNetworkInfo)
        {
            throw new NotImplementedException();
        }
    }
}
