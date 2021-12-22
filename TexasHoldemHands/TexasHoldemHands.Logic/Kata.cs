using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace TexasHoldemHands.Logic
{
    // TODO: Check special cases: Full House with two triples, three pairs, straight + pair, straight + 2 pairs, straight + triple, flush + pair, ...
    public static class Kata
    {
        private const string Nothing = "nothing";
        private const string Pair = "pair";
        private const string TwoPair = "two pair";
        private const string ThreeOfAKind = "three-of-a-kind";
        private const string Straight = "straight";
        private const string Flush = "flush";
        private const string FullHouse = "full house";
        private const string FourOfAKind = "four-of-a-kind";

        private const int CardsPerHand = 5;
        private const int CardsPerPair = 2;
        private const int CardsPerTriple = 3;

        private static readonly List<string> AllRanksDescending = new()
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

        private static readonly char[] AllSuits = { '♠', '♦', '♣', '♥' };

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
            var xIndex = AllRanksDescending.IndexOf(x);
            var yIndex = AllRanksDescending.IndexOf(y);

            return xIndex < yIndex ? -1 : 1;
        }

        private static string Rank(string card) => card[..^1];

        private static char Suit(string card) => card[^1];

        public class HandCards
        {
            public HandCards(string[] holeCards, string[] communityCards)
            {
                AllCards = new List<string>(holeCards);
                AllCards.AddRange(communityCards);
                
                RanksDescending = AllCards.Select(Rank).ToList();
                RanksDescending.Sort(Descending);

                RankFrequencies = CountRankFrequencies(RanksDescending);
                IndividualRanks = RankFrequencies.Where(bin => bin.Value == 1)
                    .Select(bin => bin.Key)
                    .ToList();

                SuitFrequencies = CountSuitFrequencies(Suits);
            }
            
            public Dictionary<string, int> RankFrequencies { get; }

            public Dictionary<char,int> SuitFrequencies { get; set; }

            // TODO: Do we really need these IndividualRanks - if we have the special cases listed above, IndividualRanks may be useless.
            public List<string> IndividualRanks { get; }

            public List<string> AllCards { get; }

            // TODO: Initialize Ranks and Suits in the constructor

            public List<string> RanksDescending { get; }

            public List<char> Suits => AllCards.Select(Suit).ToList();

            private Dictionary<string, int> CountRankFrequencies(List<string> ranks)
            {
                var rankFrequencies = AllRanksDescending.ToDictionary(rank => rank, _ => 0);

                ranks.ForEach(card => rankFrequencies[card]++);

                return rankFrequencies;
            }

            private Dictionary<char, int> CountSuitFrequencies(List<char> suits)
            {
                var suitFrequencies = AllSuits.ToDictionary(suit => suit, _ => 0);

                suits.ForEach(suit => suitFrequencies[suit]++);
                return suitFrequencies;
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

        public class StraightFlushClassifier : HandClassifier
        {
            // TODO: Remove duplicated RequiredNumberOfConsecutiveCards
            private const int RequiredNumberOfConsecutiveCards = 4;

            public override HandClassification ClassifyHand(HandCards handCards)
            {
                var flushSuit = handCards.SuitFrequencies.FirstOrDefault(bin => bin.Value >= CardsPerHand).Key;
                var isFlush = flushSuit != 0;

                var flushRanks = handCards.AllCards.Where(card => Suit(card) == flushSuit).Select(Rank).ToImmutableSortedSet().Reverse();
                var (startIndex, length) = FindConsecutiveCards(flushRanks);
                var isStraight = length >= RequiredNumberOfConsecutiveCards; 

                if (!isFlush || !isStraight)
                {
                    return Next.ClassifyHand(handCards);
                }

                var handClassification = new HandClassification();
                handClassification.Type = "straight-flush";
                handClassification.Ranks.AddRange(flushRanks.Skip(startIndex).Take(CardsPerHand));

                return handClassification;
            }

            // TODO: Remove duplicated FindConsecutiveCards
            private (int startIndex, int length) FindConsecutiveCards(IEnumerable<string> rankSet)
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

                // TODO: Remove commented code
                //var skipCards = countConsecutiveCards - CardsPerHand;
                //var takeCards = Math.Min(CardsPerHand, countConsecutiveCards);

                return (currentIndex - countConsecutiveCards - 1, countConsecutiveCards);
            }

            // TODO: Remove duplicated OrdinalNumberOf
            private int OrdinalNumberOf(string rank) => AllRanksDescending.IndexOf(rank);

        }

        public class FourOfAKindClassifier : HandClassifier
        {
            public override HandClassification ClassifyHand(HandCards handCards)
            {
                var rank = handCards.RankFrequencies.FirstOrDefault(bin => bin.Value == 4).Key;

                var isFourOfAKind = !string.IsNullOrEmpty(rank);
                if (!isFourOfAKind)
                {
                    return Next.ClassifyHand(handCards);
                }

                var handClassification = new HandClassification();
                handClassification.Type = FourOfAKind;
                handClassification.Ranks.Add(rank);
                handClassification.Ranks.Add(handCards.RankFrequencies.First(bin => bin.Value is > 0 and < 4).Key);
                return handClassification;
            }
        }

        public class FullHouseClassifier : HandClassifier
        {
            public override HandClassification ClassifyHand(HandCards handCards)
            {
                var pairRank = handCards.RankFrequencies.FirstOrDefault(bin => bin.Value == CardsPerPair).Key;
                var tripleRank = handCards.RankFrequencies.FirstOrDefault(bin => bin.Value == CardsPerTriple).Key;
                var isFullHouse = !string.IsNullOrEmpty(pairRank) && !string.IsNullOrEmpty(tripleRank);

                if (!isFullHouse)
                {
                    return Next.ClassifyHand(handCards);
                }

                var handClassification = new HandClassification();
                handClassification.Type = FullHouse;
                handClassification.Ranks.Add(tripleRank);
                handClassification.Ranks.Add(pairRank);

                return handClassification;
            }
        }

        public class FlushClassifier : HandClassifier
        {
            public override HandClassification ClassifyHand(HandCards handCards)
            {
                var flushSuit = handCards.SuitFrequencies.FirstOrDefault(bin => bin.Value >= CardsPerHand).Key;
                var isFlush = flushSuit != 0;

                if (!isFlush)
                {
                    return Next.ClassifyHand(handCards);
                }

                var ranks = handCards.AllCards.Where(card => Suit(card) == flushSuit).Select(Rank).ToList();
                ranks.Sort(Descending);

                var handClassification = new HandClassification();
                handClassification.Type = Flush;
                handClassification.Ranks.AddRange(ranks.Take(CardsPerHand));

                return handClassification;
            }
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
                handClassification.Type = Straight;
                handClassification.Ranks.AddRange(rankSet.Skip(startIndex).Take(CardsPerHand));

                return handClassification;
            }

            private (int startIndex, int length) FindConsecutiveCards(IEnumerable<string> rankSet)
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

            private int OrdinalNumberOf(string rank) => AllRanksDescending.IndexOf(rank);
        }

        private class ThreeOfAKindClassifier : HandClassifier
        {
            public override HandClassification ClassifyHand(HandCards handCards)
            {
                var tripleRank = handCards.RankFrequencies.FirstOrDefault(IsTriple).Key;
                var isThreeOfAKind = !string.IsNullOrEmpty(tripleRank);

                if (!isThreeOfAKind)
                {
                    return Next.ClassifyHand(handCards);
                }

                var handClassification = new HandClassification();
                handClassification.Type = ThreeOfAKind;
                handClassification.Ranks.Add(tripleRank);
                handClassification.Ranks.AddRange(handCards.IndividualRanks.Take(CardsPerHand - CardsPerTriple));

                return handClassification;
            }

            private bool IsTriple(KeyValuePair<string, int> cardAndQuantity) => cardAndQuantity.Value == CardsPerTriple;
        }

        private class PairClassifier : HandClassifier
        {
            public override HandClassification ClassifyHand(HandCards handCards)
            {
                var numberOfPairs = handCards.RankFrequencies.Count(bin => bin.Value == CardsPerPair);

                if (numberOfPairs is < 1 or > 2)
                {
                    return Next.ClassifyHand(handCards);
                }

                var handClassification = new HandClassification();
                handClassification.Type = NumberOfPairsToHandType(numberOfPairs);

                var pairRanks = handCards.RankFrequencies.Where(bin => bin.Value == CardsPerPair)
                    .Select(bin => bin.Key)
                    .ToList();
                handClassification.Ranks.AddRange(pairRanks);

                handClassification.Ranks.AddRange(
                    handCards.IndividualRanks.Take(CardsPerHand - numberOfPairs * CardsPerPair));

                return handClassification;
            }

            private static string NumberOfPairsToHandType(int numberOfPairs)
            {
                var numberToPairName = new Dictionary<int, string> { { 1, Pair }, { 2, TwoPair } };
                return numberToPairName[numberOfPairs];
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
                _root = new StraightFlushClassifier();
                _ = _root
                    .RegisterNext(new FourOfAKindClassifier())
                    .RegisterNext(new FullHouseClassifier())
                    .RegisterNext(new FlushClassifier())
                    .RegisterNext(new StraightClassifier())
                    .RegisterNext(new ThreeOfAKindClassifier())
                    .RegisterNext(new PairClassifier())
                    .RegisterNext(new NothingClassifier());
            }

            public HandClassification ClassifyHand(string[] holeCards, string[] communityCards) =>
                _root.ClassifyHand(new HandCards(holeCards, communityCards));
        }
    }
}