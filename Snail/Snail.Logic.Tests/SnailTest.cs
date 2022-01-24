using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Xunit;

namespace Snail.Logic.Tests
{
    public class SnailTest
    {
        [Fact(Skip = "Not implemented yet")]
        [SuppressMessage("Usage", "xUnit1004:Test methods should not be skipped")]
        public void SnailTest1()
        {
            int[][] array = { new[] { 1, 2, 3 }, new[] { 4, 5, 6 }, new[] { 7, 8, 9 } };
            var r = new[] { 1, 2, 3, 6, 9, 8, 7, 4, 5 };
            Test(array, r);
        }

        public string Int2dToString(int[][] a) => $"[{string.Join("\n", a.Select(row => $"[{string.Join(",", row)}]"))}]";

        [SuppressMessage("Usage", "xUnit1013:Public method should be marked as test")]
        public void Test(int[][] array, int[] result)
        {
            var text = $"{Int2dToString(array)}\nshould be sorted to\n[{string.Join(",", result)}]\n";
            Console.WriteLine(text);
            Assert.Equal(result, SnailSolution.Snail(array));
        }
    }
}