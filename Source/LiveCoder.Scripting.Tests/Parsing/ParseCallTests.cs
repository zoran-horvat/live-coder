using System;
using System.Collections.Generic;
using System.Linq;
using LiveCoder.Scripting.Lexing;
using LiveCoder.Scripting.Parsing;
using LiveCoder.Scripting.Parsing.Tree;
using LiveCoder.Scripting.Text;
using Xunit;

namespace LiveCoder.Scripting.Tests.Parsing
{
    public class ParseCallTests
    {
        [Theory]
        [InlineData("call()", typeof(MethodReference))]
        [InlineData("something.call()", typeof(AttributeReference), typeof(MethodReference))]
        [InlineData("something.call().again", typeof(AttributeReference), typeof(MethodReference), typeof(AttributeReference))]
        public void ReturnsReferencesOfSpecificTypes(string script, params Type[] referenceTypes) =>
            Assert.All(
                referenceTypes.Zip(this.ParseReferences(script), (expectedType, reference) => (expectedType, reference)),
                tuple => Assert.IsType(tuple.expectedType, tuple.reference));

        [Theory]
        [InlineData("call()", 1)]
        [InlineData("something.call()", 2)]
        [InlineData("something.call().again", 3)]
        public void ReturnsSpecifiedNumberOfReferences(string script, int expectedReferencesCount) =>
            Assert.Equal(expectedReferencesCount, this.ParseReferences(script).Count());

        private IEnumerable<Reference> ParseReferences(params string[] script) =>
            this.ParseReferences(new NonEmptyText(script));

        private IEnumerable<Reference> ParseReferences(IText text) =>
            this.ParseReferences(new Lexer().Tokenize(text));

        private IEnumerable<Reference> ParseReferences(TokensArray tokens) =>
            new Parser().Parse(tokens).SelectMany(expression => expression);
    }
}
