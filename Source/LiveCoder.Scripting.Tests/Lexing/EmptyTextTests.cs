using LiveCoder.Scripting.Lexing;
using LiveCoder.Scripting.Text;
using Xunit;

namespace LiveCoder.Scripting.Tests.Lexing
{
    public class EmptyTextTests
    {
        [Fact]
        public void Tokenize_ReceivesEmptyText_ReturnsEmptySequence()
        {
            Assert.Empty(new Lexer().Tokenize(new EmptyText()));
        }
    }
}
