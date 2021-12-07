using System.Collections.Generic;
using System.Linq;

namespace TexasHoldemHands.Logic
{
    public static class Kata
    {
        public static (string type, string[] ranks) Hand(string[] holeCards, string[] communityCards)
        {
            const string nothing = "nothing";

            var allRanks = holeCards.Select(card => card[0..^1])
                .Concat(communityCards.Select(card => card[0..^1]))
                .ToList();
            allRanks.Sort(Descending);

            var cardFrequencies = new Dictionary<string, int>()
            {
                { "A", 0 },
                { "K", 0 },
                { "Q", 0 },
                { "J", 0 },
                { "10", 0 },
                { "9", 0 },
                { "8", 0 },
                { "7", 0 },
                { "6", 0 },
                { "5", 0 },
                { "4", 0 },
                { "3", 0 },
                { "2", 0 },
            };
            allRanks.ForEach(card => cardFrequencies[card]++);
            var rank = nothing;
            if (cardFrequencies.Any(bin => bin.Value == 2))
            {
                var pair = "pair";
                rank = pair;
            }

            var pairRanks = cardFrequencies.Where(bin => bin.Value == 2).Select(bin => bin.Key).ToList();
            var individualRanks = cardFrequencies.Where(bin => bin.Value == 1).Select(bin => bin.Key).Take(5 - 2 * pairRanks.Count).ToList();
            var topRanks = pairRanks.Concat(individualRanks).ToArray();

            return (rank, topRanks);
        }

        private static int Descending(string x, string y)
        {
            var ranks = new List<string> { "A", "K", "Q", "J", "10", "9", "8", "7", "6", "5", "4", "3", "2" };

            var xIndex = ranks.IndexOf(x);
            var yIndex = ranks.IndexOf(y);

            return xIndex < yIndex ? -1 : 1;
        }
    }
}
