using System.Linq;
using LiveCoder.Common.Text.Documents;
using LiveCoder.Scripting.Commands;
using LiveCoder.Scripting.Compiler;
using Xunit;

namespace LiveCoder.Scripting.Tests.Compiler
{
    public class SpecificStatementTests
    {
        [Theory]
        [InlineData("Hello")]
        [InlineData("")]
        [InlineData("Something again")]
        public void SayWithSpecifiedString_ReturnsSayStatementWithThatMessage(string message) =>
            Assert.Equal(message, this.Compile<Say>($"say(\"{message}\");").Message);

        private TStatement Compile<TStatement>(string script) where TStatement : IStatement =>
            (TStatement) this.AsSingleStatement(script);

        private IStatement AsSingleStatement(string script) =>
            this.Compile(script).Statements.Single();

        private Script Compile(string script) =>
            ScriptCompiler.Compile(this.AsText(script));

        private IText AsText(string line) =>
            new NonEmptyText(new[] {line});
    }
}
