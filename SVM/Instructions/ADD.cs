using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Linq;

namespace SVM.Instructions
{
    class ADD : Instruction
    {
        public override string ASM => "ADD";

        public override byte OP => 0x21;

        public override byte[] Encode(string asm, Dictionary<string, ushort> markerRefs)
        {
            var parts = asm.Split(" ", 2);
            Debug.Assert(parts.Length == 2);
            var reg = Register.FromASM(parts[0]);

            var bc = new byte[] { OP, reg };

            return bc.Concat(Location.FromASM(parts[1]).Encode());
        }

        public override byte[] Decode(VM vm)
        {
            var pc = vm.PC;
            vm.PC += 1 + Location.SIZE;
            return vm.MEM.Subset(pc, 1 + Location.SIZE);
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
            vm.R[reg] += loc.Read(vm);
        }

        public override string ToASM(byte[] vars)
        {
            Debug.Assert(vars.Length == 1 + Location.SIZE);
            var reg = vars[0];
            var loc = Location.FromByteCode(vars, 1);
            return string.Format("{0} {1} {2}", ASM, Register.ToASM(reg), loc.ToASM());
        }
    }
}
