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
                b.WithSize("L").WithProvider("LP");
                return new TheoryData<string, List<ShippingCostEntry>, List<ShippingCostEntry>>
                {
                    {
                        "every-third-large",
                        new List<ShippingCostEntry>
                        {
                            b.OnDay(1).WithPricing(6.9m, 0.0m).Build(),
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
                    {
                        "..and-only-third-large",
                        new List<ShippingCostEntry>
                        {
                            b.OnDay(1).WithPricing(6.9m,0.0m).Build(),
                            b.OnDay(2).Build(),
                            b.OnDay(3).Build(),
                            b.OnDay(4).Build(),
                            b.OnDay(5).Build(),
                            b.OnDay(6).Build(),
                        },
                        new List<ShippingCostEntry>
                        {
                            b.OnDay(1).Build(),
                            b.OnDay(2).Build(),
                            b.OnDay(3).WithPricing(0.0m,6.9m).Build(),
                            b.OnDay(4).WithPricing(6.9m,0.0m).Build(),
                            b.OnDay(5).Build(),
                            b.OnDay(6).Build(),
                        }
                    },
                    {
                        "..even-with-prior-months",
                        new List<ShippingCostEntry>
                        {
                            b.OnMonth(5).OnDay(1).WithPricing(6.9m,0.0m).Build(),
                            b.OnMonth(6).OnDay(1).Build(),
                            b.OnDay(2).Build(),
                            b.OnDay(3).Build(),
                        },
                        new List<ShippingCostEntry>
                        {
                            b.OnMonth(5).OnDay(1).Build(),
                            b.OnMonth(6).OnDay(1).Build(),
                            b.OnDay(2).Build(),
                            b.OnDay(3).WithPricing(0.0m,6.9m).Build(),
                        }
                    },
                    {
                        "..or-months-after",
                        new List<ShippingCostEntry>
                        {
                            b.OnDay(1).WithPricing(6.9m,0.0m).Build(),
                            b.OnDay(2).Build(),
                            b.OnDay(3).Build(),
                            b.OnMonth(7).OnDay(1).Build(),
                        },
                        new List<ShippingCostEntry>
                        {
                            b.OnMonth(6).OnDay(1).Build(),
                            b.OnDay(2).Build(),
                            b.OnDay(3).WithPricing(0.0m,6.9m).Build(),
                            b.OnMonth(7).OnDay(1).WithPricing(6.9m,0.0m).Build(),
                        }
                    },
                    {
                        "..or-multiple-months",
                        new List<ShippingCostEntry>
                        {
                            b.OnMonth(6).OnDay(1).WithPricing(6.9m,0.0m).Build(),
                            b.OnDay(2).Build(),
                            b.OnDay(3).Build(),
                            b.OnMonth(7).OnDay(1).Build(),
                            b.OnMonth(7).OnDay(2).Build(),
                            b.OnMonth(7).OnDay(3).Build(),
                        },
                        new List<ShippingCostEntry>
                        {
                            b.OnMonth(6).OnDay(1).WithPricing(6.9m,0.0m).Build(),
                            b.OnDay(2).Build(),
                            b.OnDay(3).WithPricing(0.0m,6.9m).Build(),
                            b.OnMonth(7).OnDay(1).WithPricing(6.9m,0.0m).Build(),
                            b.OnMonth(7).OnDay(2).Build(),
                            b.OnMonth(7).OnDay(3).WithPricing(0.0m,6.9m).Build(),
                        }
                    },
                    {
                        "..or-same-day",
                        new List<ShippingCostEntry>
                        {
                            b.OnMonth(6).OnDay(1).WithPricing(6.9m,0.0m).Build(),
                            b.OnDay(1).Build(),
                            b.OnDay(1).Build(),
                            b.OnDay(1).Build(),
                        },
                        new List<ShippingCostEntry>
                        {
                            b.OnDay(1).WithPricing(6.9m,0.0m).Build(),
                            b.OnDay(1).Build(),
                            b.OnDay(1).WithPricing(0.0m,6.9m).Build(),
                            b.OnDay(1).WithPricing(6.9m,0.0m).Build(),
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
            for (int i = 0; i < actual.Count; i++)
            {
                Assert.Equal(expected[i], actual[i]);
            }

            Assert.Equal(expected, actual);
        }

    }
}