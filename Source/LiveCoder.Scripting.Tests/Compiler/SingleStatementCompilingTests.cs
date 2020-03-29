using System;
using System.Linq;
using LiveCoder.Common.Text.Documents;
using LiveCoder.Scripting.Commands;
using LiveCoder.Scripting.Compiler;
using Xunit;

namespace LiveCoder.Scripting.Tests.Compiler
{
    public class SingleStatementCompilingTests
    {
        [Theory]
        [InlineData(@"say(""Hello"");")]
        public void ReturnsOneStatement(string line) =>
            Assert.Single(this.CompiledLine(line).Statements);

        [Theory]
        [InlineData(@"say(""Hello"");", typeof(Say))]
        public void ReturnsStatementOfSpecificType(string line, Type commandType) =>
            Assert.IsType(commandType, this.SingleStatement(line));

        private IStatement SingleStatement(string line) =>
            this.CompiledLine(line).Statements.Single();

        private Script CompiledLine(string line) =>
            ScriptCompiler.Compile(this.Script(line));

        private IText Script(string line) =>
            new NonEmptyText(new[] {line});
    }
}
