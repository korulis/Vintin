using Discounts;
using Discounts.Discounters;
using Moq;
using System.Collections.Generic;
using AutoFixture;
using AutoFixture.AutoMoq;
using Discounts.Rules;
using Xunit;

namespace ConsoleApp.Tests
{
    public class ThirdLpPackageDiscounterBehavioralTests
    {
        private readonly ThirdLpPackageDiscounter _sut;
        private readonly Mock<DiscountingRules> _discountRules;
        private readonly IFixture _fixture;

        public ThirdLpPackageDiscounterBehavioralTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());

            var shipmentUnderDiscount = _fixture.Freeze<Mock<ShipmentWithApplicableDiscount>>();
            shipmentUnderDiscount.Setup(t => t.Apply())
                .Returns(() => _fixture.Create<ShipmentCost>());

            _discountRules = _fixture.Freeze<Mock<DiscountingRules>>();
            _discountRules.Setup(x => x.AssignDiscount(It.IsAny<ShipmentCost>()))
                .Returns(shipmentUnderDiscount.Object);
           
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
            var shipment = new ShipmentCostBuilder().Build();
            var input = new List<ShipmentCost>
            {
                shipment
            };
            var shipmentWithApplicableDiscount = _fixture.Create<Mock<ShipmentWithApplicableDiscount>>();
            _discountRules.Setup(t => t.AssignDiscount(shipment))
                .Returns(shipmentWithApplicableDiscount.Object);

            //Act
            _sut.Discount(input);

            //Assert
            shipmentWithApplicableDiscount.Verify(t => t.Apply(), Times.Once);
        }

        [Fact]
        public void Discount_UpdatesRulesUsingDiscountedShipment()
        {
            //Arrange
            var shipment = new ShipmentCostBuilder().Build();
            var input = new List<ShipmentCost>
            {
                shipment
            };

            var shipmentWithApplicableDiscount = _fixture.Create<Mock<ShipmentWithApplicableDiscount>>();
            _discountRules.Setup(t => t.AssignDiscount(It.IsAny<ShipmentCost>()))
                .Returns(shipmentWithApplicableDiscount.Object);
            var expectedShipmentCost = _fixture.Create<ShipmentCost>();
            shipmentWithApplicableDiscount.Setup(t => t.Apply()).Returns(expectedShipmentCost);

            //Act
            _sut.Discount(input);

            //Assert
            _discountRules.Verify(t => t.Update(expectedShipmentCost), Times.Once);

        }


    }
}