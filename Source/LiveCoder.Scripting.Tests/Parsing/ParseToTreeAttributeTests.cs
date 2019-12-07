using System.Collections.Generic;
using LiveCoder.Scripting.Lexing;
using LiveCoder.Scripting.Parsing;
using LiveCoder.Scripting.Parsing.Tree;
using LiveCoder.Scripting.Text;
using Xunit;

namespace LiveCoder.Scripting.Tests.Parsing
{
    public class ParseToTreeAttributeTests
    {
        [Theory]
        [InlineData("something")]
        [InlineData("something.again")]
        [InlineData("something.again.andAgain")]
        [InlineData("something.again.and.again")]
        public void ReceivesSingleGlobalExpression_ResultContainsOneGlobalExpressionElement(string script) =>
            Assert.Single(this.GetSyntaxTree(script));

        [Theory]
        [InlineData("something")]
        [InlineData("something.again")]
        [InlineData("something.again.and.again")]
        public void ReceivesSingleGlobalExpression_ReturnsNonNullScriptNode(string script) =>
            Assert.NotNull(this.GetSyntaxTree(script));

        private ScriptNode GetSyntaxTree(params string[] scriptLines) =>
            new Parser().ParseTree(this.Tokenize(scriptLines));

        private TokensArray Tokenize(params string[] scriptLines) =>
            new Lexer().Tokenize(new NonEmptyText(scriptLines)).StripSeparators();
    }
}
