using Discounts;
using Discounts.Discounters;
using Moq;
using System.Collections.Generic;
using AutoFixture;
using AutoFixture.AutoMoq;
using Discounts.Filters;
using Xunit;

namespace ConsoleApp.Tests
{
    public class ThirdLpPackageDiscounterBehavioralTests
    {
        private readonly ThirdLpPackageDiscounter _sut;
        private readonly Mock<DiscountingRules> _discountRules;
        private readonly IFixture _fixture;
        private readonly Mock<ShipmentWithApplicableDiscount> _shipmentUnderDiscount;

        public ThirdLpPackageDiscounterBehavioralTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());

            _shipmentUnderDiscount = _fixture.Freeze<Mock<ShipmentWithApplicableDiscount>>();
            _shipmentUnderDiscount.Setup(t => t.Apply())
                .Returns(() => _fixture.Create<ShipmentCost>());

            _discountRules = _fixture.Freeze<Mock<DiscountingRules>>();
            _discountRules.Setup(x => x.AssignDiscount(It.IsAny<ShipmentCost>()))
                .Returns(_shipmentUnderDiscount.Object);

           


            var underlyingDiscounter = new Mock<Discounter>();
            underlyingDiscounter.Setup(t => t.Discount(It.IsAny<IEnumerable<ShipmentCost>>()))
                .Returns<IEnumerable<ShipmentCost>>(x => x);

            _sut = new ThirdLpPackageDiscounter(underlyingDiscounter.Object,  () => _discountRules.Object);
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
            var input = new List<ShipmentCost>
            {
                firstShipment
            };
            _discountRules.Setup(t => t.AssignDiscount(firstShipment))
                .Returns(_shipmentUnderDiscount.Object);
            var shipmentCost = _fixture.Create<ShipmentCost>();
            _shipmentUnderDiscount.Setup(t => t.Apply())
                .Returns(shipmentCost);


            //Act
            _sut.Discount(input);

            //Assert
            _shipmentUnderDiscount.Verify(t => t.Apply(), Times.Once);
        }

    }
}