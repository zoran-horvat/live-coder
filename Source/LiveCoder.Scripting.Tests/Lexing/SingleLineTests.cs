using System.Collections.Generic;
using System.Linq;
using LiveCoder.Scripting.Lexing;
using LiveCoder.Scripting.Lexing.Lexemes;
using LiveCoder.Scripting.Text;
using Xunit;

namespace LiveCoder.Scripting.Tests.Lexing
{
    public class SingleLineTests
    {
        [Fact]
        public void Tokenize_ReceivesLineContainingSingleWord_ReturnsOneToken() => 
            Assert
                .Single(
                this.TokenizeContentOnly("something"));

        [Fact]
        public void Tokenize_ReceivesSingleWord_ReturnsIdentifier() => 
            Assert
                .IsType<Identifier>(
                this.SingleToken("something"));

        [Theory]
        [InlineData(" ")]
        [InlineData("\t")]
        [InlineData("\t\t   \t    ")]
        [InlineData("   \t   \t   ")]
        public void Tokenize_ReceivesBlankSpace_ReturnsWhiteSpace(string content) =>
            Assert
                .IsType<WhiteSpace>(
                this.SingleToken(content));

        [Theory]
        [InlineData("something")]
        [InlineData("again")]
        public void Tokenize_ReceivesSingleWord_ReturnsTokenContainingThatWord(string word) => 
            Assert.Equal(
                word, 
                this.SingleToken(word).Value);

        private Token SingleToken(params string[] lines) =>
            this.TokenizeContentOnly(lines).Single();

        private IEnumerable<Token> TokenizeContentOnly(params string[] lines) =>
            this.Tokenize(lines).Where(token => !(token is EndOfLine));

        private IEnumerable<Token> Tokenize(params string[] lines) =>
            this.Tokenize(this.InitializeText(lines));

        private IEnumerable<Token> Tokenize(IText text) =>
            new Lexer().Tokenize(text);

        private IText InitializeText(params string[] lines) =>
            new NonEmptyText(lines);
    }
}
