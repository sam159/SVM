using System;
using System.Collections.Generic;
using System.Text;

namespace SVM
{
    static class NumberExtensions
    {
        public static byte HiByte(this ushort num)
        {
            return (byte)((num & 0xFF00) >> 8);
        }

        public static byte LoByte(this ushort num)
        {
            return (byte)(num & 0xFF);
        }
    }
}
