using System;
using System.Collections.Generic;
using System.Text;

namespace SVM.Instructions
{
    class BTS : SHL
    {
        public override string ASM => "BTS";

        public override byte OP => 0x36;

        protected override void Run(VM vm, byte reg, byte bit)
        {
            bit--;
            vm.R[reg] = (ushort)(vm.R[reg] | 1 << bit);
        }
    }
}
