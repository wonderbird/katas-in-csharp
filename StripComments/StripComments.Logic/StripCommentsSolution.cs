using System;
using System.Linq;

namespace StripComments.Logic
{
    public static class StripCommentsSolution
    {
        public static string StripComments(string text, string[] commentSymbols) =>
            string.Join(
                '\n',
                text.Split('\n')
                    .Select(
                        line => line.Split(commentSymbols, StringSplitOptions.None)[0].TrimEnd(' ')
                    )
                    .ToList()
            );
    }
}