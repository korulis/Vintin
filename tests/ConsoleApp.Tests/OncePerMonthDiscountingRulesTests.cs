using Discounts.Filters;
using Xunit;

namespace ConsoleApp.Tests
{
    public class OncePerMonthDiscountingRulesTests
    {
        private readonly OncePerMonthDiscountingRules _sut;

        public OncePerMonthDiscountingRulesTests()
        {
            _sut = new OncePerMonthDiscountingRules();
        }

        [Fact]
        public void AssignDiscount_ReturnsNoDiscount_IfShipmentIsCorrupt()
        {
            //Arrange
            var shipment = new ShipmentCostBuilder().Build();

            //Act
            var actual = _sut.AssignDiscount(shipment);

            //Assert
            Assert.IsType<ShipmentWithNoDiscount>(actual);
        }
    }
}
