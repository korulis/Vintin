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
        private static readonly Dictionary<(string, string), decimal> CostReference = Defaults.CostReference;

        private readonly SmallPackageLowestPriceDiscounter _sut;

        public SmallPackageLowestPriceDiscounterTests()
        {
            _sut = new SmallPackageLowestPriceDiscounter(new ZeroDiscounter(), CostReference);
        }

        public static TheoryData<string, ShipmentCost, decimal, decimal> PackageData
        {
            get
            {
                var b = new ShipmentCostBuilder();
                b.WithProvider("MR");

                return new TheoryData<string, ShipmentCost, decimal, decimal>
                {
                    {"discount-MR", b.WithSize("S").WithPricing(2.0m,0.0m).Build(), 1.5m, 0.5m},
                    {"prior-discount", b.WithSize("S").WithPricing(1.0m, 1.0m).Build(), 1.0m, 1.0m},
                    {"negative-discount", b.WithSize("S").WithPricing(4.0m, -1.0m).Build(), 1.5m, 1.5m},
                    {"not-small-1", b.WithSize("L").WithPricing(6.0m, 0.0m).Build(), 6.0m, 0.0m},
                    {"not-small-2", b.WithSize("M").WithPricing(6.0m, 0.0m).Build(), 6.0m, 0.0m},
                };
            }
        }

        [Theory]
        [MemberData(nameof(PackageData))]
        public void DiscountsOnlySmallestItems(
            string desc,
            ShipmentCost shipmentCost,
            decimal expectedPrice,
            decimal expectedDiscount)
        {
            //Arrange
            var shipmentCosts = new List<ShipmentCost>
            {
                shipmentCost
            };

            //Act
            var actual = _sut.Discount(shipmentCosts).ToList()[0];

            //Assert
            Assert.Equal(expectedPrice, actual.Price);
            Assert.Equal(expectedDiscount, actual.Discount);
        }

        [Fact]
        public void DoesNotDiscountCorruptEntries()
        {
            //Arrange
            var shipment = Shipment.Corrupt("raw entry");
            shipment.Date = DateTime.Today;
            shipment.PackageSize = "S";
            shipment.ShippingProvider = "MR";
            var shipmentCosts = new List<ShipmentCost>()
            {
                new ShipmentCost(
                    shipment, 2.0m, 0.0m)
            };

            //Act
            var actual = _sut.Discount(shipmentCosts).ToList()[0];

            //Assert
            Assert.Equal(2.0m, actual.Price);
            Assert.Equal(0.0m, actual.Discount);
        }
    }
}