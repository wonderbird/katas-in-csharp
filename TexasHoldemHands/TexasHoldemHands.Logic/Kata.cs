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

            var topRanks = allRanks.Take(5).ToArray();
            return (nothing, topRanks);
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
