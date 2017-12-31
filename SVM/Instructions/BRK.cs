using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SVM.Instructions
{
    class BRK : Instruction
    {
        public override string ASM => "BRK";

        public override byte OP => 0x61;

        public override byte[] Encode(string asm, Dictionary<string, ushort> markerRefs)
        {
            Debug.Assert(string.IsNullOrWhiteSpace(asm));
            return new byte[] { OP };
        }

        public override void Exec(VM vm, byte[] vars)
        {
            if (vm.DEBUG)
            {
                vm.RUN = false;
                vm.InvokeBreakpoint();
            }
        }

        public override string ToASM(byte[] vars)
        {
            Debug.Assert(vars.Length == 0);
            return ASM;
        }
    }
}
