using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace StripComments.Logic
{
    public static class StripCommentsSolution
    {
        [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        public static string StripComments(string text, string[] commentSymbols)
        {
            var result = text;

            foreach (var commentSymbol in commentSymbols)
            {
                if (text.StartsWith(commentSymbol, StringComparison.CurrentCulture))
                {
                    result = "";
                }
            }
            return result;
        }
    }
}