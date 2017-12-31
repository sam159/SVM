using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SVM.Instructions
{
    class POP : PUSH
    {
        public override string ASM => "POP";

        public override byte OP => 0x43;

        protected override void Run(VM vm, byte reg)
        {
            vm.R[reg] = vm.PopStack();
        }
    }
}
