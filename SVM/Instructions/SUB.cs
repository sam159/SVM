using System;
using System.Collections.Generic;
using System.Text;

namespace SVM.Instructions
{
    class SUB : ADD
    {
        public override string ASM => "SUB";

        public override byte OP => 0x22;
        
        protected override void Run(VM vm, byte reg, Location loc)
        {
            vm.R[reg] -= loc.Read(vm);
        }
    }
}
