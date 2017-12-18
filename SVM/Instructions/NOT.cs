using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace SVM.Instructions
{
    class NOT : CLR
    {
        public override string ASM => "NOT";

        public override byte OP => 0x30;

        protected override void Run(VM vm, byte reg)
        {
            vm.R[reg] = (ushort)~vm.R[reg];
        }
    }
}
