using System;
using System.Linq;
using LiveCoder.Scripting.Lexing.Lexemes;
using Xunit;

namespace LiveCoder.Scripting.Tests.Lexing
{
    public class TokenizeInvalidInputTests : TokenizeTestsBase
    {
        [Theory]
        [InlineData("one(###)two", typeof(Identifier), typeof(Operator), typeof(InvalidToken), typeof(EndOfLine))]
        [InlineData("one(\"not closed)two", typeof(Identifier), typeof(Operator), typeof(InvalidToken), typeof(EndOfLine))]
        [InlineData("one(\"not\\escaped\")two", typeof(Identifier), typeof(Operator), typeof(InvalidToken), typeof(EndOfLine))]
        public void InvalidInput_ReturnsSpecificTokenTypes(string input, params Type[] expectedTokenTypes) =>
            Assert.All(
                expectedTokenTypes.Zip(base.Tokenize(input), (expectedType, token) => (expectedType, token)),
                tuple => Assert.IsType(tuple.expectedType, tuple.token));

        [Theory]
        [InlineData("one(#$!%)two", "one", "(", "#$!%)two")]
        [InlineData("one(\"not closed)two", "one", "(", "\"not closed)two")]
        [InlineData("one(\"not\\escaped\")two", "one", "(", "\"not\\escaped\")two")]
        public void InvalidInput_ReturnsTokensWithSpecificValues(string input, params string[] expectedTokenValues) =>
            Assert.All(
                expectedTokenValues.Concat(new[] {new EndOfLine().Value}).Zip(base.Tokenize(input), (expectedValue, token) => (expectedValue, token)),
                tuple => Assert.Equal(tuple.expectedValue, tuple.token.Value));
    }
}
