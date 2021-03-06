﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SVM.Instructions
{
    class RET : Instruction
    {
        public override string ASM => "RET";

        public override byte OP => 0x41;

        public override byte[] Encode(string asm, Dictionary<string, ushort> markerRefs)
        {
            return new byte[] { OP };
        }

        public override void Exec(VM vm, byte[] vars)
        {
            Debug.Assert(vars.Length == 0);

            vm.PC = vm.PopStack();
        }

        public override string ToASM(byte[] vars)
        {
            return ASM;
        }
    }
}
