using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIOperations;
namespace Ports
{
    public class LoraCommunication
    {
        
        public static string SendFixedMessage(byte[] byteArray)
        {
            try
            {
                LoraPort.LoraSerialPort.Write(byteArray, 0, byteArray.Length);

            }
            catch (Exception e) 
            {
                return (UIOperations.TextboxOperations.DateTimeSigner($"Bir Sorunla karşılaştık " +
                    $"{UIOperations.TextboxOperations.LoraMessageSentFailedException(e)}"));
            }

            return (UIOperations.TextboxOperations.LoraMessageSentSuccessful());
        }

      

    

        public static string DeterminePackageID(byte[] byteArray)
        {
             
        }

        public static string ReciveAnaAvPackage()
        {

        }

        public static string ReciveYedekAvPackage()
        {

        }
        public static string ReciveGorevYPackage()
        {

        }

        public static bool CheckForPackage()
        {

        }
    
    }
}
