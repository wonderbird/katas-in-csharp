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
        public void WhenArrayWithEmptyArrayThenReturnEmptyArray() => Assert.Empty(SnailSolution.Snail(new []{ Array.Empty<int>() }));

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
        public void When3X3MatrixThenStopAtCenter()
        {
            var input = new[] { new[] { 1, 2, 3 }, new[] { 4, 5, 6 }, new[] { 7, 8, 9 } };
            AssertEqualWithDetailedMessage(new[] { 1, 2, 3, 6, 9, 8, 7, 4, 5 }, SnailSolution.Snail(input));
        }

        [Fact]
        public void When4X4MatrixThenReturnTwoRounds()
        {
            var input = new[]
            {
                new[] { 1, 2, 3, 4 }, new[] { 5, 6, 7, 8 }, new[] { 9, 10, 11, 12 }, new[] { 13, 14, 15, 16 }
            };
            AssertEqualWithDetailedMessage(new[] { 1, 2, 3, 4, 8, 12, 16, 15, 14, 13, 9, 5, 6, 7, 11, 10 },
                SnailSolution.Snail(input));
        }

        [Fact]
        public void When5X5MatrixThenReturnTwoAndAHalfRound()
        {
            var input = new[]
            {
                new[] { 1,  2,   3,  4,  5 },
                new[] { 6,  7,   8,  9, 10 },
                new[] { 11, 12, 13, 14, 15 },
                new[] { 16, 17, 18, 19, 20 },
                new[] { 21, 22, 23, 24, 25 }
            };
            AssertEqualWithDetailedMessage(new[] { 1, 2, 3, 4, 5, 10, 15, 20, 25, 24, 23, 22, 21, 16, 11, 6, 7, 8, 9, 14, 19, 18, 17, 12, 13 },
                SnailSolution.Snail(input));
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