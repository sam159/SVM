using System;
using System.Collections.Generic;
using System.Text;

namespace SVM.Instructions
{
    class JBS : JBC
    {
        public override string ASM => "JBS";

        public override byte OP => 0x54;

        protected override bool CheckJump(VM vm, byte reg, byte bit)
        {
            byte compare = (byte)(1 << bit - 1);
            return (vm.R[reg] & compare) == compare;
        }
    }
}
