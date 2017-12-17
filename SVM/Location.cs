using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SVM
{
    class Location
    {
        public const byte PORT_CODE = 0x0;
        public const string PORT_ASM = "P";
        public const byte REG_CODE = 0x1;
        public const string REG_ASM = "R";
        public const byte MEM_CODE = 0x2;
        public const string MEM_ASM = "M";
        public const byte LITERAL_CODE = 0x3;
        public const string LITERAL_ASM = "L";

        public const int SIZE = 3;

        public static byte EncodeType(string type)
        {
            switch (type)
            {
                case PORT_ASM: return PORT_CODE;
                case REG_ASM: return REG_CODE;
                case MEM_ASM: return MEM_CODE;
                case LITERAL_ASM: return LITERAL_CODE;
                default: throw new Exception("Invalid location type");
            }
        }
        public static string DecodeType(byte type)
        {
            switch (type)
            {
                case PORT_CODE: return PORT_ASM;
                case REG_CODE: return REG_ASM;
                case MEM_CODE: return MEM_ASM;
                case LITERAL_CODE: return LITERAL_ASM;
                default: throw new Exception("Invalid location type");
            }
        }

        public static Location FromASM(string asm)
        {
            var parts = asm.Split(" ", 2);
            Debug.Assert(parts.Length > 0);
            byte type = EncodeType(parts[0]);
            ushort loc = 0;
            Debug.Assert(parts.Length == 2);
            string locVal = parts[1];
            if (locVal.StartsWith('\'') || locVal.StartsWith('"'))
            {
                Debug.Assert(locVal.Length > 1);
                loc = (byte)locVal[1];
            }
            else if (locVal.StartsWith("0x", StringComparison.InvariantCultureIgnoreCase))
            {
                Debug.Assert(locVal.Length > 2);
                loc = ushort.Parse(locVal.Substring(2), System.Globalization.NumberStyles.HexNumber);
            }
            else
            {
                loc = ushort.Parse(locVal);
            }

            return new Location(type, loc);
        }
        public static Location FromByteCode(byte[] data, int start)
        {
            Debug.Assert(data.Length > start);
            var type = data[start];
            ushort loc = 0;
            Debug.Assert(data.Length > start + 1);
            loc = (ushort)((data[start + 1] << 8) + data[start + 2]);
            return new Location(type, loc);
        }

        private byte type;
        private ushort loc;

        public Location(byte type, ushort loc)
        {
            this.type = type;
            this.loc = loc;
        }

        public ushort Read(VM vm)
        {
            switch (type)
            {
                case PORT_CODE:
                    return vm.Ports[loc].Read();
                case REG_CODE:
                    return vm.R[loc];
                case MEM_CODE:
                    return vm.MEM[loc];
                case LITERAL_CODE:
                    return loc;
                default: throw new Exception("Invalid location type");
            }
        }
        public void Write(VM vm, ushort val)
        {
            switch (type)
            {
                case PORT_CODE:
                    vm.Ports[loc].Write((byte)val);
                    break;
                case REG_CODE:
                    vm.R[loc] = val;
                    break;
                case MEM_CODE:
                    vm.MEM[loc] = (byte)val;
                    break;
                case LITERAL_CODE:
                    throw new Exception("Invalid operation");
                default:
                    throw new Exception("Invalid location type");
            }
        }

        public byte[] Encode()
        {
            byte ub = (byte)((loc & 0xFF00) >> 8);
            byte lb = (byte)(loc & 0xFF);
            return new byte[] { type, ub, lb };
        }

        public string ToASM()
        {
            return string.Format("{0} 0x{1:X}", DecodeType(type), loc);
        }
    }
}
