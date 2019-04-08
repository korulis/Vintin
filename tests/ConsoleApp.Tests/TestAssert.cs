using System.Collections.Generic;
using Discounts;
using Xunit;

namespace ConsoleApp.Tests
{
    public class TestAssert
    {
        public static void Equal(IReadOnlyList<ShipmentCost> expected, IReadOnlyList<ShipmentCost> actual, string desc = "")
        {
            for (var i = 0; i < actual.Count; i++)
            {
                var diffs = AutoCompare.Comparer.Compare(expected[i], actual[i]);
                Assert.True(
                    diffs.Count == 0, diffs.Count == 0 ? "":
                    $"{desc}. Not equal collections. Element index:<{i}>; property: <{diffs[0].Name}>; expected:<{diffs[0].OldValue}>; actual:<{diffs[0].NewValue}>");
            }
        }
    }
}