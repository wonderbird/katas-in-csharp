using System.Collections.Generic;
using System.Linq;

namespace TexasHoldemHands.Logic
{
    public static class Kata
    {
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
            const string nothing = "nothing";

            var allRanks = holeCards.Select(card => card[..^1])
                .Concat(communityCards.Select(card => card[..^1]))
                .ToList();
            allRanks.Sort(Descending);

            var cardFrequencies = Ranks.ToDictionary(rank => rank, _ => 0);
            allRanks.ForEach(card => cardFrequencies[card]++);
            var rank = nothing;
            if (cardFrequencies.Any(bin => bin.Value == 2))
            {
                const string pair = "pair";
                rank = pair;
            }

            var pairRanks = cardFrequencies.Where(bin => bin.Value == 2)
                .Select(bin => bin.Key)
                .ToList();
            var individualRanks = cardFrequencies.Where(bin => bin.Value == 1)
                .Select(bin => bin.Key)
                .Take(5 - 2 * pairRanks.Count)
                .ToList();
            var topRanks = pairRanks.Concat(individualRanks).ToArray();

            return (rank, topRanks);
        }

        private static int Descending(string x, string y)
        {
            var xIndex = Ranks.IndexOf(x);
            var yIndex = Ranks.IndexOf(y);

            return xIndex < yIndex ? -1 : 1;
        }
    }
}