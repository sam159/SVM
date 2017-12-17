using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace SVM.Instructions
{
    class INC : CLR
    {
        public override string ASM => "INC";

        public override byte OP => 0x25;

        protected override void Run(VM vm, byte reg)
        {
            vm.R[reg]++;
        }
    }
}
