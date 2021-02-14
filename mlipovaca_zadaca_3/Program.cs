using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mlipovaca_zadaca_3
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 1)
            {
                FileHelper.CheckConfigurationFile(args);
            }
            else
            {
                Console.WriteLine("--- Unijeli ste previše/premalo argumenata za pokretanje programa ---");
                Console.WriteLine("--- IZLAZAK IZ PROGRAMA ---");
                Console.ReadLine();
                Environment.Exit(0);
            }
        }
    }
}
