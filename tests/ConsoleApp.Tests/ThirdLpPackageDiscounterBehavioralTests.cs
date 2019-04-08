using Discounts;
using Discounts.Discounters;
using Moq;
using System.Collections.Generic;
using AutoFixture;
using Discounts.Filters;
using Xunit;

namespace ConsoleApp.Tests
{
    public class ThirdLpPackageDiscounterBehavioralTests
    {
        private readonly ThirdLpPackageDiscounter _sut;
        private readonly Mock<DiscountingRules> _discountRules;
        private readonly Fixture _fixture;

        public ThirdLpPackageDiscounterBehavioralTests()
        {
            _fixture = new Fixture();

            _discountRules = new Mock<DiscountingRules>();
            var underlyingDiscounter = new Mock<Discounter>();

            underlyingDiscounter.Setup(t => t.Discount(It.IsAny<IEnumerable<ShipmentCost>>()))
                .Returns<IEnumerable<ShipmentCost>>(x => x);

            _sut = new ThirdLpPackageDiscounter(underlyingDiscounter.Object, _discountRules.Object);
        }

        [Fact]
        public void Discount_AssignsDiscountToShipments()
        {
            //Arrange
            var shipmentsCosts = _fixture.Create<List<ShipmentCost>>();

            //Act
            _sut.Discount(shipmentsCosts);

            //Assert
            foreach (var shipmentsCost in shipmentsCosts)
            {
                _discountRules.Verify(t=>t.AssignDiscount(shipmentsCost), Times.Once);
            }

        }


        [Fact]
        public void Discount_AppliesDiscountAssignedByRules()
        {
            //Arrange
            var firstShipment = new ShipmentCostBuilder().Build();
            var firstShipmentUnderDiscount = new Mock<ShipmentWithApplicableDiscount>();
            var input = new List<ShipmentCost>
            {
                firstShipment
            };
            _discountRules.Setup(t => t.AssignDiscount(firstShipment))
                .Returns(firstShipmentUnderDiscount.Object);


            //Act
            _sut.Discount(input);

            //Assert
            firstShipmentUnderDiscount.Verify(t => t.Apply(), Times.Once);
        }

    }
}