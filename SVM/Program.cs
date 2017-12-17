using System;
using System.IO;

namespace SVM
{
    class Program
    {
        static void Main(string[] args)
        {
            var file = Path.Combine(Environment.CurrentDirectory, args[0]);

            var content = File.ReadAllText(file);

            Console.WriteLine("Assembling {0}", file);

            var asm = new Assembler();

            var mem = asm.Compile(content);

            var vm = new VM();
            vm.CycleDelay = 25;
            vm.Load(mem, 0);

            vm.Run();

            Console.ReadKey(true);
        }
    }
}
