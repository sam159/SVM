using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace SVM.Instructions
{
    class CLR : Instruction
    {
        public override string ASM => "CLR";

        public override byte OP => 0x20;

        public override byte[] Encode(string asm, Dictionary<string, ushort> markerRefs)
        {
            Debug.Assert(asm.Length == 1);
            byte reg = Register.FromASM(asm);
            return new byte[] { OP, reg };
        }

        public override byte[] Decode(VM vm)
        {
            return new byte[] { vm.MEM[vm.PC++] };
        }

        public override void Exec(VM vm, byte[] vars)
        {
            Debug.Assert(vars.Length == 1);
            byte reg = vars[0];
            Debug.Assert(reg <= VM.REGISTERS);
            Run(vm, reg);
        }

        protected virtual void Run(VM vm, byte reg)
        {
            vm.R[reg] = 0;
        }

        public override string ToASM(byte[] vars)
        {
            Debug.Assert(vars.Length == 1);
            return string.Format("{0} {1}", ASM, Register.ToASM(vars[0]));
        }
    }
}
