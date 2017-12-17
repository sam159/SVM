using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace SVM.Instructions
{
    class CLRI : Instruction
    {
        public override string ASM => "CLRI";

        public override byte OP => 0x62;

        public override byte[] Encode(string asm, Dictionary<string, ushort> markerRefs)
        {
            return new byte[] { OP };
        }

        public override void Exec(VM vm, byte[] vars)
        {
            Debug.Assert(vars.Length == 0);
            vm.RI = 0;
        }

        public override string ToASM(byte[] vars)
        {
            Debug.Assert(vars.Length == 0);
            return ASM;
        }
    }
}
