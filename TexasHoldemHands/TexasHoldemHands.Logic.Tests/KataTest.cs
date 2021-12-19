using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace TexasHoldemHands.Logic.Tests
{
    [SuppressMessage("Style", "IDE0060:Remove unused parameter",
        Justification =
            "The parameter 'description' makes the test output more understandable, but is not required for the test itself.")]
    [SuppressMessage("Usage", "xUnit1026:Theory methods should use all of their parameters",
        Justification =
            "The parameter 'description' makes the test output more understandable, but is not required for the test itself.")]
    public class KataTest
    {
        [Theory]
        [InlineData("Codewars test case", new[] { "K♠", "A♦" }, new[] { "J♣", "Q♥", "9♥", "2♥", "3♦" }, "nothing",
            new[] { "A", "K", "Q", "J", "9" })]
        [InlineData("Different community cards than in Codewars test case", new[] { "K♠", "A♦" },
            new[] { "J♣", "10♥", "9♥", "2♥", "3♦" }, "nothing", new[] { "A", "K", "J", "10", "9" })]
        [InlineData("Different hole and community cards than in Codewars test case", new[] { "2♠", "6♦" },
            new[] { "K♣", "8♥", "7♥", "Q♥", "3♦" }, "nothing", new[] { "K", "Q", "8", "7", "6" })]
        public void Nothing(
            string description,
            string[] holeCards,
            string[] communityCards,
            string expectedType,
            string[] expectedRanks
        )
        {
            var (type, ranks) = Kata.Hand(holeCards, communityCards);
            Assert.Equal(expectedType, type);
            Assert.Equal(expectedRanks, ranks);
        }

        [Theory]
        [InlineData("Codewars test case", new[] { "K♠", "Q♦" }, new[] { "J♣", "Q♥", "9♥", "2♥", "3♦" }, "pair",
            new[] { "Q", "K", "J", "9" })]
        public void SinglePair(
            string description,
            string[] holeCards,
            string[] communityCards,
            string expectedType,
            string[] expectedRanks
        )
        {
            var (type, ranks) = Kata.Hand(holeCards, communityCards);
            Assert.Equal(expectedType, type);
            Assert.Equal(expectedRanks, ranks);
        }

        [Theory]
        [InlineData("Codewars test case", new[] { "K♠", "J♦" }, new[] { "J♣", "K♥", "9♥", "2♥", "3♦" }, "two pair",
            new[] { "K", "J", "9" })]
        public void TwoPair(
            string description,
            string[] holeCards,
            string[] communityCards,
            string expectedType,
            string[] expectedRanks
        )
        {
            var (type, ranks) = Kata.Hand(holeCards, communityCards);
            Assert.Equal(expectedType, type);
            Assert.Equal(expectedRanks, ranks);
        }

        [Theory]
        [InlineData("Codewars test case", new[] { "4♠", "9♦" }, new[] { "J♣", "Q♥", "Q♠", "2♥", "Q♦" },
            "three-of-a-kind", new[] { "Q", "J", "9" })]
        public void ThreeOfAKind(
            string description,
            string[] holeCards,
            string[] communityCards,
            string expectedType,
            string[] expectedRanks
        )
        {
            var (type, ranks) = Kata.Hand(holeCards, communityCards);
            Assert.Equal(expectedType, type);
            Assert.Equal(expectedRanks, ranks);
        }

        [Theory]
        [InlineData("Codewars test case", new[] { "Q♠", "2♦" }, new[] { "J♣", "10♥", "9♥", "K♥", "3♦" },
            "straight", new[] { "K", "Q", "J", "10", "9" })]
        [InlineData("Highest not included", new[] { "Q♠", "8♦" }, new[] { "J♣", "10♥", "9♥", "A♥", "3♦" },
            "straight", new[] { "Q", "J", "10", "9", "8" })]
        [InlineData("Start with lowest", new[] { "4♠", "5♦" }, new[] { "6♣", "7♥", "9♥", "A♥", "3♦" },
            "straight", new[] { "7", "6", "5", "4", "3" })]
        public void Straight(
            string description,
            string[] holeCards,
            string[] communityCards,
            string expectedType,
            string[] expectedRanks
        )
        {
            var (type, ranks) = Kata.Hand(holeCards, communityCards);
            Assert.Equal(expectedType, type);
            Assert.Equal(expectedRanks, ranks);
        }

        [Theory]
        [InlineData("Codewars test case", new[] { "A♠", "K♦" }, new[] { "J♥", "5♥", "10♥", "Q♥", "3♥" },
            "flush", new[] { "Q", "J", "10", "5", "3" })]
        public void Flush(
            string description,
            string[] holeCards,
            string[] communityCards,
            string expectedType,
            string[] expectedRanks
        )
        {
            var (type, ranks) = Kata.Hand(holeCards, communityCards);
            Assert.Equal(expectedType, type);
            Assert.Equal(expectedRanks, ranks);
        }

        [Theory]
        [InlineData("Codewars test case", new[] { "A♠", "A♦" }, new[] { "K♣", "K♥", "A♥", "Q♥", "3♦" },
            "full house", new[] { "A", "K" })]
        public void FullHouse(
            string description,
            string[] holeCards,
            string[] communityCards,
            string expectedType,
            string[] expectedRanks
        )
        {
            var (type, ranks) = Kata.Hand(holeCards, communityCards);
            Assert.Equal(expectedType, type);
            Assert.Equal(expectedRanks, ranks);
        }

        [Theory]
        [InlineData("Codewars test case", new[] { "2♠", "3♦" }, new[] { "2♣", "2♥", "3♠", "3♥", "2♦" },
            "four-of-a-kind", new[] { "2", "3" })]
        public void FourOfAKind(
            string description,
            string[] holeCards,
            string[] communityCards,
            string expectedType,
            string[] expectedRanks
        )
        {
            var (type, ranks) = Kata.Hand(holeCards, communityCards);
            Assert.Equal(expectedType, type);
            Assert.Equal(expectedRanks, ranks);
        }

        [Theory]
        [InlineData("Codewars test case", new[] { "8♠", "6♠" }, new[] { "7♠", "5♠", "9♠", "J♠", "10♠" },
            "straight-flush", new[] { "J", "10", "9", "8", "7" })]
        public void StraightFlush(
            string description,
            string[] holeCards,
            string[] communityCards,
            string expectedType,
            string[] expectedRanks
        )
        {
            var (type, ranks) = Kata.Hand(holeCards, communityCards);
            Assert.Equal(expectedType, type);
            Assert.Equal(expectedRanks, ranks);
        }
    }
}