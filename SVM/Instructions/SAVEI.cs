using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SVM.Instructions
{
    class SAVEI : LOADI
    {
        public override string ASM => "SAVEI";

        public override byte OP => 0x61;

        protected override void Run(VM vm, byte reg)
        {
            vm.MEM[vm.RI] = (byte)vm.R[reg];
        }
    }
}
