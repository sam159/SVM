using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SVM.Instructions
{
    class SHL : Instruction
    {
        public override string ASM => "SHL";

        public override byte OP => 0x34;

        public override byte[] Encode(string asm, Dictionary<string, ushort> markerRefs)
        {
            var parts = asm.Split(" ");
            Debug.Assert(parts.Length == 2);
            byte reg = Register.FromASM(parts[0]);
            byte bit = byte.Parse(parts[1]);
            Debug.Assert(bit < 8);

            return new byte[] { OP, reg, bit };
        }

        public override byte[] Decode(VM vm)
        {
            var bc = vm.MEM.Subset(vm.PC, 2);
            vm.PC += 2;
            return bc;
        }

        public override void Exec(VM vm, byte[] vars)
        {
            Debug.Assert(vars.Length == 2);

            byte reg = vars[0];
            byte bit = vars[1];

            Debug.Assert(reg <= VM.REGISTERS);
            Debug.Assert(bit < 8);

            Run(vm, reg, bit);
        }

        protected virtual void Run(VM vm, byte reg, byte bit)
        {
            vm.R[reg] = (ushort)(vm.R[reg] << bit);
        }

        public override string ToASM(byte[] vars)
        {
            Debug.Assert(vars.Length == 2);

            byte reg = vars[0];
            byte bit = vars[1];

            return string.Format("{0} {1} {2}", ASM, Register.ToASM(reg), bit);
        }
    }
}
