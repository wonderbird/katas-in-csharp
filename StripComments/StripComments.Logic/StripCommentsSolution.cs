using System.Collections.Generic;
using System.Linq;

namespace StripComments.Logic
{
    public static class StripCommentsSolution
    {
        public static string StripComments(string text, string[] commentSymbols)
        {
            const char endOfLine = '\n';

            var inputLines = text.Split(endOfLine);

            var outputLines = inputLines
                .Select(line => StripCommentsFromSingleLine(line, commentSymbols))
                .ToList();

            return string.Join(endOfLine, outputLines);
        }

        private static string StripCommentsFromSingleLine(
            string line,
            IEnumerable<string> commentSymbols
        )
        {
            var remainder = line;

            foreach (var commentSymbol in commentSymbols)
            {
                remainder = remainder.Split(commentSymbol)[0].TrimEnd();
            }

            return remainder;
        }
    }
}