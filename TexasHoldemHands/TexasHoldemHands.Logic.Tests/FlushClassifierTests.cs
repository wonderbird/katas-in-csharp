using Xunit;
using static TexasHoldemHands.Logic.Kata;

namespace TexasHoldemHands.Logic.Tests
{
    public class FlushClassifierTests
    {
        [Fact]
        public void WhenNoFlushThenCallNextClassifier()
        {
            var handCards = new HandCards(new[] { "4♠", "5♦" }, new[] { "6♣", "7♥", "9♥", "A♥", "3♦" });

            var classifier = new FlushClassifier();
            var mockClassifier = new MockClassifier();
            classifier.RegisterNext(mockClassifier);

            classifier.ClassifyHand(handCards);

            mockClassifier.Verify();
        }

        [Fact]
        public void WhenFlushThenReturnCorrectType()
        {
            var handCards = new HandCards(new[] { "4♥", "5♥" }, new[] { "6♣", "7♥", "9♥", "A♥", "3♦" });

            var classifier = new FlushClassifier();
            var classification = classifier.ClassifyHand(handCards);

            Assert.Equal("flush", classification.Type);
        }

        [Fact]
        public void WhenFlushThenReturnCorrectRanks()
        {
            var handCards = new HandCards(new[] { "4♥", "5♥" }, new[] { "6♣", "7♥", "9♥", "A♥", "3♦" });

            var classifier = new FlushClassifier();
            var classification = classifier.ClassifyHand(handCards);

            Assert.Equal(new[] {"A", "9", "7", "5", "4"}, classification.Ranks);
        }
    }

    public class MockClassifier : HandClassifier
    {
        public bool IsCalled { get; set; }

        public override HandClassification ClassifyHand(HandCards handCards)
        {
            IsCalled = true;
            return null;
        }

        public void Verify()
        {
            Assert.True(IsCalled);
        }
    }
}