using LiveCoder.Scripting.Lexing;
using LiveCoder.Scripting.Text;
using Xunit;

namespace LiveCoder.Scripting.Tests.Lexing
{
    public class TokenizeEmptyTextTests
    {
        [Fact]
        public void ReturnsEmptySequence()
        {
            Assert.Empty(new Lexer().Tokenize(new EmptyText()));
        }
    }
}
