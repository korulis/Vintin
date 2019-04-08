﻿using Discounts;
using Discounts.Discounters;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace ConsoleApp.Tests
{
    public class AcceptanceTests
    {
        private static readonly Dictionary<(string, string), decimal> CostReference = Defaults.CostReference;
        private static readonly string[] AcceptableDateFormats = Defaults.DateFormats;
        private static readonly string[] AcceptableProviders = Defaults.ShippingProviders;
        private static readonly string[] AcceptableSizes = Defaults.PackageSizes;
        private const string Separator = Defaults.Separator;
        
        private const string InputFilePath = Defaults.InputFilePath;
        private const string OutputFilePath = "output.txt";

        public static TheoryData<string, IDiscounter> CalculatorData =>
            new TheoryData<string, IDiscounter>
            {
                {"free-shipping-output.txt" , new CompleteDiscounts()},
                {"greedy-shipping-output.txt" , new ZeroDiscounter()},
                {"discounted-shipping-output.txt" ,
                    new SmallPackageLowestPriceDiscounter(
                        new ThirdLpPackageDiscounter(
                            new TenLimitDiscounter(new ZeroDiscounter()),
                            CostReference),
                        CostReference)},
            };

        [Theory]
        [MemberData(nameof(CalculatorData))]
        public void ShipmentCostCalculationTests(string expectedOutputFile, IDiscounter discounter)
        {
            //Arrange
            var expectedDiscountedLines = File.ReadLines(expectedOutputFile).ToArray();
            var fileBasedShipmentProcessor = BuildProcessor(discounter);

            //Act
            fileBasedShipmentProcessor.Process(InputFilePath);

            //Assert
            var actualDiscountedEntries = File.ReadLines(OutputFilePath).ToArray();
            for (var i = 0; i < expectedDiscountedLines.Length; i++)
            {
                Assert.Equal(expectedDiscountedLines[i], actualDiscountedEntries[i]);
            }
            Assert.Equal(expectedDiscountedLines, actualDiscountedEntries);
        }

        private static FileBasedShipmentProcessor BuildProcessor(IDiscounter discounter)
        {

            var shipmentMapper = new ShipmentMapper(
                Separator,
                AcceptableDateFormats,
                AcceptableProviders,
                AcceptableSizes);
            var shipmentCostCalculator = new ShipmentCostCalculator(discounter, CostReference);
            void OutputMethod(IEnumerable<string> x) => File.WriteAllLines(OutputFilePath, x);

            var fileBasedShipmentProcessor = new FileBasedShipmentProcessor(
                shipmentMapper,
                shipmentCostCalculator,
                OutputMethod);
            return fileBasedShipmentProcessor;
        }
    }
}
