using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Discounts;
using Discounts.Discounters;
using Xunit;

namespace ConsoleApp.Tests
{
    public class ShippingPriceCalculatorTests
    {
        private readonly ShippingCostCalculator _sut;

        public ShippingPriceCalculatorTests()
        {
            _sut = new ShippingCostCalculator(new ZeroDiscounter());
        }

        [Theory]
        [InlineData("S", "LP", "1.50")]
        [InlineData("M", "LP", "4.90")]
        [InlineData("L", "LP", "6.90")]
        [InlineData("S", "MR", "2.00")]
        [InlineData("M", "MR", "3.00")]
        [InlineData("L", "MR", "4.00")]
        public void CalculatePrice_CalculatesCostsWithoutDiscount(
            string packageSize, 
            string shippingProvider, 
            string expectedCostString)
        {
            //Arrange
            var expectedCost = Convert.ToDecimal(expectedCostString, CultureInfo.InvariantCulture);
            var shippingEntries = new List<ShippingEntry>
            {
                new ShippingEntry
                {
                    PackageSize = packageSize,
                    ShippingProvider = shippingProvider
                }
            };

            //Act
            var actual = _sut.CalculatePrice(shippingEntries).ToList();

            //Assert
            Assert.Equal(expectedCost, actual[0].Price);
        }

        [Fact]
        public void CalculatePrice_ReturnsMarkedOutputIfInputIsCorrupt()
        {
            //Arrange
            var shippingEntries = new List<ShippingEntry>
            {
                ShippingEntry.Corrupt("entry text")
            };

            //Act
            var actual = _sut.CalculatePrice(shippingEntries).ToList();

            //Assert
            Assert.True(actual[0].ShippingEntry.IsCorrupt);
        }
    }
}