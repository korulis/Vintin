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

            Console.Write("Hit any key.");
            Console.Read();
        }

        private static FileBasedShippingCostCalculator PoorMansDi()
        {
            void ConsoleOutputMethod(IEnumerable<string> lines)
            {
                foreach (var line in lines)
                {
                    Console.WriteLine(line);
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
