﻿using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using Discounts;
using Discounts.Discounters;
using Discounts.Rules;

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

        private static FileBasedShipmentProcessor PoorMansDi()
        {
            void ConsoleOutputMethod(IEnumerable<string> lines)
            {
                foreach (var line in lines)
                {
                    Console.WriteLine(line);
                }
            }

            var tenLimit = new TenLimitDiscounter(
                new ZeroDiscounter(),
                () => new MonthlyCapDiscountingRules(
                    Defaults.TenMonthlyCap.MonthlyCap));

            var thirdPackage = new ThirdLpPackageDiscounter(
                tenLimit, 
                () => new OncePerMonthDiscountingRules(
                    Defaults.ThirdLpPackageEveryMonth.SpecialProvider,
                    Defaults.ThirdLpPackageEveryMonth.SpecialSize,
                    Defaults.ThirdLpPackageEveryMonth.LuckOrderNumber));
            var smallPackage = new SmallPackageLowestPriceDiscounter(thirdPackage, Defaults.CostReference);

            var processor = new FileBasedShipmentProcessor(
                new ShipmentMapper(
                    Defaults.Separator,  
                    Defaults.DateFormats, 
                    Defaults.ShippingProviders,
                    Defaults.PackageSizes),
                new ShipmentCostCalculator(
                    smallPackage, 
                    Defaults.CostReference),
                ConsoleOutputMethod);
            return processor;
        }
    }
}
