using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SVM
{
    abstract class Instruction
    {
        public static Instruction[] GetAllInstructions()
        {
            var types = from t in Assembly.GetAssembly(typeof(Instruction)).GetTypes()
                        where t.IsSubclassOf(typeof(Instruction))
                        select t;

            var instr = new List<Instruction>();

            foreach(var t in types)
            {
                instr.Add((Instruction)Activator.CreateInstance(t));
            }

            return instr.ToArray();
        }

        public abstract string ASM { get; }
        public abstract byte OP { get; }

        public virtual byte[] Encode(string asm)
        {
            return Encode(asm, null);
        }
        public abstract byte[] Encode(string asm, Dictionary<string, ushort> markerRefs);
        public abstract string ToASM(byte[] vars);
        public abstract void Exec(VM vm, byte[] vars);
        public virtual byte[] Decode(VM vm)
        {
            return new byte[0];
        }
    }
}
