using System;
using System.Collections.Generic;
using Discounts;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputFileName = args.Length == 0 ? "input.txt" : args[0];

            void OutputMethod(IEnumerable<string> x)
            {
                foreach (var xi in x)
                {
                    Console.WriteLine(xi);
                }
            }

            // poor man's DI.
            var processor = new FileBasedCashier(
                new ShippingEntryMapper(" ", "yyyy-MM-dd"),
                new ShippingCostsCalculator(),
                OutputMethod);

            processor.Process(inputFileName);

            Console.ReadKey();
        }
    }
}
