using System.Collections.Generic;
using Discounts;
using Discounts.Discounters;
using Moq;
using Xunit;

namespace ConsoleApp.Tests
{
    public class ThirdLpPackageDiscounterBehavioralTests
    {
        private readonly ThirdLpPackageDiscounter _sut;
        private readonly Mock<IDiscounter> _underlyingDiscounter;
        private readonly Mock<IFilter> _filter;

        public ThirdLpPackageDiscounterBehavioralTests()
        {

            _underlyingDiscounter = new Mock<IDiscounter>();
            _underlyingDiscounter.Setup(t => t.Discount(It.IsAny<IEnumerable<ShipmentCost>>()))
                .Returns<IEnumerable<ShipmentCost>>(x => x);
            _sut = new ThirdLpPackageDiscounter(_underlyingDiscounter.Object);
        }

        [Fact]
        public void Discount_AppliesDiscountAssignedByFilter()
        {
            //Arrange
            var firstShipment = new ShipmentCostBuilder().Build();
            var firstShipmentUnderDiscount = new Mock<ApplicableDiscount>();
            var input = new List<ShipmentCost>()
            {
                firstShipment
            };
            _filter.Setup(t => t.Apply(firstShipment))
                .Returns(firstShipmentUnderDiscount.Object);


            //Act
            var actual = _sut.Discount(input);

            //Assert
            firstShipmentUnderDiscount.Verify(t=>t.Apply(), Times.Once);

        }

    }
    public class ShipmentUnderNoDiscount : ApplicableDiscount
    {
        public ShipmentCost Apply()
        {
            throw new System.NotImplementedException();
        }
    }

    public class ShipmentUnderFullDiscount : ApplicableDiscount
    {
        public ShipmentCost Apply()
        {
            throw new System.NotImplementedException();
        }
    }

    internal interface IFilter
    {
        ApplicableDiscount Apply(ShipmentCost shipmentCost);
    }

    internal interface ApplicableDiscount
    {
        ShipmentCost Apply();
    }
}