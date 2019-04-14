using Discounts;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Xunit;

namespace ConsoleApp.Tests
{
    public class ShipmentMapperTests
    {
        private const string Separator = Defaults.Separator;
        private static readonly string[] DateFormats = Defaults.DateFormats;
        private static readonly string[] AcceptableSizes = Defaults.PackageSizes;
        private static readonly string[] AcceptableProviders = Defaults.ShippingProviders;
        private readonly ShipmentMapper _sut;

        public ShipmentMapperTests()
        {
            _sut = new ShipmentMapper(Separator, DateFormats, AcceptableProviders, AcceptableSizes);
        }


        [Fact]
        public void ParseInput_ParsesStringLines()
        {
            //Arrange
            var input = new List<string>
            {
                "1999-03-31 S LP",
            };

            //Act 
            var a = _sut.ParseInput(input).ToList()[0];

            //Assert
            Assert.True(a is Shipment);
            var actual = (Shipment)a;
            Assert.Equal(new DateTime(1999, 3, 31), actual.Date);
            Assert.Equal("S", actual.PackageSize);
            Assert.Equal("LP", actual.ShippingProvider);
        }

        [Fact]
        public void ParseInput_ParsesStringLinesToProduceObjectArray_WithSameOrderingAsInput()
        {
            //Arrange
            var input = new List<string>
            {
                "1999-03-31 S LP",
                "1999-03-30 S LP"
            };

            //Act 
            var result = _sut.ParseInput(input).ToList();

            //Assert
            var actual = result.Select(x => x is Shipment s ? s : null).ToList();

            Assert.Equal(2, actual.Count);
            Assert.Equal(new DateTime(1999, 3, 31), actual[0].Date);
            Assert.Equal(new DateTime(1999, 3, 30), actual[1].Date);
        }

        [Theory]
        [InlineData(true, "1999-03-30 S LP")] //base case
        [InlineData(true, "19990330 S LP")]
        [InlineData(true, "1999-03-30 M LP")]
        [InlineData(true, "1999-03-30 L LP")]
        [InlineData(true, "1999-03-30 S MR")]
        [InlineData(false, "S 1999-03-30 LP")]
        [InlineData(false, "1999-03-30 XL LP")]
        [InlineData(false, "1999-03-30 S DHL")]
        [InlineData(false, "1999-13-30 S LP")]
        [InlineData(false, "1999-03-30 S")]
        [InlineData(false, "1999-03-30 CUSP")]
        [InlineData(false, "Something completely else")]
        public void ParseInput_ValidInputFormats(bool expectedValidData, string inputLine)
        {
            //Arrange
            var input = new List<string> { inputLine };

            //Act 
            var actual = _sut.ParseInput(input).ToList()[0];

            //Assert
            Assert.Equal(expectedValidData, actual is Shipment);
        }

        [Theory]
        [InlineData("1999-03-30 S LP")]
        [InlineData("19990330 S LP")]
        public void ParseInput_CanParseDifferentDateFormats(string inputLine)
        {
            //Arrange
            var input = new List<string> { inputLine };

            //Act 
            var actual = _sut.ParseInput(input).ToList()[0] as Shipment;

            //Assert
            Assert.Equal(new DateTime(1999, 03, 30), actual?.Date);
        }


        [Theory]
        [InlineData("0.00", "-")]
        [InlineData("0.01", "0.01")]
        public void FormatDiscounted_TransformsTheObjectIntoString(string discountString, string expectedDiscount)
        {
            //Arrange
            var date = new DateTime(1999, 1, 1);
            const string packageSize = "SeriousPackageSize";
            const string shippingProvider = "SeriousShippingProvider";
            const decimal shippingCost = 0.00m;
            var discount = Convert.ToDecimal(discountString, CultureInfo.InvariantCulture);
            var input = new List<GoodShipmentCost>
            {
                new GoodShipmentCost(
                    new Shipment(
                        date,
                        packageSize,
                        shippingProvider,
                        Separator,
                        DateFormats),
                    shippingCost,
                    discount)
            };

            //Act 
            var actual = _sut.FormatOutput(input).ToList()[0];

            //Assert
            Assert.Equal(
                string.Join(
                    Separator,
                    date.ToString(DateFormats[0]),
                    packageSize,
                    shippingProvider,
                    "0.00",
                    expectedDiscount),
                actual);
        }

        [Fact]
        public void FormatDiscounted_ReturnsListOfStrings_WithSameOrderingAsInput()
        {
            //Arrange
            var input = new List<GoodShipmentCost>
            {
                new GoodShipmentCost(
                    new Shipment(
                        new DateTime(1999,1,1),
                        "S",
                        "ML",
                        Separator,
                        DateFormats),
                    0.00m,
                    0.00m),
                new GoodShipmentCost(
                    new Shipment(
                        new DateTime(1998,1,1),
                        "S",
                        "ML",
                        Separator,
                        DateFormats),
                    0.00m,
                    0.00m),
            };

            //Act 
            var actual = _sut.FormatOutput(input).ToList();

            //Assert
            Assert.Contains("1999", actual[0]);
            Assert.Contains("1998", actual[1]);
        }

        [Fact]
        public void FormatDiscounted_MarksInvalidInputInTheOutput()
        {
            //Arrange
            var input = new List<IShipmentCost<IShipment>>
            {
                new IgnoredShipmentCost(
                    new IgnoredShipment("This is some corrupt entry"))
                };

            //Act 
            var actual = _sut.FormatOutput(input).ToList()[0];

            //Assert
            Assert.Contains("Ignored", actual);
        }
    }
}
