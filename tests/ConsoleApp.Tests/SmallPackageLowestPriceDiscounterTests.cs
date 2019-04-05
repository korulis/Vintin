using System;
using System.Collections.Generic;
using System.Linq;
using Discounts;
using Xunit;

namespace ConsoleApp.Tests
{
    public class SmallPackageLowestPriceDiscounterTests
    {
        private SmallPackageLowestPriceDiscounter _sut;

        public SmallPackageLowestPriceDiscounterTests()
        {
            _sut = new SmallPackageLowestPriceDiscounter();
        }

        [Fact]
        public void DiscountsOnlySmallestItems()
        {
            //Arrange
            var shippingEntriesWithCosts = new List<ProcessedShippingEntry>()
            {
                new ProcessedShippingEntry(
                    new ShippingEntry
                    {
                        Date = DateTime.Today,
                        PackageSize = "S",
                        ShippingProvider = "MR"
                    }, 2m, 0)
            };

            //Act
            var actual = _sut.Discount(shippingEntriesWithCosts).ToList()[0];

            //Assert
            Assert.Equal(1.5m, actual.ShippingCost);
            Assert.Equal(0.5m, actual.Discount);

        }
    }
}