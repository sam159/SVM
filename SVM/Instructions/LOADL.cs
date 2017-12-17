using System;
using System.Collections.Generic;
using System.Text;

namespace SVM.Instructions
{
    class LOADL : LOAD
    {
        public override string ASM => "LOADL";
        public override byte OP => 0x13;

        protected override void Run(VM vm, byte reg, Location loc)
        {
            var data = loc.Read(vm);
            vm.R[reg] = (ushort)((vm.R[reg] & 0xFF00) + data);
        }
    }
}
