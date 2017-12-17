using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Diagnostics;

namespace SVM.Instructions
{
    class SAVE : LOAD
    {
        public override string ASM => "SAVE";

        public override byte OP => 0x11;

        protected override void Run(VM vm, byte reg, Location loc)
        {
            loc.Write(vm, vm.R[reg]);
        }
    }
}
