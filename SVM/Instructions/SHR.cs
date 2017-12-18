using System;
using System.Collections.Generic;
using System.Text;

namespace SVM.Instructions
{
    class SHR : SHL
    {
        public override string ASM => "SHR";

        public override byte OP => 0x35;

        protected override void Run(VM vm, byte reg, byte bit)
        {
            vm.R[reg] = (ushort)(vm.R[reg] >> bit);
        }
    }
}
