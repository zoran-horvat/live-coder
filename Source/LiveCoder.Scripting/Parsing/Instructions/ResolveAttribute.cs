using LiveCoder.Common.Optional;

namespace LiveCoder.Scripting.Parsing.Instructions
{
    public class ResolveAttribute : Instruction
    {
        public override Option<object> Execute(IContext globalScope, Option<object> currentScope) => 
            throw new System.NotImplementedException();
    }
}
