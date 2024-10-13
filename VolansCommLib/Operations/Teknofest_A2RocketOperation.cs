using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using VolansCommLib.Methodes;
using VolansCommLib.NodeConnector.LoraNodeConnector;
using VolansCommLib.Nodes.NodeClasses;
using VolansCommLib.Methodes;
namespace VolansCommLib.Operations
{
    internal class Teknofest_A2RocketOperation:IOperation
    {
        VolansPayloadNode PayloadNode { get; }
        VolansAvionicsNode MainAvionicsNode { get; }
        VolansAvionicsNode SecondaryAvionicsNode { get; }

        VolansHYINode HYINode { get; }


        //200ms de bir veri gönderilemiyorsa threading timera geçilebileir
        System.Timers.Timer OperationTimer;

        byte[] LastRecivedRocketAvionicsMessage;


        public Teknofest_A2RocketOperation(VolansAvionicsNode MainAvionics, VolansAvionicsNode SecondaryAvionics, VolansPayloadNode Payload, VolansHYINode HYI)
        {

            this.MainAvionicsNode = MainAvionics;
            this.SecondaryAvionicsNode = SecondaryAvionics;
            this.PayloadNode = Payload;
            this.HYINode = HYI;
            SetTimerInterval();

        }



        string SetTimerInterval(double timerInterval = 200)
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
            byte[] packageToSend = ArrayCombineMethodes.combineTwoArrays(LastRecivedRocketAvionicsMessage, PayloadNode.GetSensorInfo());
            HYINode.SendMessage(packageToSend);


        }

        void StopOperation()
        {
            OperationTimer.Stop();
        }

        void ContinueOperation()
        {
            if(OperationTimer != null)
            {
                OperationTimer.Start();
            }
        }

        
        void IOperation.OnOperationRun(object source, ElapsedEventArgs e)
        {
            throw new NotImplementedException();
        }

        void IOperation.EndOperation()
        {
            OperationTimer.Stop();
            OperationTimer.Close();
            OperationTimer = null;
        }

        public void RunOperation()
        {
            if (CheckRunningConditions()&&OperationTimer.Enabled!= true) 
            {
                
                OperationTimer.Elapsed += OnOperationRun;
                OperationTimer.Enabled = true;
            }
            else
            {
                throw new NotImplementedException();
            }


        }

        public bool CheckRunningConditions()
        {

            //iki NodeConnector de iletişime hazırsa true döndürür aksi taktirde false döndürür.
            return (HYINode.ConnectedNodeConnector.CommMethode.Communicator.IsReadyToCommunicate()&&
                    MainAvionicsNode.ConnectedNodeConnector.CommMethode.Communicator.IsReadyToCommunicate());
        }
    }
}
