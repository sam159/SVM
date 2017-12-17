using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SVM.Instructions
{
    class HALT : Instruction
    {
        public override string ASM => "HALT";

        public override byte OP => 0x50;

        public override byte[] Encode(string asm, Dictionary<string, ushort> markerRefs)
        {
            return new byte[] { OP };
        }

        public override void Exec(VM vm, byte[] vars)
        {
            Debug.Assert(vars.Length == 0);
            vm.RUN = false;
            vm.Ports[0].Write(Encoding.ASCII.GetBytes("\r\nSYSTEM HALTED"));
        }

        public override string ToASM(byte[] vars)
        {
            return ASM;
        }
    }
}
