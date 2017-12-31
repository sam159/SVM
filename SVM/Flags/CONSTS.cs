using SVM.Ports;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SVM.Flags
{
    class CONSTS : Flag
    {
        public const byte AVAILABLE = 1;
        public const byte ENABLED = 2;
        public const byte READBLOCK = 4;
        public const byte READAVAILABLE = 8;

        public override string ASM => "CONSTS";

        public override byte Address => 0x00;

        public CONSTS(VM vm) : base(vm) { }

        public override byte Read()
        {
            var port = vm.Ports[0] as ConsolePort;
            Debug.Assert(port != null);

            byte result = AVAILABLE;
            if (port.Enabled)
            {
                result |= ENABLED;
            }
            if (port.ReadBlock)
            {
                result |= READBLOCK;
            }
            if (Console.KeyAvailable)
            {
                result |= READAVAILABLE;
            }

            return result;
        }

        public override void Write(byte val)
        {
            var port = vm.Ports[0] as ConsolePort;
            Debug.Assert(port != null);

            port.Enabled = (val & ENABLED) == ENABLED;
            port.ReadBlock = (val & READBLOCK) == READBLOCK;
        }
    }
}
