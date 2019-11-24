using System;
using System.Linq;
using LiveCoder.Scripting.Lexing.Lexemes;
using Xunit;

namespace LiveCoder.Scripting.Tests.Lexing
{
    public class MultilineTests : TokenizationTestsBase
    {
        [Theory]
        [InlineData(4, "something", "again")]
        [InlineData(8, "something", "again", "and", "again")]
        public void Tokenize_ReceivesMultipleLinesWithSingleWordEach_ReturnsTwiceAsManyTokens(int expectedCount, params string[] lines) =>
            Assert.Equal(
                expectedCount,
                base.Tokenize(lines).Count());

        [Theory]
        [InlineData("something")]
        [InlineData("something", "again")]
        [InlineData("something", "again", "and", "again")]
        public void Tokenize_ReceivesMultipleLinesWithSingleWordEach_ReturnsOnlyIdentifiers(params string[] lines) =>
            Assert.All(base.TokensAtEvenIndices(lines),
                token => Assert.IsType<Identifier>(token));

        [Theory]
        [InlineData("something")]
        [InlineData("something", "again")]
        [InlineData("something", "again", "and", "again")]
        public void Tokenize_ReceivesMultipleLinesWithSingleWordEach_TokenValuesEqualThoseWords(params string[] lines) =>
            Assert.All(lines.Zip(base.TokensAtEvenIndices(lines), (word, token) => (word, token)),
                tuple => Assert.Equal(tuple.word, tuple.token.Value));

        [Theory]
        [InlineData("something")]
        [InlineData("something", "again")]
        [InlineData("something", "again", "and", "again")]
        public void Tokenize_ReceivesMultipleLinesWithSingleWordEach_EveryTokenFollowedByNewLine(params string[] lines) =>
            Assert.All(base.TokensAtOddIndices(lines),
                token => Assert.IsType<EndOfLine>(token));
    }
}
