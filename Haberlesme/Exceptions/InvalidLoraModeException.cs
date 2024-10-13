using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haberlesme.Exceptions
{
    internal class InvalidLoraModeException:Exception
    {
        public InvalidLoraModeException(string operation) 
            : base($"Your lora module cannot execute this {operation} opreation becouse it's current operating mode") 
        {
            
        }
        
    }
}
