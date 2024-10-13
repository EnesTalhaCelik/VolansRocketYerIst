using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolansCommLib.NetworkInfoHolder
{
    internal interface ILoraNetworkInfoHolder:INetworkInfoHolder
    {

        byte LoraAddress { get; }
        byte LoraStartCode { get; }//bu öncü kod durumuna göre değişebiliyormuş uyanma saniyesini belirtiyor olabilir.
        byte LoraChannel { get; }


    }
}
