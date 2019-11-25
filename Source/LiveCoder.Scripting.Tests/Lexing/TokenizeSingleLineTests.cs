using System.Collections.Generic;
using System.Linq;
using LiveCoder.Scripting.Lexing;
using LiveCoder.Scripting.Lexing.Lexemes;
using Xunit;

namespace LiveCoder.Scripting.Tests.Lexing
{
    public class TokenizeSingleLineTests : TokenizeTestsBase
    {
        [Fact]
        public void LineContainingSingleWord_ReturnsOneToken() => 
            Assert
                .Single(
                this.TokenizeContentOnly("something"));

        [Fact]
        public void SingleWord_ReturnsIdentifier() => 
            Assert
                .IsType<Identifier>(
                this.SingleToken("something"));

        [Theory]
        [InlineData(" ")]
        [InlineData("\t")]
        [InlineData("\t\t   \t    ")]
        [InlineData("   \t   \t   ")]
        public void BlankSpace_ReturnsWhiteSpace(string content) =>
            Assert
                .IsType<WhiteSpace>(
                this.SingleToken(content));

        [Theory]
        [InlineData("something")]
        [InlineData("again")]
        public void SingleWord_ReturnsTokenContainingThatWord(string word) => 
            Assert.Equal(
                word, 
                this.SingleToken(word).Value);

        private Token SingleToken(params string[] lines) =>
            this.TokenizeContentOnly(lines).Single();

        private IEnumerable<Token> TokenizeContentOnly(params string[] lines) =>
            base.Tokenize(lines).Where(token => !(token is EndOfLine));
    }
}
