using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace odbcman
{
    class HammingWeightHelper
    {
        //types and constants used in the functions below
        //long is an unsigned 64-bit integer variable type (defined in C99 version of C language)
        const ulong m1 = 0x5555555555555555; //binary: 0101...
        const ulong m2 = 0x3333333333333333; //binary: 00110011..
        const ulong m4 = 0x0f0f0f0f0f0f0f0f; //binary:  4 zeros,  4 ones ...
        const ulong m8 = 0x00ff00ff00ff00ff; //binary:  8 zeros,  8 ones ...
        const ulong m16 = 0x0000ffff0000ffff; //binary: 16 zeros, 16 ones ...
        const ulong m32 = 0x00000000ffffffff; //binary: 32 zeros, 32 ones
        const ulong hff = 0xffffffffffffffff; //binary: all ones
        const ulong h01 = 0x0101010101010101; //the sum of 256 to the power of 0,1,2,3...

        public int booleansToInt(bool[] arr)
        {
            int n = 0;
            foreach (bool b in arr)
                n = (n << 1) | (b ? 1 : 0);
            return n;
        }

        //This is a naive implementation, shown for comparison,
        //and to help in understanding the better functions.
        //This algorithm uses 24 arithmetic operations (shift, add, and).
        public int popcount64a(ulong x)
        {
            x = (x & m1) + ((x >> 1) & m1); //put count of each  2 bits into those  2 bits 
            x = (x & m2) + ((x >> 2) & m2); //put count of each  4 bits into those  4 bits 
            x = (x & m4) + ((x >> 4) & m4); //put count of each  8 bits into those  8 bits 
            x = (x & m8) + ((x >> 8) & m8); //put count of each 16 bits into those 16 bits 
            x = (x & m16) + ((x >> 16) & m16); //put count of each 32 bits into those 32 bits 
            x = (x & m32) + ((x >> 32) & m32); //put count of each 64 bits into those 64 bits 
            return (int)x;
        }
    }
}
