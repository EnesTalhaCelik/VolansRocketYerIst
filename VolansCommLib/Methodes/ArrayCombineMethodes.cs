using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolansCommLib.Methodes
{
    internal static class ArrayCombineMethodes
    {
        internal static byte[]  combineTwoArrays(byte[] array1, byte[] array2)
        {
            byte[] buffer = new byte[array1.Length+array2.Length];
            Array.Copy(buffer, array1, array1.Length);
            Array.Copy(buffer,array1.Length, array2, 0, array2.Length);
            return buffer;
        }

        internal static byte[] combineThreeArrays(byte[] array1, byte[] array2, byte[] array3)
        {
            byte[] buffer = new byte[array1.Length + array2.Length+array3.Length];
            Array.Copy(buffer, array1, array1.Length);
            Array.Copy(buffer, array1.Length, array2, 0, array2.Length);
            Array.Copy(buffer, array1.Length+array2.Length , array3, 0, array3.Length);
            return buffer;

        }
    }
}
