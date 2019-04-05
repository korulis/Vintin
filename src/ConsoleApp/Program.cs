using System;
using System.Collections.Generic;
using Discounts;
using Discounts.Discounters;

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

        private static FileBasedShippingCostCalculator PoorMansDi()
        {
            void OutputMethod(IEnumerable<string> x)
            {
                foreach (var xi in x)
                {
                    Console.WriteLine(xi);
                }
            }

            var processor = new FileBasedShippingCostCalculator(
                new ShippingEntryMapper(" ", "yyyy-MM-dd"),
                new ShippingCostCalculator(new TempNoDiscounts()),
                OutputMethod);
            return processor;
        }
    }

    internal class TempNoDiscounts : IDiscounter
    {
        public IEnumerable<ShippingCostEntry> Discount(IEnumerable<ShippingCostEntry> pricedShippingEntries)
        {
            throw new NotImplementedException();
        }
    }
}
