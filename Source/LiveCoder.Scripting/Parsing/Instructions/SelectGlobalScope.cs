using LiveCoder.Common.Optional;

namespace LiveCoder.Scripting.Parsing.Instructions
{
    public class SelectGlobalScope : Instruction
    {
        public override Option<object> Execute(IContext globalScope, Option<object> currentScope) =>
            Option.Of<object>(globalScope);
    }
}
