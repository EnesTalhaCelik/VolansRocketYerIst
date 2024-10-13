using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolansCommLib.Methodes
{
    internal static class SearchMethodes
    {

        internal static int SearchByteArray(byte[] sourceArray, byte[] searchArray)
        {
            if (sourceArray == null || searchArray == null || searchArray.Length == 0 || searchArray.Length > sourceArray.Length)
                return -1;


            for (int i = 0; i <= sourceArray.Length - searchArray.Length; i++) 
            {

                bool found = true;
                for (int j = 0; j < searchArray.Length; j++)
                {
                    if (sourceArray[i + j] != searchArray[j])
                    {
                        found = false;
                        break;
                    }
                }
                if (found)
                    return i; // Return the starting index
            }
            return -1; // Return -1 if not found  
    }


    }
}
