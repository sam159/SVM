using System;
using System.Collections.Generic;
using System.Text;

namespace SVM.Instructions
{
    class MUL : ADD
    {
        public override string ASM => "SUB";

        public override byte OP => 0x24;
        
        protected override void Run(VM vm, byte reg, Location loc)
        {
            vm.R[reg] *= loc.Read(vm);
        }
    }
}
