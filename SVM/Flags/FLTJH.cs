using System;
using System.Collections.Generic;
using System.Text;

namespace SVM.Flags
{
    class FLTJH : MemoryBackedFlag
    {
        public FLTJH(VM vm) : base(vm) { }

        public override string ASM => "FLTJH";

        public override byte Address => 0x11;
    }
}
