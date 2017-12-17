using System;
using System.Collections.Generic;
using System.Text;

namespace SVM.Ports
{
    class ConsolePort : Port
    {
        public ConsolePort(VM vm) 
            : base(vm)
        { }

        public override ushort Read()
        {
            var result = Console.ReadKey(true);
            return (byte)result.KeyChar;
        }

        public override void Write(byte val)
        {
            Console.Write((char)val);
        }
    }
}
