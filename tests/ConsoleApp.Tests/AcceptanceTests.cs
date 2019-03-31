using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Xunit;

namespace ConsoleApp.Tests
{
    public class AcceptanceTests
    {
        public class ShippingEntry
        {
            private readonly IReadOnlyList<string> _lineElements;
            public bool IsCorrupt { get; }
            public DateTime Date { get; set; }
            public string PackageSize { get; set; }
            public string ShippingProvider { get; set; }

            public ShippingEntry()
            {
                IsCorrupt = false;
            }

            private ShippingEntry(IReadOnlyList<string> lineElements)
            {
                _lineElements = lineElements;
                IsCorrupt = true;
            }

            public static ShippingEntry Corrupt(IReadOnlyList<string> lineElements)
            {
                return new ShippingEntry(lineElements);
            }

            public override string ToString()
            {
                if (IsCorrupt)
                {
                    return _lineElements[0] + " " + _lineElements[1];
                }
                else
                {
                    return $"{Date.ToShortDateString()} {PackageSize} {ShippingProvider}";
                }
            }
        }

        [Fact]
        public void FreeShippingTestCase()
        {
            //Arrange
            var shippingEntryLines = File.ReadLines("input.txt");
            var expectedDiscountedLines = File.ReadLines("free-shipping-output.txt").ToArray();
            var discounter = new ShippingCostCalculator();

            //Act
            var shippingEntries = shippingEntryLines
                .Select(x => x.Split(' '))
                .Select(Parse)
                .ToList();
            var actualDiscountedEntries = discounter.Discount(shippingEntries).Select(x => x.ToString()).ToArray();

            //Assert

            Assert.Equal(expectedDiscountedLines.Length, actualDiscountedEntries.Length);
            for (var i = 0; i < expectedDiscountedLines.Length; i++)
            {
                Assert.Equal(expectedDiscountedLines[i], actualDiscountedEntries[i]);
            }

            //Assert.Equal(expectedDiscountedLines, actualDiscountedEntries);
        }

        private static ShippingEntry Parse(IReadOnlyList<string> lineElements)
        {
            //var date = DateTime.TryParseExact(lineElements[0], "yyyy-MM-dd", CultureInfo.InvariantCulture, out DateTime d);
            var isDatePresent = DateTime.TryParseExact(
                lineElements[0],
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var date);


            if (lineElements.Count != 3 || !isDatePresent)
            {
                return ShippingEntry.Corrupt(lineElements);
            }

            var result = new ShippingEntry
            {
                //Date = Convert.ToDateTime(lineElements[0]),
                Date = date,
                PackageSize = lineElements[1],
                ShippingProvider = lineElements[2]
            };
            return result;
        }

        public class DiscountedShippingEntry
        {
            private readonly ShippingEntry _shippingEntry;
            private readonly string _shippingCost;
            private readonly string _discount;

            public DiscountedShippingEntry(ShippingEntry shippingEntry, string shippingCost, string discount)
            {
                _shippingEntry = shippingEntry;
                _shippingCost = shippingCost;
                _discount = discount;
            }

            public override string ToString()
            {
                if (_shippingEntry.IsCorrupt)
                {
                    return $"{_shippingEntry} Ignored";
                }
                else
                {
                    return $"{_shippingEntry} {_shippingCost} {_discount}";
                }
            }
        }
    }

    public class ShippingCostCalculator
    {
        public IEnumerable<AcceptanceTests.DiscountedShippingEntry> Discount(IEnumerable<AcceptanceTests.ShippingEntry> shippingEntries)
        {
            return shippingEntries
                .Select(x => new AcceptanceTests.DiscountedShippingEntry(x, "0.00", "-"));
        }

    }
}
