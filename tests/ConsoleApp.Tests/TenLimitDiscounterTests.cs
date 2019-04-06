using System.Collections.Generic;
using System.Linq;
using Discounts;
using Discounts.Discounters;
using Xunit;

namespace ConsoleApp.Tests
{
    public class TenLimitDiscounterTests
    {
        private readonly TenLimitDiscounter _sut;

        public TenLimitDiscounterTests()
        {
            _sut = new TenLimitDiscounter();
        }


        public static TheoryData<string, List<ShippingCostEntry>, List<ShippingCostEntry>> PackageData
        {
            get
            {
                var b = new ShippingCostEntryBuilder();
                return new TheoryData<string, List<ShippingCostEntry>, List<ShippingCostEntry>>
                {
                    {
                        "single-large-discount-fix",
                        new List<ShippingCostEntry>
                        {
                            b.OnDay(1).WithPricing(9.0m, 11.0m).Build(),
                        },
                        new List<ShippingCostEntry>
                        {
                            b.OnDay(1).WithPricing(10.0m,10.0m).Build(),
                        }
                    },
                    {
                        "can-do-nothing",
                        new List<ShippingCostEntry>
                        {
                            b.OnDay(1).WithPricing(11.0m, 9.0m).Build(),
                        },
                        new List<ShippingCostEntry>
                        {
                            b.OnDay(1).WithPricing(11.0m,9.0m).Build(),
                        }
                    },
                    {
                        "amend-the-second-discount",
                        new List<ShippingCostEntry>
                        {
                            b.OnDay(1).WithPricing(1.0m, 5.0m).Build(),
                            b.OnDay(1).WithPricing(1.0m, 7.0m).Build(),
                        },
                        new List<ShippingCostEntry>
                        {
                            b.OnDay(1).WithPricing(1.0m, 5.0m).Build(),
                            b.OnDay(1).WithPricing(3.0m, 5.0m).Build(),
                        }
                    },
                    {
                        "with-previous-month",
                        new List<ShippingCostEntry>
                        {
                            b.OnMonth(5).OnDay(1).WithPricing(1.0m, 5.0m).Build(),
                            b.OnMonth(6).OnDay(1).WithPricing(1.0m, 3.0m).Build(),
                            b.OnMonth(6).OnDay(1).WithPricing(1.0m, 8.0m).Build(),
                        },
                        new List<ShippingCostEntry>
                        {
                            b.OnMonth(5).OnDay(1).WithPricing(1.0m, 5.0m).Build(),
                            b.OnMonth(6).OnDay(1).WithPricing(1.0m, 3.0m).Build(),
                            b.OnMonth(6).OnDay(1).WithPricing(2.0m, 7.0m).Build(),
                        }
                    },
                    {
                        "with-upcoming-month",
                        new List<ShippingCostEntry>
                        {
                            b.OnMonth(6).OnDay(1).WithPricing(1.0m, 3.0m).Build(),
                            b.OnMonth(6).OnDay(1).WithPricing(1.0m, 5.0m).Build(),
                            b.OnMonth(7).OnDay(1).WithPricing(1.0m, 8.0m).Build(),
                        },
                        new List<ShippingCostEntry>
                        {
                            b.OnMonth(6).OnDay(1).WithPricing(1.0m, 3.0m).Build(),
                            b.OnMonth(6).OnDay(1).WithPricing(1.0m, 5.0m).Build(),
                            b.OnMonth(7).OnDay(1).WithPricing(1.0m, 8.0m).Build(),
                        }
                    },
                    {
                        "several-months",
                        new List<ShippingCostEntry>
                        {
                            b.OnMonth(5).OnDay(1).WithPricing(1.0m, 8.0m).Build(),
                            b.OnMonth(5).OnDay(1).WithPricing(1.0m, 5.0m).Build(),
                            b.OnMonth(6).OnDay(1).WithPricing(1.0m, 12.0m).Build(),
                            b.OnMonth(7).OnDay(1).WithPricing(1.0m, 5.0m).Build(),
                            b.OnMonth(7).OnDay(1).WithPricing(1.0m, 8.0m).Build(),
                        },
                        new List<ShippingCostEntry>
                        {
                            b.OnMonth(5).OnDay(1).WithPricing(1.0m, 8.0m).Build(),
                            b.OnMonth(5).OnDay(1).WithPricing(4.0m, 2.0m).Build(),
                            b.OnMonth(6).OnDay(1).WithPricing(3.0m, 10.0m).Build(),
                            b.OnMonth(7).OnDay(1).WithPricing(1.0m, 5.0m).Build(),
                            b.OnMonth(7).OnDay(1).WithPricing(4.0m, 5.0m).Build(),
                        }
                    },
                };
            }
        }

        [Theory]
        [MemberData(nameof(PackageData))]
        public void DiscountUpToTenPerMonth(
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