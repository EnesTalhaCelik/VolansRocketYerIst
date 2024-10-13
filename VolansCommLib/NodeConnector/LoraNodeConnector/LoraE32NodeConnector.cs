using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolansCommLib.CommMethodes;
using VolansCommLib.CommMethodes.CommMethodes;
using VolansCommLib.NetworkInfoHolder;
using VolansCommLib.Nodes;
using VolansCommLib.Nodes.NodeClasses;
using VolansCommLib.PackageHandlers;

namespace VolansCommLib.NodeConnector.LoraNodeConnector
{
    internal class LoraE32NodeConnector : INodeConnector
    {



        public List<IVirtualNode> ConnectedNodes { get; set; } = new List<IVirtualNode>();


        public IPackageHandler PackageHandler { get; set; } 
        public ICommMethode CommMethode { get; set; } = new LoraCommMethode();

        public IMainNode mainNode => throw new NotImplementedException();

        public string NodeConnectorName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void ConnectToANode(IVirtualNode node)
        {
            if (node is VolansAvionicsNode)
                ConnectedNodes.Add(node);
        }

        public void ConnectToANode(ILoraNetworkInfoHolder nodeINFO)
        {

                VolansAvionicsNode node = new VolansAvionicsNode();
                node.NetworkInfo = nodeINFO;
                ConnectedNodes.Add(node);
        }


        public string DisconnectNode(IVirtualNode NodeToRemove)
        {
            if(!ConnectedNodes.Remove(NodeToRemove))
            {

                return ($"There is no node called {NodeToRemove.NetworkInfo.NetworkNodeName} in specified node connector");
            }
            return "success";
            
        }

        public void DisconnectNode()
        {
            throw new NotImplementedException();
        }

        public void SendMessageToNode(int listIndex,IVirtualNode reciverNode, byte[] Message)
        {

            ConnectedNodes[listIndex].SendMessage(Message);

        }

        public void SendMessageToNode()
        {
            throw new NotImplementedException();
        }

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
