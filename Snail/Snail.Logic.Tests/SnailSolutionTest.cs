using System;
using System.Collections.Generic;
using Xunit;

namespace Snail.Logic.Tests
{
    public class SnailSolutionTest
    {
        [Fact]
        public void When0X0MatrixThenReturnEmptyArray() => Assert.Empty(SnailSolution.Snail(Array.Empty<int[]>()));

        [Fact]
        public void When1X1MatrixThenReturnThatElement()
        {
            var input = new[] { new[] { 42 } };
            AssertEqualWithDetailedMessage(new[] { 42 }, SnailSolution.Snail(input));
        }

        [Fact]
        public void When2X2MatrixThenReturnSingleRound()
        {
            var input = new[] { new[] { 1, 2 }, new[] { 3, 4 } };
            AssertEqualWithDetailedMessage(new[] { 1, 2, 4, 3 }, SnailSolution.Snail(input));
        }

        [Fact]
        public void When3X3MatrixThenReturnSingleRound()
        {
            var input = new[] { new[] { 1, 2, 3 }, new[] { 4, 5, 6 }, new [] { 7, 8, 9} };
            var actual = SnailSolution.Snail(input);
            AssertEqualWithDetailedMessage(new[] { 1, 2, 3, 6, 9, 8, 7, 4, 5 }, actual);
        }

        private static void AssertEqualWithDetailedMessage(int[] expected, int[] actual)
        {
            var expectedString = ToString(expected);
            var actualString = ToString(actual);
            Assert.Equal(expectedString, actualString);
        }

        private static string ToString(IEnumerable<int> array) => $"[ {string.Join(", ", array)} ]";
    }
}