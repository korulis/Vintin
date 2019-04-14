using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Discounts;
using Discounts.Discounters;
using Xunit;

namespace ConsoleApp.Tests
{
    public class ShippingCostCalculatorTests
    {
        private readonly ShipmentCostCalculator _sut;

        public ShippingCostCalculatorTests()
        {
            _sut = new ShipmentCostCalculator(new ZeroDiscounter(), Defaults.CostReference);
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
            var shippingEntries = new List<Shipment>
            {
                new Shipment(DateTime.Today, packageSize, shippingProvider, Defaults.Separator, Defaults.DateFormats)
            };

            //Act
            var actual = _sut.CalculatePrice(shippingEntries).ToList()[0] as GoodShipmentCost;

            //Assert
            Assert.Equal(expectedCost, actual.Price);
        }

        [Fact]
        public void CalculatePrice_ReturnsMarkedOutputIfInputIsCorrupt()
        {
            //Arrange
            var shipments = new List<IShipment>
            {
                new IgnoredShipment("entry text")
            };

            //Act
            var actual = _sut.CalculatePrice(shipments).ToList();

            //Assert

            Assert.True(actual[0] is IgnoredShipmentCost);
        }
    }
}