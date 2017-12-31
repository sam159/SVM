using System;
using System.Diagnostics;
using System.IO;

namespace SVM
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: dotnet SVN.dll [Filename]");
                return;
            }
            var file = Path.Combine(Environment.CurrentDirectory, args[0]);

            if (!File.Exists(file))
            {
                Console.WriteLine("Error: File Not Found");
            }

            var content = File.ReadAllText(file);

            Console.WriteLine("Assembling {0}", file);

            var asm = new Assembler();

            var mem = asm.Compile(content);
            Console.WriteLine("Compiled to {0} bytes", mem.Length);

            var vm = new VM();
            vm.CycleDelay = 25;
            vm.Load(mem, 0);
            
            vm.Run();

            Console.ReadKey(true);
        }
    }
}
