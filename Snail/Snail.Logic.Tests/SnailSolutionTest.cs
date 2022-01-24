using System;
using Xunit;

namespace Snail.Logic.Tests
{
    public class SnailSolutionTest
    {
        [Fact]
        public void When0x0MatrixThenReturnEmptyArray() => Assert.Empty(SnailSolution.Snail(Array.Empty<int[]>()));

        [Fact]
        public void When1x1MatrixThenReturnThatElement()
        {
            var input = new[] { new[] { 42 } };
            Assert.Equal(new[] { 42 }, SnailSolution.Snail(input));
        }

        [Fact]
        public void When2x2MatrixThenReturnSingleRound()
        {
            var input = new[] { new[] { 1, 2 }, new[] { 3, 4 } };
            Assert.Equal(new[] { 1, 2, 4, 3 }, SnailSolution.Snail(input));
        }
    }
}