using System;
using LiveCoder.Common.Text.Documents;

namespace LiveCoder.Scripting.Compiler
{
    public class ScriptCompiler
    {
        public Script Compile(IText text) => 
            text is EmptyText ? Script.Empty
            : throw new NotImplementedException();
    }
}
