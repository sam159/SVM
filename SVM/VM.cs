using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using SVM.Flags;

namespace SVM
{
    class VM
    {
        public const int REGISTERS = 4;
        public const int PORTS = 255;
        public const int STACKDEPTH = 16;
        public const int MEMSIZE = 0xFFFF;

        public bool RUN;
        public bool DEBUG;
        public ushort PC;
        public ushort[] R = new ushort[REGISTERS];
        public ushort RI;
        public byte SP;
        public ushort[] STACK = new ushort[STACKDEPTH];
        public byte[] MEM = new byte[MEMSIZE];
        public Port[] Ports = new Port[PORTS];
        public ushort FlagStart = 0xFF00;

        public ulong InstructionCount = 0;
        public int CycleDelay = 0;

        private Dictionary<byte, Instruction> instructions = new Dictionary<byte, Instruction>();
        private Dictionary<byte, Flag> flags = new Dictionary<byte, Flag>();
        private Dictionary<Type, byte> flagTypeMap = new Dictionary<Type, byte>();

        public event EventHandler Breakpoint;
        
        public VM()
        {
            foreach(var instr in Instruction.GetAllInstructions())
            {
                instructions.Add(instr.OP, instr);
            }
            foreach(var flag in Flag.GetAllFlags(this))
            {
                flags.Add(flag.Address, flag);
                flagTypeMap.Add(flag.GetType(), flag.Address);
            }

            Ports[0] = new Ports.ConsolePort(this);
            Reset();
        }

        public void Reset()
        {
            PC = 0;
            SP = 0;
            RI = 0;
            InstructionCount = 0;
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
                var nextPC = PC++;
                try
                {
                    InstructionCount++;
                    if (PC == 0xFFFF)
                    {
                        throw new Fault(FaultType.MemoryOverflow);
                    }
                    var op = MEM[nextPC];
                    if (!instructions.ContainsKey(op))
                    {
                        throw new Fault(FaultType.UndefinedOp);
                    }
                    var instr = instructions[op];
                    byte[] decoded = instr.Decode(this);
                    //Console.Write("{4:X4} | A{0:X3} B{1:X3} C{2:X3} D{3:X3} | ", R[0], R[1], R[2], R[3], pc);
                    //Console.WriteLine("{0}", instr.ToASM(decoded));
                    instr.Exec(this, decoded);
                    //Console.ReadKey(true);
                } catch(Fault flt)
                {
                    FLTSTS faultStatus = GetFlag<FLTSTS>();
                    if (!faultStatus.Trip(flt))
                    {
                        //Halt system as trip failed (already tripped or not enabled)
                        var nextBytes = BitConverter.ToString(MEM.Subset(nextPC, 4)).Replace("-", " ");
                        Ports[0].Write(Encoding.ASCII.GetBytes(
                            string.Format("Unhandled Fault [{0}] at 0x{1:X2}: next bytes {2}.", flt.Type, nextPC, nextBytes)
                            ));
                        RUN = false;
                    } else
                    {
                        //Tripped ok. Get and jump to handler
                        FLTJH faultJMPH = GetFlag<FLTJH>();
                        FLTJL faultJMPL = GetFlag<FLTJL>();
                        //Push current PC onto stack
                        PushStack(PC);
                        PC = (ushort)((faultJMPH.Read() << 8) + faultJMPL.Read()); 
                    }
                }
            }
        }

        #region Memory

        public byte Read(ushort location)
        {
            if (location >= FlagStart && location < FlagStart + 0xFF)
            {
                byte flagAddress = (byte)(location - FlagStart);
                if (flags.ContainsKey(flagAddress))
                {
                    return flags[flagAddress].Read();
                }
                return 0;
            }
            return MEM[location];
        }
        public void Write(ushort location, byte val)
        {
            if (location >= FlagStart && location < FlagStart + 0xFF)
            {
                byte flagAddress = (byte)(location - FlagStart);
                if (flags.ContainsKey(flagAddress))
                {
                    flags[flagAddress].Write(val);
                }
            } else
            {
                MEM[location] = val;
            }
        }

        public void PushStack(ushort val)
        {
            if (SP > VM.STACKDEPTH)
            {
                throw new Fault(FaultType.StackExceeded);
            }

            STACK[SP++] = val;
        }
        public ushort PopStack()
        {
            if (SP <= 0)
            {
                throw new Fault(FaultType.StackExceeded);
            }
            var val = STACK[--SP];
            STACK[SP] = 0;
            return val;
        }

        #endregion

        public void InvokeBreakpoint()
        {
            Breakpoint?.Invoke(this, new EventArgs());
        }

        public T GetFlag<T>() where T : Flag
        {
            if (flagTypeMap.ContainsKey(typeof(T)))
            {
                return flags[flagTypeMap[typeof(T)]] as T;
            }
            return default(T);
        }
    }
}
