using System.Linq;
using LiveCoder.Common.Text.Documents;
using LiveCoder.Scripting.Compiler;
using Xunit;

namespace LiveCoder.Scripting.Tests.Compiler
{
    // ReSharper disable once InconsistentNaming
    public class Compile_EmptyScriptTests
    {
        [Fact]
        public void ReturnsNonNull() => 
            Assert.NotNull(this.CompiledEmptyText());

        [Fact]
        public void ReturnsScriptWithNoCommands() =>
            Assert.Empty(this.CompiledEmptyText().Commands);

        private Script CompiledEmptyText() =>
            new LiveCoderScriptCompiler().Compile(new EmptyText());
    }
}
