using System;
using System.Collections.Generic;

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

        public static string[] DateFormats => new[] {"yyyy-MM-dd", "yyyymmdd"};
    }
}
