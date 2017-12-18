using System;
using System.Collections.Generic;
using System.Text;

namespace SVM.Instructions
{
    class OR : ADD
    {
        public override string ASM => "OR";
        public override byte OP => 0x32;

        protected override void Run(VM vm, byte reg, Location loc)
        {
            vm.R[reg] |= loc.Read(vm);
        }
    }
}
