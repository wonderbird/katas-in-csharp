namespace StripComments.Logic
{
    public static class StripCommentsSolution
    {
        public static string StripComments(string text, string[] commentSymbols) =>
            text + commentSymbols[0];
    }
}