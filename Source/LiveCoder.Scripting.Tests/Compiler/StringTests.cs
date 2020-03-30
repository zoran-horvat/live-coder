using System;
using System.Collections.Generic;
using System.Linq;
using LiveCoder.Common.Text.Documents;
using LiveCoder.Scripting.Commands;
using LiveCoder.Scripting.Compiler;
using Xunit;

namespace LiveCoder.Scripting.Tests.Compiler
{
    public class StringTests
    {
        [Theory]
        [InlineData("", "")]
        [InlineData("Hello", "Hello")]
        [InlineData("Something, again", "Something, again")]
        public void PlainString_ReturnsExpectedString(string expected, string rawMessage) =>
            Assert.Equal(expected, this.CompiledMessage(string.Empty, rawMessage));

        private string CompiledMessage(string prefix, params string[] rawMessageLines) =>
            this.CompileSay(prefix, rawMessageLines).Message;

        private Say CompileSay(string prefix, params string[] rawMessageLines) =>
            this.CompileSay(this.ToText(prefix, rawMessageLines));

        private IText ToText(string prefix, IEnumerable<string> rawMessageLines) =>
            new NonEmptyText(this.WrapIntoSay(prefix, rawMessageLines).ToArray());

        private IEnumerable<string> WrapIntoSay(string prefix, IEnumerable<string> rawMessageLines) =>
            this.CloseParentheses(rawMessageLines.Select((line, pos) => pos == 0 ?  $"say({prefix}\"{line}" : line));

        private IEnumerable<string> CloseParentheses(IEnumerable<string> lines) =>
            lines.Reverse().Select((line, pos) => pos == 0 ? $"{line}\");" : line).Reverse();

        private Say CompileSay(IText text) =>
            (Say)ScriptCompiler.Compile(text).Statements.Single();
    }
}
