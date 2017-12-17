using System;
using System.Collections.Generic;
using System.Text;

namespace SVM.Instructions
{
    class SAVEH : LOAD
    {
        public override string ASM => "SAVEH";
        public override byte OP => 0x14;

        protected override void Run(VM vm, byte reg, Location loc)
        {
            byte data = (byte)((vm.R[reg] & 0xFF00) >> 8);
            loc.Write(vm, data);
        }
    }
}
