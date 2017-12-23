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
        public const byte REGISTER_CODE = 0x1;
        public const string REGISTER_ASM = "R";
        public const byte MEMORY_CODE = 0x2;
        public const string MEMORY_ASM = "M";
        public const byte IMMEDIATE_CODE = 0x3;
        public const string IMMEDIATE_ASM = "I";
        public const byte ADDRESS_CODE = 0x4;
        public const string ADDRESS_ASM = "A";

        public const int SIZE = 3;

        public static byte EncodeType(string type)
        {
            switch (type)
            {
                case PORT_ASM: return PORT_CODE;
                case REGISTER_ASM: return REGISTER_CODE;
                case MEMORY_ASM: return MEMORY_CODE;
                case IMMEDIATE_ASM: return IMMEDIATE_CODE;
                case ADDRESS_ASM:return ADDRESS_CODE;
                default: throw new Exception("Invalid location type");
            }
        }
        public static string DecodeType(byte type)
        {
            switch (type)
            {
                case PORT_CODE: return PORT_ASM;
                case REGISTER_CODE: return REGISTER_ASM;
                case MEMORY_CODE: return MEMORY_ASM;
                case IMMEDIATE_CODE: return IMMEDIATE_ASM;
                case ADDRESS_CODE: return ADDRESS_ASM;
                default: throw new Exception("Invalid location type");
            }
        }

        public static Location FromASM(string asm)
        {
            Debug.Assert(asm.Length > 1);
            asm = asm.Replace(" ", "");
            byte type = EncodeType(asm[0].ToString());
            ushort loc = 0;
            string locVal = asm.Substring(1);
            switch(type)
            {
                case PORT_CODE:
                    loc = byte.Parse(locVal);
                    break;
                case ADDRESS_CODE:
                case REGISTER_CODE:
                    loc = Register.FromASM(locVal);
                    break;
                case MEMORY_CODE:
                    loc = ReadNumber(locVal, true);
                    break;
                case IMMEDIATE_CODE:
                    loc = ReadNumber(locVal, false);
                    break;
            }

            return new Location(type, loc);
        }

        private static ushort ReadNumber(string val, bool allowChar)
        {
            ushort converted;
            if (allowChar && (val.StartsWith('\'') || val.StartsWith('"')))
            {
                Debug.Assert(val.Length > 1);
                converted = (byte)val[1];
            }
            else if (val.StartsWith("0x", StringComparison.InvariantCultureIgnoreCase))
            {
                Debug.Assert(val.Length > 2);
                converted = ushort.Parse(val.Substring(2), System.Globalization.NumberStyles.HexNumber);
            }
            else
            {
                converted = ushort.Parse(val);
            }
            return converted;
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
                    Debug.Assert(loc <= VM.PORTS);
                    return vm.Ports[loc].Read();
                case REGISTER_CODE:
                    Debug.Assert(loc <= VM.REGISTERS);
                    return vm.R[loc];
                case MEMORY_CODE:
                    return vm.Read(loc);
                case IMMEDIATE_CODE:
                    return loc;
                case ADDRESS_CODE:
                    Debug.Assert(loc <= VM.REGISTERS);
                    return vm.Read(vm.R[loc]);
                default: throw new Exception("Invalid location type");
            }
        }
        public void Write(VM vm, ushort val)
        {
            switch (type)
            {
                case PORT_CODE:
                    Debug.Assert(loc <= VM.PORTS);
                    vm.Ports[loc].Write((byte)(val & 0xFF));
                    break;
                case REGISTER_CODE:
                    Debug.Assert(loc <= VM.REGISTERS);
                    vm.R[loc] = val;
                    break;
                case MEMORY_CODE:
                    vm.Write(loc, (byte)(val & 0xFF));
                    break;
                case IMMEDIATE_CODE:
                    throw new Exception("Invalid operation");
                case ADDRESS_CODE:
                    Debug.Assert(loc <= VM.REGISTERS);
                    vm.Write(vm.R[loc], (byte)(val & 0xFF));
                    break;
                default:
                    throw new Exception("Invalid location type");
            }
        }

        public byte[] Encode()
        {
            return new byte[] { type, loc.HiByte(), loc.LoByte() };
        }

        public string ToASM()
        {
            switch(type)
            {
                case PORT_CODE:
                    return string.Format("{0}{1}", DecodeType(type), loc);
                case REGISTER_CODE:
                    return string.Format("R{0}", Register.ToASM((byte)loc));
                case ADDRESS_CODE:
                    return string.Format("A{0}", Register.ToASM((byte)loc));
                case IMMEDIATE_CODE:
                case MEMORY_CODE:
                default:
                    return string.Format("{0}0x{1:X}", DecodeType(type), loc);
            }
        }
    }
}
