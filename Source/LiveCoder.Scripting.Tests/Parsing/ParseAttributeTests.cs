using System.Collections.Generic;
using System.Linq;
using LiveCoder.Scripting.Lexing;
using LiveCoder.Scripting.Parsing;
using LiveCoder.Scripting.Parsing.Tree;
using LiveCoder.Scripting.Text;
using Xunit;

namespace LiveCoder.Scripting.Tests.Parsing
{
    public class ParseAttributeTests
    {
        [Theory]
        [InlineData("something")]
        [InlineData("something.again")]
        [InlineData("something.again.andAgain")]
        [InlineData("something.again.and.again")]
        [InlineData("something.again.and.again.oneMore")]
        public void ReceivesSingleGlobalExpression_ResultContainsOneGlobalExpressionElement(string script) =>
            Assert.Single(this.GetSyntaxTree(script));

        [Theory]
        [InlineData("something")]
        [InlineData("something.again")]
        [InlineData("something.again.and.again")]
        [InlineData("something.again.and.again.oneMore")]
        public void ReceivesSingleGlobalExpression_ReturnsNonNullScriptNode(string script) =>
            Assert.NotNull(this.GetSyntaxTree(script));

        [Theory]
        [InlineData("something", "again")]
        [InlineData("something.again", "and")]
        [InlineData("something.again", "and.again")]
        [InlineData("something.again", "and.again", "and.one.more.line")]
        [InlineData("something.again.and.again.oneMore", "something.again.and.again.oneMore", "something.again.and.again.oneMore", "something.again.and.again.oneMore", "something.again.and.again.oneMore")]
        public void ReceivesMultipleGlobalExpressions_ReturnsThatManyGlobalExpressionElements(params string[] scriptLines) =>
            Assert.Equal(
                scriptLines.Length,
                this.GetSyntaxTree(scriptLines).Count());

        [Theory]
        [InlineData("something", 1)]
        [InlineData("something.again", 2)]
        [InlineData("something again", 1, 1)]
        [InlineData("something.again and", 2, 1)]
        [InlineData("something.again and.one.again", 2, 3)]
        [InlineData("something.again and.again and.one.more.line", 2, 2, 4)]
        [InlineData("something.again.and.again.oneMore something.again.and.again.oneMore something.again.and.again.oneMore something.again.and.again.oneMore something.again.and.again.oneMore", 5, 5, 5, 5, 5)]
        public void ReturnsSpecifiedNumberOfReferencesPerGlobalExpression(string script, params int[] expectedCounts) =>
            Assert.All(
                this.GetReferenceCounts(this.GetSyntaxTree(script), expectedCounts),
                tuple => Assert.Equal(tuple.expectedCount, tuple.actualCount));

        private IEnumerable<(int expectedCount, int actualCount)> GetReferenceCounts(ScriptNode script, IEnumerable<int> expectedCounts) =>
            expectedCounts.Zip(
                script.Select(expression => expression.Count()), 
                (expectedCount, actualCount) => (expectedCount, actualCount));

        [Theory]
        [InlineData("something", "something")]
        [InlineData("something.again", "something", "again")]
        [InlineData("something.again.and", "something", "again", "and")]
        [InlineData("something.again.and.oneMore", "something", "again", "and", "oneMore")]
        public void ReturnsGlobalExpressionWithSpecificIdentifierNames(string script, params string[] expectedNames) =>
            Assert.All(
                this.GetIdentifierNames(this.FirstGlobalExpression(script), expectedNames),
                tuple => Assert.Equal(tuple.expectedName, tuple.actualName));

        private IEnumerable<(string expectedName, string actualName)> GetIdentifierNames(GlobalExpression expression, IEnumerable<string> expectedNames) =>
            expectedNames.Zip(
                expression.OfType<AttributeReference>().Select(reference => reference.Identifier.Value),
                (expectedName, actualName) => (expectedName, actualName));

        private GlobalExpression FirstGlobalExpression(params string[] scriptLines) =>
            this.GetSyntaxTree(scriptLines).First();

        private ScriptNode GetSyntaxTree(params string[] scriptLines) =>
            new Parser().Parse(this.Tokenize(scriptLines));

        private TokensArray Tokenize(params string[] scriptLines) =>
            new Lexer().Tokenize(new NonEmptyText(scriptLines));
    }
}
