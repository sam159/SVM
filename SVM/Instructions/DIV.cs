using System;
using System.Collections.Generic;
using System.Text;

namespace SVM.Instructions
{
    class DIV : ADD
    {
        public override string ASM => "DIV";

        public override byte OP => 0x23;
        
        protected override void Run(VM vm, byte reg, Location loc)
        {
            vm.R[reg] /= loc.Read(vm);
        }
    }
}
