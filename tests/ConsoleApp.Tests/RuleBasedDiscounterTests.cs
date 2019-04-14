using Discounts;
using Discounts.Discounters;
using Moq;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using AutoFixture.AutoMoq;
using Discounts.ApplicableDiscounts;
using Discounts.Rules;
using Xunit;

namespace ConsoleApp.Tests
{
    public class RuleBasedDiscounterTests
    {
        private readonly RuleBasedDiscounter _sut;
        private readonly Mock<DiscountingRules> _discountRules;
        private readonly IFixture _fixture;

        public RuleBasedDiscounterTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());

            var shipmentUnderDiscount = _fixture.Freeze<Mock<ShipmentWithApplicableDiscount>>();
            shipmentUnderDiscount.Setup(t => t.Apply())
                .Returns(() => _fixture.Create<GoodShipmentCost>());

            _discountRules = _fixture.Freeze<Mock<DiscountingRules>>();
            _discountRules.Setup(x => x.AssignDiscount(It.IsAny<GoodShipmentCost>()))
                .Returns(shipmentUnderDiscount.Object);
           
            var underlyingDiscounter = new Mock<Discounter>();
            underlyingDiscounter.Setup(t => t.Discount(It.IsAny<IEnumerable<GoodShipmentCost>>()))
                .Returns<IEnumerable<GoodShipmentCost>>(x => x);

            _sut = new RuleBasedDiscounter(underlyingDiscounter.Object,  () => _discountRules.Object);
        }

        [Fact]
        public void Discount_AssignsDiscountToShipments()
        {
            //Arrange
            var shipmentsCosts = _fixture.Create<List<GoodShipmentCost>>();

            //Act
            _sut.Discount(shipmentsCosts).ToList();

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
            var input = new List<GoodShipmentCost>
            {
                shipment
            };
            var shipmentWithApplicableDiscount = _fixture.Create<Mock<ShipmentWithApplicableDiscount>>();
            _discountRules.Setup(t => t.AssignDiscount(shipment))
                .Returns(shipmentWithApplicableDiscount.Object);

            //Act
            _sut.Discount(input).ToList();

            //Assert
            shipmentWithApplicableDiscount.Verify(t => t.Apply(), Times.Once);
        }

        [Fact]
        public void Discount_UpdatesRulesUsingDiscountedShipment()
        {
            //Arrange
            var shipment = new ShipmentCostBuilder().Build();
            var input = new List<GoodShipmentCost>
            {
                shipment
            };

            var shipmentWithApplicableDiscount = _fixture.Create<Mock<ShipmentWithApplicableDiscount>>();
            _discountRules.Setup(t => t.AssignDiscount(It.IsAny<GoodShipmentCost>()))
                .Returns(shipmentWithApplicableDiscount.Object);
            var expectedShipmentCost = _fixture.Create<GoodShipmentCost>();
            shipmentWithApplicableDiscount.Setup(t => t.Apply()).Returns(expectedShipmentCost);

            //Act
            _sut.Discount(input).ToList();

            //Assert
            _discountRules.Verify(t => t.Update(expectedShipmentCost), Times.Once);

        }


    }
}