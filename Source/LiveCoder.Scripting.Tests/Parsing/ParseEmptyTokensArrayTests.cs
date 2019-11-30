using LiveCoder.Scripting.Lexing;
using LiveCoder.Scripting.Parsing;
using Xunit;

namespace LiveCoder.Scripting.Tests.Parsing
{
    public class ParseEmptyTokensArrayTests
    {
        [Fact]
        public void ReturnsEmptyInstructionsArray() => 
            Assert.Empty(new Interpreter().Parse(TokensArray.Empty()));
    }
}
