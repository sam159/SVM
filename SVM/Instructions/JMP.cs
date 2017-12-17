using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SVM.Instructions
{
    class JMP : Instruction
    {
        public override string ASM => "JMP";

        public override byte OP => 0x51;
        
        public override byte[] Encode(string asm, Dictionary<string, ushort> markerRefs)
        {
            ushort loc = 0;

            if (asm.StartsWith(':') && markerRefs != null)
            {
                markerRefs.Add(asm.Substring(1), 1);
            }
            else if (asm.StartsWith("0x", StringComparison.InvariantCultureIgnoreCase))
            {
                loc = ushort.Parse(asm.Substring(2), System.Globalization.NumberStyles.HexNumber);
            }
            else
            {
                loc = ushort.Parse(asm);
            }

            return new byte[] { OP, loc.HiByte(), loc.LoByte() };
        }

        public override byte[] Decode(VM vm)
        {
            var code = vm.MEM.Subset(vm.PC, 2);
            vm.PC += 2;
            return code;
        }

        public override void Exec(VM vm, byte[] vars)
        {
            Debug.Assert(vars.Length == 2);
            ushort loc = (ushort)((vars[0] << 8) + vars[1]);
            vm.PC = loc;
        }

        public override string ToASM(byte[] vars)
        {
            ushort loc = (ushort)((vars[0] << 8) + vars[1]);
            return string.Format("{0} 0x{1:x}", ASM, loc);
        }
    }
}
