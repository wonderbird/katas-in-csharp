using Xunit;

namespace TexasHoldemHands.Logic.Tests
{
    public class KataTest
    {
        [Theory]
        [InlineData(new[]{ "K♠", "A♦" }, new[]{ "J♣", "Q♥", "9♥", "2♥", "3♦" }, "nothing", new[]{ "A", "K", "Q", "J", "9" })]
        [InlineData(new[]{ "K♠", "A♦" }, new[]{ "J♣", "10♥", "9♥", "2♥", "3♦" }, "nothing", new[]{ "A", "K", "J", "10", "9" })]
        public void Nothing(
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