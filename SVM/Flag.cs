﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SVM
{
    abstract class Flag
    {
        public static Flag[] GetAllFlags(VM vm)
        {
            var types = from t in Assembly.GetAssembly(typeof(Flag)).GetTypes()
                        where t.IsSubclassOf(typeof(Flag))
                        select t;

            var flags = new List<Flag>();

            foreach (var t in types)
            {
                if (!t.IsAbstract)
                {
                    flags.Add((Flag)Activator.CreateInstance(t, vm));
                }
            }

            return flags.ToArray();
        }

        protected VM vm;

        public Flag(VM vm)
        {
            this.vm = vm;
        }

        public abstract string ASM { get; }
        public abstract byte Address { get; }

        public abstract byte Read();
        public abstract void Write(byte val);
    }
}
