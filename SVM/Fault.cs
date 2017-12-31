using System;
using System.Collections.Generic;
using System.Text;

namespace SVM
{
    public enum FaultType
    {
        None,
        UndefinedOp,
        IllegalOp,
        StackExceeded,
        MemoryOverflow
    }

    class Fault : Exception
    {
        public FaultType Type { get; private set; }

        public Fault(FaultType type) : base()
        {
            Type = type;
        }

        public Fault(string message) : base(message)
        {

        }
    }
}
