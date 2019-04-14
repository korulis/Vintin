using System.Collections.Generic;
using System.Linq;
using Discounts;
using Discounts.Discounters;
using Discounts.Rules;
using Xunit;

namespace ConsoleApp.Tests.RuleBasedDiscounterCases
{
    public class ThirdLpPackageDiscounterTests
    {
        private readonly RuleBasedDiscounter _sut;

        public ThirdLpPackageDiscounterTests()
        {
            _sut = new RuleBasedDiscounter(
                new ZeroDiscounter(),
                () => new OncePerMonthDiscountingRules(
                    Defaults.ThirdLpPackageEveryMonth.SpecialProvider,
                    Defaults.ThirdLpPackageEveryMonth.SpecialSize,
                    Defaults.ThirdLpPackageEveryMonth.LuckOrderNumber));
        }

        public static TheoryData<string, List<GoodShipmentCost>, List<GoodShipmentCost>> PackageData
        {
            get
            {
                var b = new ShipmentCostBuilder();
                b.WithSize("L").WithProvider("LP");
                return new TheoryData<string, List<GoodShipmentCost>, List<GoodShipmentCost>>
                {
                    {
                        "every-third-large",
                        new List<GoodShipmentCost>
                        {
                            b.OnDay(1).WithPricing(6.9m, 0.0m).Build(),
                            b.OnDay(2).Build(),
                            b.OnDay(3).Build(),
                        },
                        new List<GoodShipmentCost>
                        {
                            b.OnDay(1).Build(),
                            b.OnDay(2).Build(),
                            b.OnDay(3).WithPricing(0.0m,6.9m).Build(),
                        }
                    },
                    {
                        "..and-only-third-large",
                        new List<GoodShipmentCost>
                        {
                            b.OnDay(1).WithPricing(6.9m,0.0m).Build(),
                            b.OnDay(2).Build(),
                            b.OnDay(3).Build(),
                            b.OnDay(4).Build(),
                            b.OnDay(5).Build(),
                            b.OnDay(6).Build(),
                        },
                        new List<GoodShipmentCost>
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
                        new List<GoodShipmentCost>
                        {
                            b.OnMonth(5).OnDay(1).WithPricing(6.9m,0.0m).Build(),
                            b.OnMonth(6).OnDay(1).Build(),
                            b.OnDay(2).Build(),
                            b.OnDay(3).Build(),
                        },
                        new List<GoodShipmentCost>
                        {
                            b.OnMonth(5).OnDay(1).Build(),
                            b.OnMonth(6).OnDay(1).Build(),
                            b.OnDay(2).Build(),
                            b.OnDay(3).WithPricing(0.0m,6.9m).Build(),
                        }
                    },
                    {
                        "..or-months-after",
                        new List<GoodShipmentCost>
                        {
                            b.OnDay(1).WithPricing(6.9m,0.0m).Build(),
                            b.OnDay(2).Build(),
                            b.OnDay(3).Build(),
                            b.OnMonth(7).OnDay(1).Build(),
                        },
                        new List<GoodShipmentCost>
                        {
                            b.OnMonth(6).OnDay(1).Build(),
                            b.OnDay(2).Build(),
                            b.OnDay(3).WithPricing(0.0m,6.9m).Build(),
                            b.OnMonth(7).OnDay(1).WithPricing(6.9m,0.0m).Build(),
                        }
                    },
                    {
                        "..or-multiple-months",
                        new List<GoodShipmentCost>
                        {
                            b.OnMonth(6).OnDay(1).WithPricing(6.9m,0.0m).Build(),
                            b.OnDay(2).Build(),
                            b.OnDay(3).Build(),
                            b.OnMonth(7).OnDay(1).Build(),
                            b.OnMonth(7).OnDay(2).Build(),
                            b.OnMonth(7).OnDay(3).Build(),
                        },
                        new List<GoodShipmentCost>
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
                        new List<GoodShipmentCost>
                        {
                            b.OnMonth(6).OnDay(1).WithPricing(6.9m,0.0m).Build(),
                            b.OnDay(1).Build(),
                            b.OnDay(1).Build(),
                            b.OnDay(1).Build(),
                        },
                        new List<GoodShipmentCost>
                        {
                            b.OnDay(1).WithPricing(6.9m,0.0m).Build(),
                            b.OnDay(1).Build(),
                            b.OnDay(1).WithPricing(0.0m,6.9m).Build(),
                            b.OnDay(1).WithPricing(6.9m,0.0m).Build(),
                        }
                    },
                    {
                        "ignores-other-sizes-before",
                        new List<GoodShipmentCost>
                        {
                            b.OnMonth(6).OnDay(1).WithSize("M").WithPricing(4.9m,0.0m).Build(),
                            b.OnMonth(6).OnDay(1).WithSize("L").WithPricing(6.9m,0.0m).Build(),
                            b.OnMonth(6).OnDay(1).WithSize("L").WithPricing(6.9m,0.0m).Build(),
                            b.OnMonth(6).OnDay(1).WithSize("L").WithPricing(6.9m,0.0m).Build(),
                        },
                        new List<GoodShipmentCost>
                        {
                            b.OnMonth(6).OnDay(1).WithSize("M").WithPricing(4.9m,0.0m).Build(),
                            b.OnMonth(6).OnDay(1).WithSize("L").WithPricing(6.9m,0.0m).Build(),
                            b.OnMonth(6).OnDay(1).WithSize("L").WithPricing(6.9m,0.0m).Build(),
                            b.OnMonth(6).OnDay(1).WithSize("L").WithPricing(0.0m,6.9m).Build(),
                        }
                    },
                    {
                        "ignores-other-sizes-in-between",
                        new List<GoodShipmentCost>
                        {
                            b.OnMonth(6).OnDay(1).WithSize("L").WithPricing(6.9m,0.0m).Build(),
                            b.OnMonth(6).OnDay(1).WithSize("L").WithPricing(6.9m,0.0m).Build(),
                            b.OnMonth(6).OnDay(1).WithSize("M").WithPricing(4.9m,0.0m).Build(),
                            b.OnMonth(6).OnDay(1).WithSize("L").WithPricing(6.9m,0.0m).Build(),
                        },
                        new List<GoodShipmentCost>
                        {
                            b.OnMonth(6).OnDay(1).WithSize("L").WithPricing(6.9m,0.0m).Build(),
                            b.OnMonth(6).OnDay(1).WithSize("L").WithPricing(6.9m,0.0m).Build(),
                            b.OnMonth(6).OnDay(1).WithSize("M").WithPricing(4.9m,0.0m).Build(),
                            b.OnMonth(6).OnDay(1).WithSize("L").WithPricing(0.0m,6.9m).Build(),
                        }
                    },
                    {
                        "ignores-other-sizes-after",
                        new List<GoodShipmentCost>
                        {
                            b.OnMonth(6).OnDay(1).WithSize("L").WithPricing(6.9m,0.0m).Build(),
                            b.OnMonth(6).OnDay(1).WithSize("L").WithPricing(6.9m,0.0m).Build(),
                            b.OnMonth(6).OnDay(1).WithSize("L").WithPricing(6.9m,0.0m).Build(),
                            b.OnMonth(6).OnDay(1).WithSize("M").WithPricing(4.9m,0.0m).Build(),
                        },
                        new List<GoodShipmentCost>
                        {
                            b.OnMonth(6).OnDay(1).WithSize("L").WithPricing(6.9m,0.0m).Build(),
                            b.OnMonth(6).OnDay(1).WithSize("L").WithPricing(6.9m,0.0m).Build(),
                            b.OnMonth(6).OnDay(1).WithSize("L").WithPricing(0.0m,6.9m).Build(),
                            b.OnMonth(6).OnDay(1).WithSize("M").WithPricing(4.9m,0.0m).Build(),
                        }
                    },
                };
            }
        }

        [Theory]
        [MemberData(nameof(PackageData))]
        public void DiscountLargeLpThirdPackage(
            string desc,
            IEnumerable<GoodShipmentCost> inputEntries,
            List<GoodShipmentCost> expected)
        {
            //Arrange

            //Act
            var actual = _sut.Discount(inputEntries).ToList();

            //Assert
            TestAssert.Equal(expected, actual, desc);
        }
    }
}