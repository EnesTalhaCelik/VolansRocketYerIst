using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolansCommLib;
using VolansCommLib.Nodes.NodeClasses;
using VolansCommLib.Operations;
namespace VolansCommLib.TestScript
{
    internal class Test
    {

        public static void main(string[] args)
        {
            
            VolansAvionicsNode BirincilSist = new VolansAvionicsNode();
            VolansAvionicsNode YedekSist = new VolansAvionicsNode();
            VolansPayloadNode Payload = new VolansPayloadNode();
            VolansHYINode hYINode = new VolansHYINode();
            VolansMainNode MainNode = new VolansMainNode();

            double x = 200;

            MainNode.InitNewLoraConnector();
            MainNode.InitNewHYIConnector();

            BirincilSist.ConnectedNodeConnector = MainNode.E32NodeConnector;
            YedekSist.ConnectedNodeConnector = MainNode.E32NodeConnector;
            Payload.ConnectedNodeConnector  = MainNode.E32NodeConnector;



            

            MainNode.E32NodeConnector.ConnectToANode(BirincilSist);
            MainNode.E32NodeConnector.ConnectToANode(YedekSist);
            MainNode.E32NodeConnector.ConnectToANode(Payload);
            MainNode.HYINodeConnector.ConnectToANode(hYINode);


            //bu şekilde çalışır ama operasyona başlamadan önce doğrulamaya ihtiyacımız var.
            //Node connectorler bağlı mı boş mu? 
            //portlar açık mı


            MainNode.RunningOperations.Add(new Teknofest_A2RocketOperation(BirincilSist, YedekSist, Payload, hYINode));

            MainNode.RunningOperations[0].RunOperation();








        }


    }
}
