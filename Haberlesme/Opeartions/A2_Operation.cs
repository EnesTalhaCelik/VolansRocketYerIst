using Haberlesme.Nodes;
using Haberlesme.Nodes.VirtualNodes;
using Haberlesme.Nodes.VirtualNodes.VirtualNodeClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
namespace Haberlesme.Opeartions
{
    internal class A2_Operation
    {



        VirtualNode_AviyonicSystem MainAvionics;
        VirtualNode_AviyonicSystem SecondaryAvionics;
        VirtualNode_Payload Payload;
        VirtualNode_HYI HYI;

        //200ms de bir veri gönderilemiyorsa threading timera geçilebileir
        System.Timers.Timer OperationTimer;


        public A2_Operation(VirtualNode_AviyonicSystem MainAvionics, VirtualNode_AviyonicSystem SecondaryAvionics, VirtualNode_Payload Payload, VirtualNode_HYI HYI, double timerInterval)
        {

            this.MainAvionics = MainAvionics;
            this.SecondaryAvionics = SecondaryAvionics;
            this.Payload = Payload;
            this.HYI = HYI;

            OperationTimer = new System.Timers.Timer(timerInterval);
            OperationTimer.Elapsed += OnOperationRun;
            OperationTimer.Enabled = true;
        }



        string SetTimerInterval(double timerInterval)
        {
            if (OperationTimer.Enabled == false)
            {
                OperationTimer = new System.Timers.Timer(timerInterval);
                return ($"Operation timer interval has been setted for act every {timerInterval} ms.");
            }
            else
            {
                return ($"Operation timer is currently running.");
            }
        }

        void OnOperationRun(object source, ElapsedEventArgs e)
        {
            
            
            

        }

        void EndOperation()
        {


        }
        


    }
}
