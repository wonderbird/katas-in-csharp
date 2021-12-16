using System.Collections.Generic;
using System.Linq;

namespace TexasHoldemHands.Logic
{
    public static class Kata
    {
        private const string Nothing = "nothing";
        private const string Pair = "pair";
        private const string TwoPair = "two pair";
        private const string ThreeOfAKind = "three-of-a-kind";

        private const int CardsPerPair = 2;
        private const int CardsPerTriple = 3;
        private const int CardsPerHand = 5;

        private static readonly List<string> AllRanks = new()
        {
            "A",
            "K",
            "Q",
            "J",
            "10",
            "9",
            "8",
            "7",
            "6",
            "5",
            "4",
            "3",
            "2"
        };

        public static (string type, string[] ranks) Hand(
            string[] holeCards,
            string[] communityCards
        )
        {
            var handCards = new HandCards(holeCards, communityCards);
            var handClassifier = new HandClassifierChain();
            var handClassification = handClassifier.ClassifyHand(handCards);

            return (type: handClassification.Type, handClassification.Ranks.ToArray());
        }

        private class HandCards
        {
            public HandCards(string[] holeCards, string[] communityCards)
            {
                var sortedRanks = CombineRanksAndSortDescending(holeCards, communityCards);
                RankFrequencies = CountRankFrequencies(sortedRanks);
                IndividualRanks = RankFrequencies.Where(bin => bin.Value == 1)
                    .Select(bin => bin.Key)
                    .ToList();
            }

            public Dictionary<string, int> RankFrequencies { get; }

            public List<string> IndividualRanks { get; }

            private List<string> CombineRanksAndSortDescending(
                string[] holeCards,
                string[] communityCards
            )
            {
                var allRanks = holeCards.Select(Rank)
                    .Concat(communityCards.Select(Rank))
                    .ToList();

                allRanks.Sort(Descending);

                return allRanks;
            }

            private string Rank(string card) => card[..^1];

            private int Descending(string x, string y)
            {
                var xIndex = AllRanks.IndexOf(x);
                var yIndex = AllRanks.IndexOf(y);

                return xIndex < yIndex ? -1 : 1;
            }

            private Dictionary<string, int> CountRankFrequencies(List<string> ranks)
            {
                var rankFrequencies = AllRanks.ToDictionary(rank => rank, _ => 0);

                ranks.ForEach(card => rankFrequencies[card]++);

                return rankFrequencies;
            }
        }

        private class HandClassification
        {
            public string Type { get; set; }
            public List<string> Ranks { get; init; } = new();
        }

        private abstract class HandClassifier
        {
            protected HandClassifier _next;

            public abstract HandClassification ClassifyHand(HandCards handCards);

            public HandClassifier RegisterNext(HandClassifier next)
            {
                _next = next;
                return _next;
            }
        }

        private class ThreeOfAKindClassifier : HandClassifier
        {
            public override HandClassification ClassifyHand(HandCards handCards)
            {
                var handClassification = new HandClassification();

                var isThreeOfAKind = handCards.RankFrequencies.Any(IsTriple);
                if (isThreeOfAKind)
                {
                    handClassification.Type = ThreeOfAKind;
                    handClassification.Ranks.Add(handCards.RankFrequencies.First(IsTriple).Key);
                    handClassification.Ranks.AddRange(handCards.IndividualRanks.Take(CardsPerHand - CardsPerTriple));

                    return handClassification;
                }

                return _next.ClassifyHand(handCards);
            }

            private bool IsTriple(KeyValuePair<string, int> cardAndQuantity) => cardAndQuantity.Value == CardsPerTriple;
        }

        private class PairClassifier : HandClassifier
        {
            public override HandClassification ClassifyHand(HandCards handCards)
            {
                var handClassification = new HandClassification();

                var numberOfPairs = handCards.RankFrequencies.Count(bin => bin.Value == CardsPerPair);

                var numberToPairName = new Dictionary<int, string> { { 1, Pair }, { 2, TwoPair } };
                if (numberOfPairs is >= 1 and <= 2)
                {
                    handClassification.Type = numberToPairName[numberOfPairs];

                    var pairRanks = handCards.RankFrequencies.Where(bin => bin.Value == CardsPerPair)
                        .Select(bin => bin.Key)
                        .ToList();

                    handClassification.Ranks.AddRange(pairRanks);
                    handClassification.Ranks.AddRange(
                        handCards.IndividualRanks.Take(CardsPerHand - numberOfPairs * CardsPerPair));

                    return handClassification;
                }

                return _next.ClassifyHand(handCards);
            }
        }

        private class NothingClassifier : HandClassifier
        {
            public override HandClassification ClassifyHand(HandCards handCards) =>
                new() { Type = Nothing, Ranks = handCards.IndividualRanks.Take(CardsPerHand).ToList() };
        }

        private class HandClassifierChain
        {
            private readonly HandClassifier _root;

            public HandClassifierChain()
            {
                _root = new PairClassifier();
                _ = _root.RegisterNext(new ThreeOfAKindClassifier())
                    .RegisterNext(new NothingClassifier());
            }

            public HandClassification ClassifyHand(HandCards handCards) => _root.ClassifyHand(handCards);
        }
    }
}