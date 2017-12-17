using System;
using System.Collections.Generic;
using System.Text;

namespace SVM.Instructions
{
    class LOADH : LOAD
    {
        public override string ASM => "LOADH";
        public override byte OP => 0x12;

        protected override void Run(VM vm, byte reg, Location loc)
        {
            var data = loc.Read(vm);
            vm.R[reg] = (ushort)((data << 8) + (vm.R[reg] & 0xFF));
        }
    }
}
