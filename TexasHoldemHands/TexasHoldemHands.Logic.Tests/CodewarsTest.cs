using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using Xunit;

namespace TexasHoldemHands.Logic.Tests
{
    public class CodewarsTest
    {
        private static readonly StringBuilder template = new();
        private static readonly StringBuilder buffer = new();

        private static readonly string[] ranks = { "A", "K", "Q", "J", "10", "9", "8", "7", "6", "5", "4", "3", "2" };

        private static readonly string[] types =
        {
            "straight-flush", "four-of-a-kind", "full house", "flush", "straight", "three-of-a-kind", "two pair",
            "pair", "nothing"
        };

        private static readonly Dictionary<string, int> ranksLookup =
            ranks.ToDictionary(x => x, x => Array.FindIndex(ranks, y => y == x));

        private static string Show(string str) => $@"""{str}""";

        private static string ShowSeq(IEnumerable<string> seq) => $"[{string.Join(", ", seq.Select(Show))}]";

        private static (string type, string[] ranks) Act(
            string[] holeCards,
            string[] communityCards
        ) =>
            Kata.Hand(holeCards, communityCards);

        private static string Error(string message)
        {
            _ = buffer.Clear();
            _ = buffer.Append(template);
            _ = buffer.AppendLine($"Error: {message}");
            return buffer.ToString();
        }

        private static void Verify(
            (string type, string[] ranks) expected,
            (string type, string[] ranks) actual,
            string[] holeCards,
            string[] communityCards
        )
        {
            _ = template.Clear();
            _ = template.AppendLine($"\tHole cards: {ShowSeq(holeCards)}");
            _ = template.AppendLine($"\tCommunity cards: {ShowSeq(communityCards)}");
            _ = template.AppendLine(
                $"Expected: (type: {Show(expected.type)}, ranks: {ShowSeq(expected.ranks)})"
            );
            Assert.NotNull(actual.type);
            Assert.NotNull(actual.ranks);
            _ = template.AppendLine(
                $"Actual: (type: {Show(actual.type)}, ranks: {ShowSeq(actual.ranks)})"
            );
            Assert.True(
                types.Any(x => string.Equals(x, actual.type, StringComparison.Ordinal)),
                Error($"{Show(actual.type)} is not valid, valid options are: {ShowSeq(types)}")
            );
            Assert.Equal(expected.type, actual.type);
            Assert.Equal(
                expected.ranks.Length,
                actual.ranks.Length
            );
            for (var i = 0; i < expected.ranks.Length; i++)
            {
                Assert.True(
                    ranks.Any(x => string.Equals(x, actual.ranks[i], StringComparison.Ordinal)),
                    Error(
                        $"{Show(actual.ranks[i])} is not valid, valid options are: {ShowSeq(ranks)}"
                    )
                );
            }

            for (var i = 0; i < expected.ranks.Length; i++)
            {
                Assert.Equal(
                    expected.ranks[i],
                    actual.ranks[i]
                );
            }
        }

        #region Sample Tests

        [Fact(DisplayName = "Fixed Tests", Skip = "Not implemented yet.")]
        [SuppressMessage("Usage", "xUnit1004:Test methods should not be skipped",
            Justification = "Tested functionality has not been implemented")]
        public void FixedTests()
        {
            SampleTest(
                ("nothing", new[] { "A", "K", "Q", "J", "9" }),
                new[] { "K♠", "A♦" },
                new[] { "J♣", "Q♥", "9♥", "2♥", "3♦" }
            );
            SampleTest(
                ("pair", new[] { "Q", "K", "J", "9" }),
                new[] { "K♠", "Q♦" },
                new[] { "J♣", "Q♥", "9♥", "2♥", "3♦" }
            );
            SampleTest(
                ("two pair", new[] { "K", "J", "9" }),
                new[] { "K♠", "J♦" },
                new[] { "J♣", "K♥", "9♥", "2♥", "3♦" }
            );
            SampleTest(
                ("three-of-a-kind", new[] { "Q", "J", "9" }),
                new[] { "4♠", "9♦" },
                new[] { "J♣", "Q♥", "Q♠", "2♥", "Q♦" }
            );
            SampleTest(
                ("straight", new[] { "K", "Q", "J", "10", "9" }),
                new[] { "Q♠", "2♦" },
                new[] { "J♣", "10♥", "9♥", "K♥", "3♦" }
            );
            SampleTest(
                ("flush", new[] { "Q", "J", "10", "5", "3" }),
                new[] { "A♠", "K♦" },
                new[] { "J♥", "5♥", "10♥", "Q♥", "3♥" }
            );
            SampleTest(
                ("full house", new[] { "A", "K" }),
                new[] { "A♠", "A♦" },
                new[] { "K♣", "K♥", "A♥", "Q♥", "3♦" }
            );
            SampleTest(
                ("four-of-a-kind", new[] { "2", "3" }),
                new[] { "2♠", "3♦" },
                new[] { "2♣", "2♥", "3♠", "3♥", "2♦" }
            );
            SampleTest(
                ("straight-flush", new[] { "J", "10", "9", "8", "7" }),
                new[] { "8♠", "6♠" },
                new[] { "7♠", "5♠", "9♠", "J♠", "10♠" }
            );
        }

        private static void SampleTest(
            (string type, string[] ranks) expected,
            string[] holeCards,
            string[] communityCards
        )
        {
            var actual = Act(holeCards, communityCards);
            Verify(expected, actual, holeCards, communityCards);
        }

        #endregion
    }
}