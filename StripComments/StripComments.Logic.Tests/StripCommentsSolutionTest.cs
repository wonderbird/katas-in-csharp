using Xunit;

namespace StripComments.Logic.Tests
{
    public class StripCommentsSolutionTest
    {
        [Fact(Skip = "Not implemented yet")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "xUnit1004:Test methods should not be skipped", Justification = "<Pending>")]
        public void StripComments_CodeWarsTests()
        {
            Assert.Equal(
                "apples, pears\ngrapes\nbananas",
                StripCommentsSolution.StripComments(
                    "apples, pears # and bananas\ngrapes\nbananas !apples",
                    new[] { "#", "!" }
                )
            );

            Assert.Equal(
                "a\nc\nd",
                StripCommentsSolution.StripComments("a #b\nc\nd $e f g", new[] { "#", "$" })
            );
        }
    }
}