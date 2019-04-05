﻿using System;
using System.Collections.Generic;
using System.Linq;
using Discounts;
using Xunit;

namespace ConsoleApp.Tests
{
    public class SmallPackageLowestPriceDiscounterTests
    {
        private readonly SmallPackageLowestPriceDiscounter _sut;

        public SmallPackageLowestPriceDiscounterTests()
        {
            _sut = new SmallPackageLowestPriceDiscounter();
        }

        public static TheoryData<string, string, decimal, decimal, decimal, decimal> SmallPackageData =>
            new TheoryData<string, string, decimal, decimal, decimal, decimal>
            {
                {"LP-vs-MR","S",2.0m, 0.0m, 1.5m, 0.5m},
                {"prior-discount","S",1.0m, 1.0m, 1.0m, 1.0m},
                {"negative-discount","S",4.0m, -1.0m, 1.5m, 1.5m},
                {"not-small-1","M",6.0m, 0.0m, 6.0m, 0.0m},
                {"not-small-2","L",6.0m, 0.0m, 6.0m, 0.0m},
            };

        [Theory]
        [MemberData(nameof(SmallPackageData))]
        public void DiscountsOnlySmallestItems(
            string desc,
            string size,
            decimal currentPrice,
            decimal currentDiscount,
            decimal expectedPrice,
            decimal expectedDiscount)
        {
            //Arrange
            var shippingEntriesWithCosts = new List<ShippingCostEntry>()
            {
                new ShippingCostEntry(
                    new ShippingEntry
                    {
                        Date = DateTime.Today,
                        PackageSize = size,
                        ShippingProvider = "MR"
                    }, currentPrice, currentDiscount)
            };

            //Act
            var actual = _sut.Discount(shippingEntriesWithCosts).ToList()[0];

            //Assert
            Assert.Equal(expectedPrice, actual.Price);
            Assert.Equal(expectedDiscount, actual.Discount);
        }
    }
}