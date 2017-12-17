using System;
using System.Collections.Generic;
using System.Text;

namespace SVM
{
    static class Register
    {
        public static byte FromASM(string name)
        {
            switch(name.ToUpper())
            {
                case "A": return 0;
                case "B": return 1;
                case "C": return 2;
                case "D": return 3;
                default: throw new Exception("Unknown Register: "+name);
            }
        }

        public static string ToASM(byte code)
        {
            switch(code)
            {
                case 0: return "A";
                case 1: return "B";
                case 2: return "C";
                case 3: return "D";
                default: throw new Exception(string.Format("Unknown Register Code: {0}", code));
            }
        }
    }
}
