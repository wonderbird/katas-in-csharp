using Xunit;

namespace TexasHoldemHands.Logic.Tests
{
    public class KataTest
    {
        [Theory]
        [InlineData("Codewars test case", new[] { "K♠", "A♦" }, new[] { "J♣", "Q♥", "9♥", "2♥", "3♦" }, "nothing", new[] { "A", "K", "Q", "J", "9" })]
        [InlineData("Different community cards than in Codewars test case", new[] { "K♠", "A♦" }, new[] { "J♣", "10♥", "9♥", "2♥", "3♦" }, "nothing", new[] { "A", "K", "J", "10", "9" })]
        [InlineData("Different hole and community cards than in Codewars test case", new[] { "2♠", "6♦" }, new[] { "K♣", "8♥", "7♥", "Q♥", "3♦" }, "nothing", new[] { "K", "Q", "8", "7", "6" })]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "The parameter 'description' makes the test output more understandable, but is not required for the test itself.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "xUnit1026:Theory methods should use all of their parameters", Justification = "The parameter 'description' makes the test output more understandable, but is not required for the test itself.")]
        public void Nothing(
            string description,
            string[] holeCards,
            string[] communityCards,
            string expectedType,
            string[] expectedRanks
        )
        {
            (var type, var ranks) = Kata.Hand(holeCards, communityCards);
            Assert.Equal(expectedType, type);
            Assert.Equal(expectedRanks, ranks);
        }
    }
}