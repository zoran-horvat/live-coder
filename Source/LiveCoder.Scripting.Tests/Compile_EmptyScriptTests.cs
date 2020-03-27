using LiveCoder.Common.Text.Documents;
using LiveCoder.Scripting.Compiler;
using Xunit;

namespace LiveCoder.Scripting.Tests
{
    // ReSharper disable once InconsistentNaming
    public class Compile_EmptyScriptTests
    {
        [Fact]
        public void ReturnsNonNull() => 
            Assert.NotNull(new LiveCoderScriptCompiler().Compile(new EmptyText()));
    }
}
