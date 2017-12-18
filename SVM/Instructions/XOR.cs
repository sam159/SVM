using System;
using System.Collections.Generic;
using System.Text;

namespace SVM.Instructions
{
    class XOR : ADD
    {
        public override string ASM => "XOR";

        public override byte OP => 0x33;

        protected override void Run(VM vm, byte reg, Location loc)
        {
            vm.R[reg] ^= loc.Read(vm);
        }
    }
}
