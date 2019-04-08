using System.Collections.Generic;
using System.Linq;
using Discounts.Filters;

namespace Discounts
{
    public static class Defaults
    {
        public static readonly Dictionary<(string, string), decimal> CostReference =
            new Dictionary<(string, string), decimal>
            {
                { ("S","LP"), 1.50m},
                { ("M","LP"), 4.90m},
                { ("L","LP"), 6.90m},
                { ("S","MR"), 2.00m},
                { ("M","MR"), 3.00m},
                { ("L","MR"), 4.00m}
            };

        public static string[] DateFormats = { "yyyy-MM-dd", "yyyyMMdd" };
        public static string[] ShippingProviders => CostReference.Keys.Select(x => x.Item2).Distinct().ToArray();
        public static string[] PackageSizes => CostReference.Keys.Select(x => x.Item1).Distinct().ToArray();
        public static DiscountingRules TempOncePerMonth = new OncePerMonthDiscountingRules();

        public const string InputFilePath = "input.txt";
        public const string Separator = " ";
    }
}
