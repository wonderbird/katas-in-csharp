using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace StripComments.Logic.Tests
{
    public class StripCommentsSolutionTest
    {
        [Fact(Skip = "Not implemented yet")]
        [SuppressMessage(
            "Usage",
            "xUnit1004:Test methods should not be skipped",
            Justification = "<Pending>"
        )]
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

        [Fact]
        public void StripComments_NoComment_ReturnsSameString()
        {
            const string input = "anything";
            Assert.Equal(input, StripCommentsSolution.StripComments(input, Array.Empty<string>()));
        }

        [Fact]
        public void StripComments_SingleLineStartingWithComment_ReturnsBlank()
        {
            string[] commentSymbols = { "#" };
            const string input = "# Hello World!";

            Assert.Equal("", StripCommentsSolution.StripComments(input, commentSymbols));
        }

        [Fact]
        public void StripComments_SingleLineStartingWithSecondComment_ReturnsBlank()
        {
            string[] commentSymbols = { "#", "//" };
            const string input = "// Hello World!";

            Assert.Equal("", StripCommentsSolution.StripComments(input, commentSymbols));
        }

        [Fact(Skip = "TODO: Implement this feature next.")]
        [SuppressMessage("Usage", "xUnit1004:Test methods should not be skipped", Justification = "<Pending>")]
        public void StripComments_TwoLinesWithCommentInSecondLine_ReturnsFirstLine()
        {
            string[] commentSymbols = { "#" };
            const string firstLine = "Hello";
            const string secondLine = "# World";
            const string input = firstLine + "\n" + secondLine;

            Assert.Equal(firstLine, StripCommentsSolution.StripComments(input, commentSymbols));
        }
    }
}