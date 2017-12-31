using System;
using System.Collections.Generic;
using System.Text;

namespace SVM.Flags
{
    abstract class MemoryBackedFlag : Flag
    {
        public MemoryBackedFlag(VM vm) : base(vm) { }

        public override byte Read()
        {
            ushort addr = (ushort)(vm.FlagStart + Address);
            return vm.MEM[addr];
        }

        public override void Write(byte val)
        {
            ushort addr = (ushort)(vm.FlagStart + Address);
            vm.MEM[addr] = val;
        }
    }
}
