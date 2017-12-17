using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace SVM.Instructions
{
    class DECI : Instruction
    {
        public override string ASM => "DECI";

        public override byte OP => 0x64;

        public override byte[] Encode(string asm, Dictionary<string, ushort> markerRefs)
        {
            return new byte[] { OP };
        }

        public override void Exec(VM vm, byte[] vars)
        {
            Debug.Assert(vars.Length == 0);
            vm.RI--;
        }

        public override string ToASM(byte[] vars)
        {
            Debug.Assert(vars.Length == 0);
            return ASM;
        }
    }
}
