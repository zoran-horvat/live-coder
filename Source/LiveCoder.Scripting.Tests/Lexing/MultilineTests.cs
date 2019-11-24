using System.Collections.Generic;
using System.Linq;
using LiveCoder.Scripting.Lexing;
using LiveCoder.Scripting.Lexing.Lexemes;
using LiveCoder.Scripting.Text;
using Xunit;

namespace LiveCoder.Scripting.Tests.Lexing
{
    public class MultilineTests
    {
        [Theory]
        [InlineData(2, "something", "again")]
        [InlineData(4, "something", "again", "and", "again")]
        public void Tokenize_ReceivesMultipleLinesWithSingleWordEach_ReturnsThatManyTokens(int expectedCount, params string[] lines) =>
            Assert.Equal(
                expectedCount,
                this.Tokenize(lines).Count());

        [Theory]
        [InlineData("something")]
        [InlineData("something", "again")]
        [InlineData("something", "again", "and", "again")]
        public void Tokenize_ReceivesMultipleLinesWithSingleWordEach_ReturnsOnlyIdentifiers(params string[] lines) =>
            Assert.All(this.Tokenize(lines),
                token => Assert.IsType<Identifier>(token));

        [Theory]
        [InlineData("something")]
        [InlineData("something", "again")]
        [InlineData("something", "again", "and", "again")]
        public void Tokenize_ReceivesMultipleLinesWithSingleWordEach_TokenValuesEqualThoseWords(params string[] lines) =>
            Assert.All(lines.Zip(this.Tokenize(lines), (word, token) => (word, token)),
                tuple => Assert.Equal(tuple.word, tuple.token.Value));

        private IEnumerable<Token> Tokenize(params string[] lines) =>
            this.Tokenize(new NonEmptyText(lines));

        private IEnumerable<Token> Tokenize(IText text) =>
            new Lexer().Tokenize(text);
    }
}
