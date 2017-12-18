using System;
using System.Collections.Generic;
using System.Text;

namespace SVM.Instructions
{
    class AND : ADD
    {
        public override string ASM => "AND";
        public override byte OP => 0x31;

        protected override void Run(VM vm, byte reg, Location loc)
        {
            vm.R[reg] &= loc.Read(vm);
        }
    }
}
