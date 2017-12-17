using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SVM.Instructions
{
    class NOP : Instruction
    {
        public override string ASM => "NOOP";

        public override byte OP => 0x00;

        public override byte[] Encode(string asm, Dictionary<string, ushort> markerRefs)
        {
            return new byte[] { OP };
        }

        public override void Exec(VM vm, byte[] vars)
        {
            Debug.Assert(vars.Length == 0);
            return;
        }

        public override string ToASM(byte[] vars)
        {
            Debug.Assert(vars.Length == 0);
            return ASM;
        }
    }
}
