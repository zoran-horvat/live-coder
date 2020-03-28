using System.Linq;
using LiveCoder.Common.Text.Documents;
using LiveCoder.Scripting.Compiler;

namespace LiveCoder.Scripting.Tests.Compiler
{
    public class SingleCommandCompilingTests
    {

        private ICommand SingleCommand(string line) =>
            this.CompiledLine(line).Commands.Single();

        private Script CompiledLine(string line) =>
            new LiveCoderScriptCompiler().Compile(this.Script(line));

        private IText Script(string line) =>
            new NonEmptyText(new[] {line});
    }
}
