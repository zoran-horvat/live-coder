using System;
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
        [InlineData(4, "something", "again")]
        [InlineData(8, "something", "again", "and", "again")]
        public void Tokenize_ReceivesMultipleLinesWithSingleWordEach_ReturnsTwiceAsManyTokens(int expectedCount, params string[] lines) =>
            Assert.Equal(
                expectedCount,
                this.Tokenize(lines).Count());

        [Theory]
        [InlineData("something")]
        [InlineData("something", "again")]
        [InlineData("something", "again", "and", "again")]
        public void Tokenize_ReceivesMultipleLinesWithSingleWordEach_ReturnsOnlyIdentifiers(params string[] lines) =>
            Assert.All(this.TokensAtEvenPositions(lines),
                token => Assert.IsType<Identifier>(token));

        [Theory]
        [InlineData("something")]
        [InlineData("something", "again")]
        [InlineData("something", "again", "and", "again")]
        public void Tokenize_ReceivesMultipleLinesWithSingleWordEach_TokenValuesEqualThoseWords(params string[] lines) =>
            Assert.All(lines.Zip(this.TokensAtEvenPositions(lines), (word, token) => (word, token)),
                tuple => Assert.Equal(tuple.word, tuple.token.Value));

        [Theory]
        [InlineData("something")]
        [InlineData("something", "again")]
        [InlineData("something", "again", "and", "again")]
        public void Tokenize_ReceivesMultipleLinesWithSingleWordEach_EveryTokenFollowedByNewLine(params string[] lines) =>
            Assert.All(this.Tokenize(lines).Select((token, index) => (token, index)).Where(tuple => tuple.index % 2 == 1).Select(tuple => tuple.token),
                token => Assert.IsType<EndOfLine>(token));

        private IEnumerable<Token> TokensAtOddIndices(string[] lines) =>
            this.TokensAtIndices(lines, index => index % 2 == 1);

        private IEnumerable<Token> TokensAtEvenPositions(string[] lines) =>
            this.TokensAtIndices(lines, index => index % 2 == 0);

        private IEnumerable<Token> TokensAtIndices(string[] lines, Func<int, bool> predicate) =>
            this.Tokenize(lines)
                .Select((token, index) => (token, index))
                .Where(tuple => predicate(tuple.index))
                .Select(tuple => tuple.token);

        private IEnumerable<Token> Tokenize(params string[] lines) =>
            this.Tokenize(new NonEmptyText(lines));

        private IEnumerable<Token> Tokenize(IText text) =>
            new Lexer().Tokenize(text);
    }
}
