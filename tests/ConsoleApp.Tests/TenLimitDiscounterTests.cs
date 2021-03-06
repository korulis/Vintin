﻿using System.Collections.Generic;
using System.Linq;
using Discounts;
using Discounts.Discounters;
using Discounts.Rules;
using Xunit;

namespace ConsoleApp.Tests
{
    public class TenLimitDiscounterTests
    {
        private readonly RuleBasedDiscounter _sut;

        public TenLimitDiscounterTests()
        {
            _sut = new RuleBasedDiscounter(
                new ZeroDiscounter(),
                ()=>new MonthlyCapDiscountingRules(
                    Defaults.TenMonthlyCap.MonthlyCap));
        }


        public static TheoryData<string, List<ShipmentCost>, List<ShipmentCost>> PackageData
        {
            get
            {
                var b = new ShipmentCostBuilder();
                return new TheoryData<string, List<ShipmentCost>, List<ShipmentCost>>
                {
                    {
                        "single-large-discount-fix",
                        new List<ShipmentCost>
                        {
                            b.OnDay(1).WithPricing(9.0m, 11.0m).Build(),
                        },
                        new List<ShipmentCost>
                        {
                            b.OnDay(1).WithPricing(10.0m,10.0m).Build(),
                        }
                    },
                    {
                        "can-do-nothing",
                        new List<ShipmentCost>
                        {
                            b.OnDay(1).WithPricing(11.0m, 9.0m).Build(),
                        },
                        new List<ShipmentCost>
                        {
                            b.OnDay(1).WithPricing(11.0m,9.0m).Build(),
                        }
                    },
                    {
                        "amend-the-second-discount",
                        new List<ShipmentCost>
                        {
                            b.OnDay(1).WithPricing(1.0m, 5.0m).Build(),
                            b.OnDay(1).WithPricing(1.0m, 7.0m).Build(),
                        },
                        new List<ShipmentCost>
                        {
                            b.OnDay(1).WithPricing(1.0m, 5.0m).Build(),
                            b.OnDay(1).WithPricing(3.0m, 5.0m).Build(),
                        }
                    },
                    {
                        "with-previous-month",
                        new List<ShipmentCost>
                        {
                            b.OnMonth(5).OnDay(1).WithPricing(1.0m, 5.0m).Build(),
                            b.OnMonth(6).OnDay(1).WithPricing(1.0m, 3.0m).Build(),
                            b.OnMonth(6).OnDay(1).WithPricing(1.0m, 8.0m).Build(),
                        },
                        new List<ShipmentCost>
                        {
                            b.OnMonth(5).OnDay(1).WithPricing(1.0m, 5.0m).Build(),
                            b.OnMonth(6).OnDay(1).WithPricing(1.0m, 3.0m).Build(),
                            b.OnMonth(6).OnDay(1).WithPricing(2.0m, 7.0m).Build(),
                        }
                    },
                    {
                        "with-upcoming-month",
                        new List<ShipmentCost>
                        {
                            b.OnMonth(6).OnDay(1).WithPricing(1.0m, 3.0m).Build(),
                            b.OnMonth(6).OnDay(1).WithPricing(1.0m, 5.0m).Build(),
                            b.OnMonth(7).OnDay(1).WithPricing(1.0m, 8.0m).Build(),
                        },
                        new List<ShipmentCost>
                        {
                            b.OnMonth(6).OnDay(1).WithPricing(1.0m, 3.0m).Build(),
                            b.OnMonth(6).OnDay(1).WithPricing(1.0m, 5.0m).Build(),
                            b.OnMonth(7).OnDay(1).WithPricing(1.0m, 8.0m).Build(),
                        }
                    },
                    {
                        "several-months",
                        new List<ShipmentCost>
                        {
                            b.OnMonth(5).OnDay(1).WithPricing(1.0m, 8.0m).Build(),
                            b.OnMonth(5).OnDay(1).WithPricing(1.0m, 5.0m).Build(),
                            b.OnMonth(6).OnDay(1).WithPricing(1.0m, 12.0m).Build(),
                            b.OnMonth(7).OnDay(1).WithPricing(1.0m, 5.0m).Build(),
                            b.OnMonth(7).OnDay(1).WithPricing(1.0m, 8.0m).Build(),
                        },
                        new List<ShipmentCost>
                        {
                            b.OnMonth(5).OnDay(1).WithPricing(1.0m, 8.0m).Build(),
                            b.OnMonth(5).OnDay(1).WithPricing(4.0m, 2.0m).Build(),
                            b.OnMonth(6).OnDay(1).WithPricing(3.0m, 10.0m).Build(),
                            b.OnMonth(7).OnDay(1).WithPricing(1.0m, 5.0m).Build(),
                            b.OnMonth(7).OnDay(1).WithPricing(4.0m, 5.0m).Build(),
                        }
                    },
                    {
                        "several-blocks-same-month",
                        new List<ShipmentCost>
                        {
                            b.WithPricing(1.0m, 8.0m).Build(),
                            b.WithPricing(1.0m, 5.0m).Build(),
                            b.WithPricing(1.0m, 12.0m).Build(),
                        },
                        new List<ShipmentCost>
                        {
                            b.WithPricing(1.0m, 8.0m).Build(),
                            b.WithPricing(4.0m, 2.0m).Build(),
                            b.WithPricing(13.0m, 0.0m).Build(),
                        }
                    },
                };
            }
        }

        [Theory]
        [MemberData(nameof(PackageData))]
        public void DiscountUpToTenPerMonth(
            string desc,
            IEnumerable<ShipmentCost> inputEntries,
            List<ShipmentCost> expected)
        {
            //Arrange

            //Act
            var actual = _sut.Discount(inputEntries).ToList();

            //Assert
            TestAssert.Equal(expected, actual, desc);
        }
    }
}