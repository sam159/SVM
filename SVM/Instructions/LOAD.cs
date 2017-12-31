using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Diagnostics;

namespace SVM.Instructions
{
    class LOAD : Instruction
    {
        public override string ASM => "LOAD";

        public override byte OP => 0x10;

        public override byte[] Encode(string asm, Dictionary<string, ushort> markerRefs)
        {
            var parts = asm.Split(" ");
            Debug.Assert(parts.Length > 1);
            var reg = Register.FromASM(parts[0]);
            var loc = Location.FromASM(String.Join(" ", parts.Skip(1)));
            return (new byte[] { OP, reg }).Concat(loc.Encode());
        }

        public override byte[] Decode(VM vm)
        {
            var pc = vm.PC;
            ushort size = 1 + Location.SIZE;
            vm.PC += size;
            return vm.MEM.Subset(pc, size);
        }

        public override void Exec(VM vm, byte[] vars)
        {
            Debug.Assert(vars.Length > 1);
            var reg = vars[0];
            Debug.Assert(reg <= VM.REGISTERS);
            var loc = Location.FromByteCode(vars, 1);
            Run(vm, reg, loc);
        }

        protected virtual void Run(VM vm, byte reg, Location loc)
        {
            vm.R[reg] = loc.Read(vm);
        }

        public override string ToASM(byte[] vars)
        {
            Debug.Assert(vars.Length > 1);
            var reg = vars[0];
            if (reg > VM.REGISTERS)
            {
                throw new Fault(FaultType.IllegalOp);
            }
            var loc = Location.FromByteCode(vars, 1);

            return string.Format("{0} {1} {2}", ASM, Register.ToASM(reg), loc.ToASM());
        }
    }
}
