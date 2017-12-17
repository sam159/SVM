using System;
using System.Collections.Generic;
using System.Text;

namespace SVM
{
    abstract class Port
    {
        private VM vm;

        public Port(VM vm)
        {
            this.vm = vm;
        }

        public abstract ushort Read();
        public abstract void Write(byte val);
        public virtual void Write(byte[] array)
        {
            foreach(var x in array)
            {
                Write(x);
            }
        }
    }
}
