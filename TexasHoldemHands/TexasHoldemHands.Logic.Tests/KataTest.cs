using Xunit;

namespace TexasHoldemHands.Logic.Tests
{
    public class KataTest
    {
        [Fact]
        public void Hand_Nothing()
        {
            string[] holeCards = { "K♠", "A♦" };
            string[] communityCards = { "J♣", "Q♥", "9♥", "2♥", "3♦" };

            string type;
            string[] ranks;
            (type, ranks) = Kata.Hand(holeCards, communityCards);

            const string expectedType = "nothing";
            Assert.Equal(expectedType, type);

            string[] expectedRanks = { "A", "K", "Q", "J", "9" };
            Assert.Equal(expectedRanks, ranks);
        }
    }
}