using System;
using System.Collections.Generic;
using System.Linq;
using Discounts;
using Discounts.Discounters;
using Xunit;

namespace ConsoleApp.Tests
{
    public class ThirdLpPackageDiscounterTests
    {
        private readonly ThirdLpPackageDiscounter _sut;

        public ThirdLpPackageDiscounterTests()
        {
            _sut = new ThirdLpPackageDiscounter();
        }


        public static TheoryData<string, List<ShippingCostEntry>, List<ShippingCostEntry>> PackageData
        {
            get
            {
                var b = new ShippingCostEntryBuilder();
                b.WithSize("L").WithProvider("LP").WithPricing(6.9m,0.0m);
                return new TheoryData<string, List<ShippingCostEntry>, List<ShippingCostEntry>>
                {
                    {
                        "every-third-large",
                        new List<ShippingCostEntry>
                        {
                            b.OnDay(1).Build(),
                            b.OnDay(2).Build(),
                            b.OnDay(3).Build(),
                        },
                        new List<ShippingCostEntry>
                        {
                            b.OnDay(1).Build(),
                            b.OnDay(2).Build(),
                            b.OnDay(3).WithPricing(0.0m,6.9m).Build(),
                        }
                    },
                };
            }
        }

        [Theory]
        [MemberData(nameof(PackageData))]
        public void DiscountLargeLpThirdPackage(
            string desc,
            IEnumerable<ShippingCostEntry> inputEntries,
            List<ShippingCostEntry> expected)
        {
            //Arrange

            //Act
            var actual = _sut.Discount(inputEntries).ToList();

            //Assert
            Assert.Equal(expected, actual);
        }

    }
}