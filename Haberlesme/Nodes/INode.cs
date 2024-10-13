using Haberlesme.DataProcessor.PackageHandler;
using Haberlesme.HaberlesmeMetotlari;
using Haberlesme.PackageTypes;
using Haberlesme.Sifreleme;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haberlesme.Nodes
{
    internal interface INode
    {
        //network info / node info classı oluştrabilirsin
        NetworkInfoHolder.NetworkInfoHolder NetworkInfo { get; set; }
        List <ICommMethodeBase> CommMethode { get; set; }
        IPackage[] Packages { get; set; }
        

    }
}
