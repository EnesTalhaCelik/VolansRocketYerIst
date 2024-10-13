using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolansCommLib.NodeConnector;
using VolansCommLib.Nodes;

namespace VolansCommLib.PackageHandlers
{

    ///<summary>
    /// Purpose of this class is managing incoming and outgoing messages. Create/Sign/Unpackage them 
    ///</summary>
    internal interface IPackageHandler
    {



        //mesaj gönderme fonksiyonlarını nodeların içine taşıyalım mı ?
        INodeConnector NodeConnector { get; set; }

        INode SearchNode(byte NodeAddress);


        ///<summary>
        /// This Method is only going to be get used in the case of sending message to avionics systems.
        /// Use case: filling start code, sender node code... etc
        /// ONLY IN AVIONICS SYSTEMS! NOT HYI!
        ///</summary>
        
        void SearchPackage(byte[] ArrayToSearch);
        byte[] CreatePackage(INode ReciverNode, byte[] Package);


        byte[] CreatePackage(byte[] message);

        void InitPackageHandler();
    }
}
