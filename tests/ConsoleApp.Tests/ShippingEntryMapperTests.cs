using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ConsoleApp.Tests
{
    public class ShippingEntryMapperTests
    {
        private const string Separator = " ";
        private readonly ShippingEntryMapper _sut;

        public ShippingEntryMapperTests()
        {
            const string acceptableDateFormat = "yyyy-MM-dd";
            _sut = new ShippingEntryMapper(Separator, acceptableDateFormat);
        }

        [Fact]
        public void MapperParsesStringLinesWithoutTransformingDataValues()
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
        public void MapperParsesStringLinesToProduceObjectArray_WithSameOrdering()
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
        public void TestValidDataFormats(bool expectedValidData, string inputLine)
        {
            //Arrange
            var input = new List<string> { inputLine };

            //Act 
            var actual = _sut.ParseInput(input).ToList()[0];

            //Assert
            Assert.Equal(expectedValidData, !actual.IsCorrupt);
        }
    }
}
