using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace SVM.Instructions
{
    class CALL : Instruction
    {
        public override string ASM => "CALL";

        public override byte OP => 0x40;

        public override byte[] Encode(string asm, Dictionary<string, ushort> markerRefs)
        {
            ushort jmp = 0;
            if (asm.StartsWith(':'))
            {
                markerRefs.Add(asm.Substring(1), 1);
            }
            else if (asm.StartsWith("0x", StringComparison.InvariantCultureIgnoreCase))
            {
                jmp = ushort.Parse(asm.Substring(2), System.Globalization.NumberStyles.HexNumber);
            }
            else
            {
                jmp = ushort.Parse(asm);
            }

            return new byte[] { OP, jmp.HiByte(), jmp.LoByte() };
        }

        public override byte[] Decode(VM vm)
        {
            var bc = vm.MEM.Subset(vm.PC, 2);
            vm.PC += 2;
            return bc;
        }

        public override void Exec(VM vm, byte[] vars)
        {
            Debug.Assert(vars.Length == 2);

            vm.PushStack(vm.PC);
            var jmp = (ushort)((vars[0] << 8) + vars[1]);
            vm.PC = jmp;
        }

        public override string ToASM(byte[] vars)
        {
            var jmp = (ushort)((vars[0] << 8) + vars[1]);
            return string.Format("{0} 0x{1:X}", ASM, jmp);
        }
    }
}
