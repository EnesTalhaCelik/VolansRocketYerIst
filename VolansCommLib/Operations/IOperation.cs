using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace VolansCommLib.Operations
{
    //BİRDEN FAZLA OPERASYON ÇALIŞTIRMAYA OLANAK SAĞLAMALI MIYIZ????
    internal interface IOperation
    {
        void OnOperationRun(object source, ElapsedEventArgs e);
        void EndOperation();
        void RunOperation();
        bool CheckRunningConditions();
    }
}
