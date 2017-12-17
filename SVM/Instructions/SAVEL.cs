using System;
using System.Collections.Generic;
using System.Text;

namespace SVM.Instructions
{
    class SAVEL : LOAD
    {
        public override string ASM => "SAVEL";
        public override byte OP => 0x15;

        protected override void Run(VM vm, byte reg, Location loc)
        {
            byte data = (byte)(vm.R[reg] & 0xFF);
            loc.Write(vm, data);
        }
    }
}
