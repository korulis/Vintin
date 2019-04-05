using System;
using System.Collections.Generic;
using System.Linq;
using Discounts;
using Discounts.Discounters;
using Xunit;

namespace ConsoleApp.Tests
{
    public class SmallPackageLowestPriceDiscounterTests
    {
        private static readonly Dictionary<(string, string), decimal> CostReference = Constants.CostReference;

        private readonly SmallPackageLowestPriceDiscounter _sut;

        public SmallPackageLowestPriceDiscounterTests()
        {
            _sut = new SmallPackageLowestPriceDiscounter(CostReference);
        }

        public static TheoryData<string, string, decimal, decimal, decimal, decimal> PackageData =>
            new TheoryData<string, string, decimal, decimal, decimal, decimal>
            {
                {"LP-vs-MR","S",2.0m, 0.0m, 1.5m, 0.5m},
                {"prior-discount","S",1.0m, 1.0m, 1.0m, 1.0m},
                {"negative-discount","S",4.0m, -1.0m, 1.5m, 1.5m},
                {"not-small-1","M",6.0m, 0.0m, 6.0m, 0.0m},
                {"not-small-2","L",6.0m, 0.0m, 6.0m, 0.0m},
            };

        [Theory]
        [MemberData(nameof(PackageData))]
        public void DiscountsOnlySmallestItems(
            string desc,
            string size,
            decimal currentPrice,
            decimal currentDiscount,
            decimal expectedPrice,
            decimal expectedDiscount)
        {
            //Arrange
            var shippingEntriesWithCosts = new List<ShippingCostEntry>()
            {
                new ShippingCostEntry(
                    new ShippingEntry
                    {
                        Date = DateTime.Today,
                        PackageSize = size,
                        ShippingProvider = "MR"
                    }, currentPrice, currentDiscount)
            };

            //Act
            var actual = _sut.Discount(shippingEntriesWithCosts).ToList()[0];

            //Assert
            Assert.Equal(expectedPrice, actual.Price);
            Assert.Equal(expectedDiscount, actual.Discount);
        }

        [Fact]
        public void DoesNotDiscountCorruptEntries()
        {
            //Arrange
            var shippingEntry = ShippingEntry.Corrupt("raw entry");
            shippingEntry.Date = DateTime.Today;
            shippingEntry.PackageSize = "S";
            shippingEntry.ShippingProvider = "MR";
            var shippingEntriesWithCosts = new List<ShippingCostEntry>()
            {
                new ShippingCostEntry(
                    shippingEntry, 2.0m, 0.0m)
            };

            //Act
            var actual = _sut.Discount(shippingEntriesWithCosts).ToList()[0];

            //Assert
            Assert.Equal(2.0m, actual.Price);
            Assert.Equal(0.0m, actual.Discount);
        }
    }
}