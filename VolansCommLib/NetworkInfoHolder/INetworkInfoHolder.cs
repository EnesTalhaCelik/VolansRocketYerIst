using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolansCommLib.NetworkInfoHolder
{
    internal interface INetworkInfoHolder
    {
        //her network info holderin içinde request data gibi bir kod olmaı!!
        //böylece farklı senaryolardaki network code ihtiyacı çözülebilir!!!

        string NetworkNodeName { get; set; }
        byte[] StartCode { get; }
        byte NetworkAddressCode { get;}
        byte[] ReturnNetworkInfo();

        
    }
}
