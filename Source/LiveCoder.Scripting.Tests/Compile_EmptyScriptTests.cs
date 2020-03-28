using LiveCoder.Common.Text.Documents;
using LiveCoder.Scripting.Compiler;
using Xunit;
using System.Linq;

namespace LiveCoder.Scripting.Tests
{
    // ReSharper disable once InconsistentNaming
    public class Compile_EmptyScriptTests
    {
        [Fact]
        public void ReturnsNonNull() => 
            Assert.NotNull(this.CompiledEmptyText());

        [Fact]
        public void ReturnsScriptWithNoCommands() =>
            Assert.Equal(0, this.CompiledEmptyText().Commands.Count());

        private Script CompiledEmptyText() =>
            new LiveCoderScriptCompiler().Compile(new EmptyText());
    }
}
