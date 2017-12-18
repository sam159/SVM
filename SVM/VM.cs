using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace SVM
{
    class VM
    {
        public const int REGISTERS = 4;
        public const int PORTS = 8;
        public const int STACKDEPTH = 16;
        public const int MEMSIZE = 0xFFFF;

        public bool RUN;
        public ushort PC;
        public ushort[] R = new ushort[REGISTERS];
        public ushort RI;
        public byte SP;
        public ushort[] STACK = new ushort[STACKDEPTH];
        public byte[] MEM = new byte[MEMSIZE];
        public Port[] Ports = new Port[PORTS];

        public int CycleDelay = 0;

        private Dictionary<byte, Instruction> instructions = new Dictionary<byte, Instruction>();

        public VM()
        {
            var instrs = Instruction.GetAllInstructions();
            foreach(var instr in instrs)
            {
                instructions.Add(instr.OP, instr);
            }

            Ports[0] = new Ports.ConsolePort(this);
            Reset();
        }

        public void Reset()
        {
            PC = 0;
            SP = 0;
            RI = 0;
            Array.Fill<ushort>(R, 0);
            Array.Fill<ushort>(STACK, 0);
            Array.Fill<byte>(MEM, 0);
        }

        public void Load(byte[] data, byte origin)
        {
            Array.Copy(data, 0, MEM, origin, data.Length);
        }

        public void Run()
        {
            RUN = true;
            while(RUN)
            {
                Step();
                if (CycleDelay > 0)
                {
                    Thread.Sleep(CycleDelay);
                }
            }
        }

        public void Step()
        {
            if (RUN)
            {
                var pc = PC;
                var instr = instructions[MEM[PC++]];
                byte[] decoded = instr.Decode(this);
                Console.Write("{4:X4} | A{0:X3} B{1:X3} C{2:X3} D{3:X3} | ", R[0], R[1], R[2], R[3], pc);
                Console.WriteLine("0x{0:X4} {1}", pc, instr.ToASM(decoded));
                instr.Exec(this, decoded);
                //Console.ReadKey(true);
            }
        }
    }
}
