using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SVM.Instructions
{
    class SETI : LOADI
    {
        public override string ASM => "SETI";

        public override byte OP => 0x65;

        protected override void Run(VM vm, byte reg)
        {
            vm.RI = vm.R[reg];
        }
    }
}
