using LiveCoder.Common.Optional;

namespace LiveCoder.Scripting.Parsing
{
    public abstract class Instruction
    {
        public abstract Option<object> Execute(IContext globalScope, Option<object> currentScope);
    }
}
