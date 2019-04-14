using System.Collections.Generic;
using System.Linq;
using Discounts;
using Discounts.Discounters;
using Xunit;

namespace ConsoleApp.Tests.RuleBasedDiscounterCases
{
    public class SmallPackageLowestPriceDiscounterTests
    {
        private static readonly Dictionary<(string, string), decimal> CostReference = Defaults.CostReference;

        private readonly SmallPackageLowestPriceDiscounter _sut;

        public SmallPackageLowestPriceDiscounterTests()
        {
            _sut = new SmallPackageLowestPriceDiscounter(new ZeroDiscounter(), CostReference);
        }

        public static TheoryData<string, GoodShipmentCost, decimal, decimal> PackageData
        {
            get
            {
                var b = new ShipmentCostBuilder();
                b.WithProvider("MR");

                return new TheoryData<string, GoodShipmentCost, decimal, decimal>
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
            GoodShipmentCost goodShipmentCost,
            decimal expectedPrice,
            decimal expectedDiscount)
        {
            //Arrange
            var shipmentCosts = new List<GoodShipmentCost>
            {
                goodShipmentCost
            };

            //Act
            var result = _sut.Discount(shipmentCosts).ToList()[0];

            //Assert
            Assert.True(result is GoodShipmentCost);
            var actual = (GoodShipmentCost) result; 
            Assert.Equal(expectedPrice, actual.Price);
            Assert.Equal(expectedDiscount, actual.Discount);
        }

        [Fact]
        public void DoesNotDiscountCorruptEntries()
        {
            //Arrange
            var shipment = new IgnoredShipment("raw entry");
            var shipmentCosts = new List<IgnoredShipmentCost>()
            {
                new IgnoredShipmentCost(shipment)
            };

            //Act
            var actual = _sut.Discount(shipmentCosts).ToList()[0];

            //Assert
            Assert.True(actual is IgnoredShipmentCost);
        }
}
}