using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace SVM.Instructions
{
    class DEC : CLR
    {
        public override string ASM => "DEC";

        public override byte OP => 0x26;

        protected override void Run(VM vm, byte reg)
        {
            vm.R[reg]--;
        }
    }
}
