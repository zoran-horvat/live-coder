using System;
using System.Linq;
using LiveCoder.Common.Text.Documents;
using LiveCoder.Scripting.Commands;
using LiveCoder.Scripting.Compiler;
using Xunit;

namespace LiveCoder.Scripting.Tests.Compiler
{
    public class SingleCommandCompilingTests
    {
        [Theory(Skip="Not implemented yet")]
        [InlineData(@"say(""Hello"");", typeof(Say))]
        public void ReturnsCommandOfSpecificType(string line, Type commandType) =>
            Assert.IsType(commandType, this.SingleCommand(line));

        private ICommand SingleCommand(string line) =>
            this.CompiledLine(line).Commands.Single();

        private Script CompiledLine(string line) =>
            new LiveCoderScriptCompiler().Compile(this.Script(line));

        private IText Script(string line) =>
            new NonEmptyText(new[] {line});
    }
}
