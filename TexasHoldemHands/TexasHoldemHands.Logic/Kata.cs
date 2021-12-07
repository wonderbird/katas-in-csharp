using System.Collections.Generic;
using System.Linq;

namespace TexasHoldemHands.Logic
{
    public static class Kata
    {
        private const string Nothing = "nothing";
        private const string Pair = "pair";
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

            var rank = Nothing;
            if (rankFrequencies.Any(bin => bin.Value == CardsPerPair))
            {
                rank = Pair;
            }

            var pairRanks = rankFrequencies.Where(bin => bin.Value == CardsPerPair)
                .Select(bin => bin.Key)
                .ToList();

            var individualRanks = rankFrequencies.Where(bin => bin.Value == 1)
                .Select(bin => bin.Key)
                .Take(CardsPerHand - CardsPerPair * pairRanks.Count)
                .ToList();

            var topRanks = pairRanks.Concat(individualRanks).ToArray();

            return (rank, topRanks);
        }

        private static Dictionary<string, int> CountRankFrequencies(List<string> sortedRanks)
        {
            var rankFrequencies = Ranks.ToDictionary(rank => rank, _ => 0);
            sortedRanks.ForEach(card => rankFrequencies[card]++);
            return rankFrequencies;
        }

        private static List<string> CombineRanksAndSortDescending(
            string[] holeCards,
            string[] communityCards
        )
        {
            var allRanks = holeCards.Select(card => card[..^1])
                .Concat(communityCards.Select(card => card[..^1]))
                .ToList();
            allRanks.Sort(Descending);
            return allRanks;
        }

        private static int Descending(string x, string y)
        {
            var xIndex = Ranks.IndexOf(x);
            var yIndex = Ranks.IndexOf(y);

            return xIndex < yIndex ? -1 : 1;
        }
    }
}