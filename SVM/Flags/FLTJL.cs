using System;
using System.Collections.Generic;
using System.Text;

namespace SVM.Flags
{
    class FLTJL : MemoryBackedFlag
    {
        public FLTJL(VM vm) : base(vm) { }

        public override string ASM => "FLTJL";

        public override byte Address => 0x12;
    }
}
