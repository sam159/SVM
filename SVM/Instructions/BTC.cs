using System;
using System.Collections.Generic;
using System.Text;

namespace SVM.Instructions
{
    class BTC : SHL
    {
        public override string ASM => "BTC";

        public override byte OP => 0x37;

        protected override void Run(VM vm, byte reg, byte bit)
        {
            vm.R[reg] = (ushort)(vm.R[reg] & ~(1 << bit));
        }
    }
}
