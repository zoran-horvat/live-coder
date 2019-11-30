using LiveCoder.Common.Optional;
using LiveCoder.Scripting.Lexing;
using LiveCoder.Scripting.Text;
using Xunit;

namespace LiveCoder.Scripting.Tests.Lexing
{
    public class TokenizeEmptyTextTests
    {
        [Fact]
        public void ReturnsEmptyTokensArray()
        {
            Assert.IsType<None<Token>>(new Lexer().Tokenize(new EmptyText()).FirstOrNone());
        }
    }
}
