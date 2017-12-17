using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SVM.Instructions
{
    class LOADI : Instruction
    {
        public override string ASM => "LOADI";

        public override byte OP => 0x60;

        public override byte[] Encode(string asm, Dictionary<string, ushort> markerRefs)
        {
            var reg = Register.FromASM(asm);
            return new byte[] { OP, reg };
        }

        public override byte[] Decode(VM vm)
        {
            var reg = vm.MEM[vm.PC];
            vm.PC++;
            return new byte[] { reg };
        }

        public override void Exec(VM vm, byte[] vars)
        {
            Debug.Assert(vars.Length == 1);
            var reg = vars[0];
            Debug.Assert(reg <= VM.REGISTERS);
            Run(vm, reg);
        }

        protected virtual void Run(VM vm, byte reg)
        {
            vm.R[reg] = vm.MEM[vm.RI];
        }

        public override string ToASM(byte[] vars)
        {
            return string.Format("{0} {1}", ASM, Register.ToASM(vars[0]));
        }
    }
}
