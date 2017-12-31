using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SVM.Instructions
{
    class PUSH : Instruction
    {
        public override string ASM => "PUSH";

        public override byte OP => 0x42;

        public override byte[] Encode(string asm, Dictionary<string, ushort> markerRefs)
        {
            var parts = asm.Split(" ");
            Debug.Assert(parts.Length == 1);
            var reg = Register.FromASM(parts[0]);

            return new byte[] { OP, reg };
        }

        public override byte[] Decode(VM vm)
        {
            return vm.MEM.Subset(vm.PC++, 1);
        }

        public override void Exec(VM vm, byte[] vars)
        {
            Debug.Assert(vars.Length == 1);
            var reg = vars[0];
            if (reg > VM.REGISTERS)
            {
                throw new Fault(FaultType.IllegalOp);
            }
            Run(vm, reg);
        }

        protected virtual void Run(VM vm, byte reg)
        {
            if (vm.SP > VM.STACKDEPTH)
            {
                throw new Fault(FaultType.StackExceeded);
            }
            vm.PushStack(vm.R[reg]);
            vm.R[reg] = 0;
        }

        public override string ToASM(byte[] vars)
        {
            Debug.Assert(vars.Length == 1);
            var reg = vars[0];

            return string.Format("{0} {1}", ASM, Register.ToASM(reg));
        }
    }
}
