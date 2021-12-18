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

        private static int Descending(string x, string y)
        {
            var xIndex = AllRanks.IndexOf(x);
            var yIndex = AllRanks.IndexOf(y);

            return xIndex < yIndex ? -1 : 1;
        }

        private static string Rank(string card) => card[..^1];

        public class HandCards
        {
            public HandCards(string[] holeCards, string[] communityCards)
            {
                AllCards = new List<string>(holeCards);
                AllCards.AddRange(communityCards);
                var sortedRanks = CombineRanksAndSortDescending(AllCards);
                RankFrequencies = CountRankFrequencies(sortedRanks);
                IndividualRanks = RankFrequencies.Where(bin => bin.Value == 1)
                    .Select(bin => bin.Key)
                    .ToList();
            }

            public Dictionary<string, int> RankFrequencies { get; }

            public List<string> IndividualRanks { get; }

            public List<string> AllCards { get; set; }

            private List<string> CombineRanksAndSortDescending(IEnumerable<string> allCards)
            {
                var allRanks = allCards.Select(Rank)
                    .ToList();

                allRanks.Sort(Descending);

                return allRanks;
            }

            private Dictionary<string, int> CountRankFrequencies(List<string> ranks)
            {
                var rankFrequencies = AllRanks.ToDictionary(rank => rank, _ => 0);

                ranks.ForEach(card => rankFrequencies[card]++);

                return rankFrequencies;
            }
        }

        public class HandClassification
        {
            public string Type { get; set; }
            public List<string> Ranks { get; init; } = new();
            public (string type, string[] ranks) Tuple => (Type, Ranks.ToArray());
        }

        public abstract class HandClassifier
        {
            protected HandClassifier Next { get; private set; }

            public abstract HandClassification ClassifyHand(HandCards handCards);

            public HandClassifier RegisterNext(HandClassifier next)
            {
                Next = next;
                return Next;
            }
        }

        public class FlushClassifier : HandClassifier
        {
            public override HandClassification ClassifyHand(HandCards handCards)
            {
                var suits = handCards.AllCards.Select(Suit).ToList();
                var suitFrequencies = _allSuits.ToDictionary(suit => suit, _ => 0);

                suits.ForEach(suit => suitFrequencies[suit]++);

                var isFlush = suitFrequencies.Any(bin => bin.Value >= CardsPerHand);
                if (!isFlush)
                {
                    return Next.ClassifyHand(handCards);
                }

                var suit = suitFrequencies.First(bin => bin.Value >= CardsPerHand).Key;
                var ranks = handCards.AllCards.Where(card => Suit(card) == suit).Select(Rank).ToList();
                ranks.Sort(Descending);

                var handClassification = new HandClassification();
                handClassification.Type = "flush";
                handClassification.Ranks.AddRange(ranks.Take(5));

                return handClassification;
            }

            private readonly char[] _allSuits = { '♠', '♦', '♣', '♥' };

            private char Suit(string arg) => arg[^1];
        }

        private class StraightClassifier : HandClassifier
        {
            private const int RequiredNumberOfConsecutiveCards = 4;

            public override HandClassification ClassifyHand(HandCards handCards)
            {
                var rankSet = handCards.RankFrequencies.Where(bin => bin.Value > 0).Select(bin => bin.Key).ToList();
                var (startIndex, length) = FindConsecutiveCards(rankSet);

                if (length < RequiredNumberOfConsecutiveCards)
                {
                    return Next.ClassifyHand(handCards);
                }

                var handClassification = new HandClassification();
                handClassification.Type = "straight";
                handClassification.Ranks.AddRange(rankSet.Skip(startIndex).Take(CardsPerHand));

                return handClassification;
            }

            private (int startIndex, int length) FindConsecutiveCards(List<string> rankSet)
            {
                var ordinalNumbers = rankSet.Select(OrdinalNumberOf).ToList();
                var countConsecutiveCards = 0;
                var currentIndex = 1;

                while (countConsecutiveCards < RequiredNumberOfConsecutiveCards && currentIndex < ordinalNumbers.Count)
                {
                    if (ordinalNumbers[currentIndex - 1] + 1 == ordinalNumbers[currentIndex])
                    {
                        countConsecutiveCards++;
                    }
                    else
                    {
                        countConsecutiveCards = 0;
                    }

                    currentIndex++;
                }

                return (currentIndex - countConsecutiveCards - 1, countConsecutiveCards);
            }

            private int OrdinalNumberOf(string rank) => AllRanks.IndexOf(rank);
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
                _ = _root
                    .RegisterNext(new FlushClassifier())
                    .RegisterNext(new StraightClassifier())
                    .RegisterNext(new ThreeOfAKindClassifier())
                    .RegisterNext(new NothingClassifier());
            }

            public HandClassification ClassifyHand(string[] holeCards, string[] communityCards) =>
                _root.ClassifyHand(new HandCards(holeCards, communityCards));
        }
    }
}