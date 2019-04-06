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

            Console.Read();
        }

        private static FileBasedShippingCostCalculator PoorMansDi()
        {
            void ConsoleOutputMethod(IEnumerable<string> x)
            {
                foreach (var xi in x)
                {
                    Console.WriteLine(xi);
                }
            }

            var tenLimit = new TenLimitDiscounter(new ZeroDiscounter());
            var thirdPackage = new ThirdLpPackageDiscounter(tenLimit);
            var smallPackage = new SmallPackageLowestPriceDiscounter(thirdPackage, Constants.CostReference);

            var processor = new FileBasedShippingCostCalculator(
                new ShippingEntryMapper(" ", "yyyy-MM-dd"),
                new ShippingCostCalculator(
                    smallPackage, 
                    Constants.CostReference),
                ConsoleOutputMethod);
            return processor;
        }
    }
}
