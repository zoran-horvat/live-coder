using System;
using System.Linq;
using LiveCoder.Scripting.Lexing.Lexemes;
using Xunit;

namespace LiveCoder.Scripting.Tests.Lexing
{
    public class TokenizeMultiTokenLineTests : TokenizeTestsBase
    {
        [Theory]
        [InlineData("something ", 3)]
        [InlineData(" something ", 4)]
        [InlineData("\t something \t again   ", 6)]
        public void ComplexLine_ReturnsMultipleTokens(string line, int expectedCount) =>
            Assert.Equal(
                expectedCount,
                base.Tokenize(line).Count());

        [Theory]
        [InlineData("something ", typeof(Identifier), typeof(WhiteSpace), typeof(EndOfLine))]
        [InlineData(" something ", typeof(WhiteSpace), typeof(Identifier), typeof(WhiteSpace), typeof(EndOfLine))]
        [InlineData("\t something \t again   ", typeof(WhiteSpace), typeof(Identifier), typeof(WhiteSpace), typeof(Identifier), typeof(WhiteSpace), typeof(EndOfLine))]
        [InlineData("target.method(argument)", typeof(Identifier), typeof(Operator), typeof(Identifier), typeof(Operator), typeof(Identifier), typeof(Operator), typeof(EndOfLine))]
        [InlineData("target.method(argument1, argument2)", typeof(Identifier), typeof(Operator), typeof(Identifier), typeof(Operator), typeof(Identifier), typeof(Operator), typeof(WhiteSpace), typeof(Identifier), typeof(Operator), typeof(EndOfLine))]
        [InlineData("obj.f(15)", typeof(Identifier), typeof(Operator), typeof(Identifier), typeof(Operator), typeof(Number), typeof(Operator), typeof(EndOfLine))]
        [InlineData("obj.f(15, \"something\")", typeof(Identifier), typeof(Operator), typeof(Identifier), typeof(Operator), typeof(Number), typeof(Operator), typeof(WhiteSpace), typeof(StringLiteral), typeof(Operator), typeof(EndOfLine))]
        public void ComplexLine_ReturnsTokensOfSpecificTypes(string line, params Type[] expectedTokenTypes) =>
            Assert.All(
                expectedTokenTypes.Zip(base.Tokenize(line), (expectedType, token) => (expectedType, token)),
                tuple => Assert.Equal(tuple.expectedType, tuple.token.GetType()));

        [Theory]
        [InlineData("something ", "something", " ")]
        [InlineData(" something ", " ", "something", " ")]
        [InlineData("\t something \t again   ", "\t ", "something", " \t ", "again", "   ")]
        [InlineData("target.method(argument)", "target", ".", "method", "(", "argument", ")")]
        [InlineData("target.method(argument1, argument2)", "target", ".", "method", "(", "argument1", ",", " ", "argument2", ")")]
        [InlineData("obj.f(15)", "obj", ".", "f", "(", "15", ")")]
        [InlineData("obj.f(15, \"something\")", "obj", ".", "f", "(", "15", ",", " ", "\"something\"", ")")]
        public void ComplexLine_ReturnsTokensWithSpecificValues(string line, params string[] tokenValues) =>
            Assert.All(
                tokenValues.Concat(new[] {new EndOfLine().Value}).Zip(base.Tokenize(line), (expectedValue, token) => (expectedValue, token)),
                tuple => Assert.Equal(tuple.expectedValue, tuple.token.Value));
    }
}
