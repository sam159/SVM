using System;
using System.Collections.Generic;
using System.Text;

namespace SVM.Instructions
{
    class JNZ : JZ
    {
        public override string ASM => "JNZ";

        public override byte OP => 0x53;

        public override void CheckJump(VM vm, byte reg, ushort loc)
        {
            if (vm.R[reg] != 0)
            {
                vm.PC = loc;
            }
        }
    }
}
