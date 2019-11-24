using System;
using System.Linq;
using LiveCoder.Scripting.Lexing.Lexemes;
using Xunit;

namespace LiveCoder.Scripting.Tests.Lexing
{
    public class MultiTokenLineTests : TokenizationTestsBase
    {
        [Theory]
        [InlineData("something ", 3)]
        [InlineData(" something ", 4)]
        [InlineData("\t something \t again   ", 6)]
        public void Tokenize_ReceivesComplexLine_ReturnsMultipleTokens(string line, int expectedCount) =>
            Assert.Equal(
                expectedCount,
                base.Tokenize(line).Count());

        [Theory]
        [InlineData("something ", typeof(Identifier), typeof(WhiteSpace), typeof(EndOfLine))]
        [InlineData(" something ", typeof(WhiteSpace), typeof(Identifier), typeof(WhiteSpace), typeof(EndOfLine))]
        [InlineData("\t something \t again   ", typeof(WhiteSpace), typeof(Identifier), typeof(WhiteSpace), typeof(Identifier), typeof(WhiteSpace), typeof(EndOfLine))]
        public void Tokenize_ReceivesComplexLine_ReturnsTokensOfSpecificTypes(string line, params Type[] expectedTokenTypes) =>
            Assert.All(
                expectedTokenTypes.Zip(base.Tokenize(line), (expectedType, token) => (expectedType, token)),
                tuple => Assert.Equal(tuple.expectedType, tuple.token.GetType()));

        [Theory]
        [InlineData("something ", "something", " ")]

        [InlineData(" something ", " ", "something", " ")]
        [InlineData("\t something \t again   ", "\t ", "something", " \t ", "again", "   ")]
        public void Tokenize_ReceivesComplexLine_ReturnsTokensWithSpecificValues(string line, params string[] tokenValues) =>
            Assert.All(
                tokenValues.Concat(new[] {new EndOfLine().Value}).Zip(base.Tokenize(line), (expectedValue, token) => (expectedValue, token)),
                tuple => Assert.Equal(tuple.expectedValue, tuple.token.Value));
    }
}
