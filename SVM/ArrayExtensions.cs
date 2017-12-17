using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Linq;

namespace SVM
{
    static class ArrayExtensions
    {
        public static byte[] Subset(this byte[] array, int start, int length)
        {
            Debug.Assert(array.Length >= start + length);
            byte[] subset = new byte[length];
            Array.Copy(array, start, subset, 0, length);
            return subset;
        }
        public static byte[] Subset(this byte[] array, int start)
        {
            Debug.Assert(array.Length >= start);
            byte[] subset = new byte[array.Length - start];
            Array.Copy(array, start, subset, 0, array.Length - start);
            return subset;
        }
        public static byte[] Concat(this byte[] array, params byte[][] others)
        {
            var size = array.Length + others.Sum(o => o.Length);
            var result = new byte[size];

            var i = array.Length;
            Array.Copy(array, 0, result, 0, array.Length);
            foreach(var o in others)
            {
                Array.Copy(o, 0, result, i, o.Length);
                i += o.Length;
            }
            return result;
        }
        public static byte[] Prepend(this byte[] array, byte val)
        {
            var newArr = new byte[array.Length + 1];
            var len = array.Length;
            Array.Copy(array, 0, newArr, 1, array.Length);
            newArr[0] = val;
            return newArr;
        }
    }
}
