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
            var classifier = new HandClassifierChain();
            var classification = classifier.ClassifyHand(holeCards, communityCards);

            return classification.Tuple;
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
            public (string type, string[] ranks) Tuple => (Type, Ranks.ToArray());
        }

        private abstract class HandClassifier
        {
            protected HandClassifier Next;

            public abstract HandClassification ClassifyHand(HandCards handCards);

            public HandClassifier RegisterNext(HandClassifier next)
            {
                Next = next;
                return Next;
            }
        }

        private class ThreeOfAKindClassifier : HandClassifier
        {
            private const int CardsPerTriple = 3;

            public override HandClassification ClassifyHand(HandCards handCards)
            {
                var isThreeOfAKind = handCards.RankFrequencies.Any(IsTriple);

                if (!isThreeOfAKind)
                {
                    return Next.ClassifyHand(handCards);
                }

                var handClassification = new HandClassification();
                handClassification.Type = ThreeOfAKind;
                handClassification.Ranks.Add(handCards.RankFrequencies.First(IsTriple).Key);
                handClassification.Ranks.AddRange(handCards.IndividualRanks.Take(CardsPerHand - CardsPerTriple));

                return handClassification;
            }

            private bool IsTriple(KeyValuePair<string, int> cardAndQuantity) => cardAndQuantity.Value == CardsPerTriple;
        }

        private class PairClassifier : HandClassifier
        {
            private const int CardsPerPair = 2;

            public override HandClassification ClassifyHand(HandCards handCards)
            {
                var numberOfPairs = handCards.RankFrequencies.Count(bin => bin.Value == CardsPerPair);

                if (numberOfPairs is < 1 or > 2)
                {
                    return Next.ClassifyHand(handCards);
                }

                var handClassification = new HandClassification();
                var numberToPairName = new Dictionary<int, string> { { 1, Pair }, { 2, TwoPair } };
                handClassification.Type = numberToPairName[numberOfPairs];

                var pairRanks = handCards.RankFrequencies.Where(bin => bin.Value == CardsPerPair)
                    .Select(bin => bin.Key)
                    .ToList();
                handClassification.Ranks.AddRange(pairRanks);

                handClassification.Ranks.AddRange(
                    handCards.IndividualRanks.Take(CardsPerHand - numberOfPairs * CardsPerPair));

                return handClassification;
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

            public HandClassification ClassifyHand(string[] holeCards, string[] communityCards) =>
                _root.ClassifyHand(new HandCards(holeCards, communityCards));
        }
    }
}