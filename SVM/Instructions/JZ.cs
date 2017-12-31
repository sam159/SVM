using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SVM.Instructions
{
    class JZ : Instruction
    {
        public override string ASM => "JZ";

        public override byte OP => 0x52;

        public override byte[] Encode(string asm, Dictionary<string, ushort> markerRefs)
        {
            var parts = asm.Split(" ", 2);
            Debug.Assert(parts.Length == 2);
                
            var reg = Register.FromASM(parts[0]);

            ushort loc = 0;

            if (parts[1].StartsWith(':') && markerRefs != null)
            {
                markerRefs.Add(parts[1].Substring(1), 2);
            }
            else if (parts[1].StartsWith("0x", StringComparison.InvariantCultureIgnoreCase))
            {
                loc = ushort.Parse(parts[1].Substring(2), System.Globalization.NumberStyles.HexNumber);
            }
            else
            {
                loc = ushort.Parse(parts[1]);
            }
            
            return new byte[] { OP, reg, loc.HiByte(), loc.LoByte() };
        }

        public override byte[] Decode(VM vm)
        {
            var code = vm.MEM.Subset(vm.PC, 3);
            vm.PC += 3;
            return code;
        }

        public override void Exec(VM vm, byte[] vars)
        {
            Debug.Assert(vars.Length == 3);
            byte reg = vars[0];
            if (reg <= VM.REGISTERS)
            {
                throw new Fault(FaultType.IllegalOp);
            }
            ushort loc = (ushort)((vars[1] << 8) + vars[2]);

            CheckJump(vm, reg, loc);
        }

        public virtual void CheckJump(VM vm, byte reg, ushort loc)
        {
            if (vm.R[reg] == 0)
            {
                vm.PC = loc;
            }
        }

        public override string ToASM(byte[] vars)
        {
            var reg = vars[0];
            ushort loc = (ushort)((vars[1] << 8) + vars[2]);
            return string.Format("{0} {1} 0x{2:x}", ASM, Register.ToASM(reg), loc);
        }
    }
}
