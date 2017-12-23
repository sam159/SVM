using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SVM.Instructions
{
    class JBC : Instruction
    {
        public override string ASM => "JBC";

        public override byte OP => 0x55;

        public override byte[] Encode(string asm, Dictionary<string, ushort> markerRefs)
        {
            var parts = asm.Split(' ');
            Debug.Assert(parts.Length == 3);

            byte reg = Register.FromASM(parts[0]);

            byte bit = byte.Parse(parts[1]);
            Debug.Assert(bit >= 1 && bit <= 8);

            ushort loc = 0;
            if (parts[2].StartsWith(':') && markerRefs != null)
            {
                markerRefs.Add(parts[2].Substring(1), 3);
            }
            else if (parts[2].StartsWith("0x", StringComparison.InvariantCultureIgnoreCase))
            {
                loc = ushort.Parse(parts[2].Substring(2), System.Globalization.NumberStyles.HexNumber);
            }
            else
            {
                loc = ushort.Parse(parts[2]);
            }

            return new byte[] { OP, reg, bit, loc.HiByte(), loc.LoByte() };
        }

        public override byte[] Decode(VM vm)
        {
            var bc = vm.MEM.Subset(vm.PC, 4);
            vm.PC += 4;
            return bc;
        }

        public override void Exec(VM vm, byte[] vars)
        {
            Debug.Assert(vars.Length == 4);

            byte reg = vars[0];
            Debug.Assert(reg <= VM.REGISTERS);

            byte bit = vars[1];
            Debug.Assert(bit >= 1 && bit <= 8);

            ushort loc = (ushort)((vars[2] << 8) + vars[3]);

            if (CheckJump(vm, reg, bit))
            {
                vm.PC = loc;
            }
        }

        protected virtual bool CheckJump(VM vm, byte reg, byte bit)
        {
            byte compare = (byte)(1 << bit - 1);
            return (vm.R[reg] & compare) == 0;
        }

        public override string ToASM(byte[] vars)
        {
            Debug.Assert(vars.Length == 4);
            byte reg = vars[0];
            Debug.Assert(reg <= VM.REGISTERS);
            byte bit = vars[1];
            ushort loc = (ushort)((vars[2] << 8) + vars[3]);

            return string.Format("{0} {1} {2} {3}", ASM, Register.ToASM(reg), bit, string.Format("0x{0:X}", loc));
        }
    }
}
