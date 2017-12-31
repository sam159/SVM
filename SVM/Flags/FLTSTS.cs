using System;
using System.Collections.Generic;
using System.Text;

namespace SVM.Flags
{
    class FLTSTS : Flag
    {
        public const byte ENABLED_BIT = 1 << 0;
        public const byte TRIP_BIT = 1 << 1;
        public const byte UNDEFINEDOP_BIT = 1 << 2;
        public const byte ILLEGALOP_BIT = 1 << 3;
        public const byte STACKEXCEEDED_BIT = 1 << 4;
        public const byte MEMORYOVERFLOW_BIT = 1 << 5;

        public FLTSTS(VM vm) : base(vm)
        {
        }

        public override string ASM => "FLTSTS";

        public override byte Address => 0x10;

        public bool Enabled { get; private set; }
        public bool Tripped { get; private set; }

        private FaultType Type = FaultType.None;

        public bool Trip(Fault flt)
        {
            if (Tripped || !Enabled)
            {
                return false;
            }
            Tripped = true;
            Type = flt.Type;
            return true;
        }

        public override byte Read()
        {
            byte result = 0;

            if (Enabled)
            {
                result |= ENABLED_BIT;
            }
            if (Tripped)
            {
                result |= TRIP_BIT;
                switch(Type)
                {
                    case FaultType.IllegalOp:
                        result |= ILLEGALOP_BIT;
                        break;
                    case FaultType.UndefinedOp:
                        result |= UNDEFINEDOP_BIT;
                        break;
                    case FaultType.StackExceeded:
                        result |= STACKEXCEEDED_BIT;
                        break;
                    case FaultType.MemoryOverflow:
                        result |= MEMORYOVERFLOW_BIT;
                        break;
                }
            }

            return result;
        }

        public override void Write(byte val)
        {
            Enabled = (val & ENABLED_BIT) == ENABLED_BIT;
            if ((val & TRIP_BIT) == 0)
            {
                Tripped = false;
                Type = FaultType.None;
            }
        }
    }
}
