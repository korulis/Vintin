using System;
using System.Collections.Generic;
using System.Linq;
using Discounts;
using Xunit;

namespace ConsoleApp.Tests
{
    public class ShippingEntryMapperTests
    {
        private const string Separator = " ";
        private const string DateFormat = "yyyy-MM-dd";
        private readonly ShippingEntryMapper _sut;

        public ShippingEntryMapperTests()
        {
            _sut = new ShippingEntryMapper(Separator, DateFormat);
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
            var actual = _sut.ParseInput(input).ToList()[0];

            //Assert
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
            var actual = _sut.ParseInput(input).ToList();

            //Assert
            Assert.Equal(2, actual.Count);
            Assert.Equal(new DateTime(1999, 3, 31), actual[0].Date);
            Assert.Equal(new DateTime(1999, 3, 30), actual[1].Date);
        }

        [Theory]
        [InlineData(true, "1999-03-30 S LP")]
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
            Assert.Equal(expectedValidData, !actual.IsCorrupt);
        }


        [Fact]
        public void FormatDiscounted_TransformsTheObjectIntoString()
        {
            //Arrange
            var date = new DateTime(1999, 1, 1);
            const string packageSize = "SeriousPackageSize";
            const string shippingProvider = "SeriousShippingProvider";
            const string shippingCost = "BigAmount";
            const string discount = "BiggerAmount";
            var input = new List<DiscountedShippingEntry>
            {
                new DiscountedShippingEntry(new ShippingEntry()
                {
                    Date = date,
                    PackageSize = packageSize,
                    ShippingProvider = shippingProvider
                }, shippingCost, discount)
            };

            //Act 
            var actual = _sut.FormatOutput(input).ToList()[0];

            //Assert
            Assert.Equal(
                string.Join(
                    Separator, 
                    date.ToString(DateFormat), 
                    packageSize, 
                    shippingProvider, 
                    shippingCost, 
                    discount), 
                actual);
        }

        [Fact]
        public void FormatDiscounted_ReturnsListOfStrings_WithSameOrderingAsInput()
        {
            //Arrange
            var input = new List<DiscountedShippingEntry>
            {
                new DiscountedShippingEntry(new ShippingEntry()
                {
                    Date = new DateTime(1999,1,1),
                    PackageSize = "S",
                    ShippingProvider = "ML"
                }, "-","-"),
                new DiscountedShippingEntry(new ShippingEntry()
                {
                    Date = new DateTime(1998,1,1),
                    PackageSize = "S",
                    ShippingProvider = "ML"
                }, "-","-"),
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
            var input = new List<DiscountedShippingEntry>
            {
                new DiscountedShippingEntry(
                    ShippingEntry.Corrupt("Something corrupt"),
                    "-",
                    "-"
                )};

            //Act 
            var actual = _sut.FormatOutput(input).ToList()[0];

            //Assert
            Assert.Contains("Ignored", actual);
        }

    }
}
