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

            var processor = PoorMansDi();

            processor.Process(inputFileName);

            Console.ReadKey();
        }

        private static FileBasedCashier PoorMansDi()
        {
            void OutputMethod(IEnumerable<string> x)
            {
                foreach (var xi in x)
                {
                    Console.WriteLine(xi);
                }
            }

            var processor = new FileBasedCashier(
                new ShippingEntryMapper(" ", "yyyy-MM-dd"),
                new ShippingPriceCalculator(),
                OutputMethod);
            return processor;
        }
    }
}
