using Xunit;

namespace StripComments.Logic.Tests
{
    public class StripCommentsSolutionTest
    {
        [Fact]
        public void StripComments_ReturnsWorld() =>
            Assert.Equal("World!", StripCommentsSolution.StripComments());
    }
}