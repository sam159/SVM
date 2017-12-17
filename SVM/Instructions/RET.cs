using System;
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
            Debug.Assert(vm.SP > 0);

            vm.SP--;
            vm.PC = vm.STACK[vm.SP];
            vm.STACK[vm.SP] = 0;
        }

        public override string ToASM(byte[] vars)
        {
            throw new NotImplementedException();
        }
    }
}
