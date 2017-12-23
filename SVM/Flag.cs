using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SVM
{
    abstract class Flag
    {
        public static Flag[] GetAllFlags()
        {
            var types = from t in Assembly.GetAssembly(typeof(Flag)).GetTypes()
                        where t.IsSubclassOf(typeof(Flag))
                        select t;

            var flags = new List<Flag>();

            foreach (var t in types)
            {
                flags.Add((Flag)Activator.CreateInstance(t));
            }

            return flags.ToArray();
        }

        public abstract string ASM { get; }
        public abstract byte Address { get; }

        public abstract byte Read(VM vm);
        public abstract void Write(VM vm, byte val);
    }
}
