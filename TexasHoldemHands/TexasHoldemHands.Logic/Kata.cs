using System;
using System.Collections.Generic;
using System.Linq;

namespace TexasHoldemHands.Logic
{
    public static class Kata
    {
        private const string Nothing = "nothing";
        private const string Pair = "pair";
        private const string TwoPair = "two pair";

        private const int CardsPerPair = 2;
        private const int CardsPerHand = 5;

        private static readonly List<string> Ranks = new()
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
            var sortedRanks = CombineRanksAndSortDescending(holeCards, communityCards);

            var rankFrequencies = CountRankFrequencies(sortedRanks);

            var rank = "";
            var topRanks = new List<string>();

            var individualRanks = rankFrequencies.Where(bin => bin.Value == 1)
                .Select(bin => bin.Key)
                .ToList();

            var isThreeOfAKind = rankFrequencies.Any(IsTriple);
            if (isThreeOfAKind)
            {
                rank = "three-of-a-kind";
                topRanks.Add(rankFrequencies.First(IsTriple).Key);
                topRanks.AddRange(individualRanks.Take(CardsPerHand - 3));
            }
            else
            {
                var numberOfPairs = rankFrequencies.Count(bin => bin.Value == CardsPerPair);
                var numberToPairName = new Dictionary<int, string> { { 1, Pair }, { 2, TwoPair } };
                if (numberOfPairs is >= 1 and <= 2)
                {
                    rank = numberToPairName[numberOfPairs];
                    var pairRanks = rankFrequencies.Where(bin => bin.Value == CardsPerPair)
                        .Select(bin => bin.Key)
                        .ToList();

                    topRanks.AddRange(pairRanks);
                    topRanks.AddRange(individualRanks.Take(CardsPerHand - numberOfPairs * CardsPerPair));
                }
            }

            if (!topRanks.Any())
            {
                rank = Nothing;
                topRanks.AddRange(individualRanks.Take(CardsPerHand));
            }

            return (rank, topRanks.ToArray());
        }

        private static bool IsTriple(KeyValuePair<string, int> cardAndQuantity) => cardAndQuantity.Value == 3;

        private static List<string> CombineRanksAndSortDescending(
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


        private static string Rank(string card) =>
            card[..^1];

        private static int Descending(string x, string y)
        {
            var xIndex = Ranks.IndexOf(x);
            var yIndex = Ranks.IndexOf(y);

            return xIndex < yIndex ? -1 : 1;
        }

        private static Dictionary<string, int> CountRankFrequencies(List<string> ranks)
        {
            var rankFrequencies = Ranks.ToDictionary(rank => rank, _ => 0);

            ranks.ForEach(card => rankFrequencies[card]++);

            return rankFrequencies;
        }
    }
}