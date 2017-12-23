using System;
using System.Collections.Generic;
using System.Text;

namespace SVM.Ports
{
    class ConsolePort : Port
    {
        public bool Enabled { get; set; }
        public bool ReadBlock { get; set; }

        public ConsolePort(VM vm) 
            : base(vm)
        {
            Enabled = true;
            ReadBlock = true;
        }

        public override ushort Read()
        {
            if (ReadBlock || Console.KeyAvailable)
            {
                var result = Console.ReadKey(true);
                return (byte)result.KeyChar;
            }
            return 0;
        }

        public override void Write(byte val)
        {
            if (Enabled)
            {
                Console.Write((char)val);
            }
        }
    }
}
