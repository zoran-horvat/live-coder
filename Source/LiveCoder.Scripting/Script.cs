using System.Collections.Generic;
using System.Linq;

namespace LiveCoder.Scripting
{
    public class Script
    {
        public IEnumerable<IStatement> Statements { get; }

        public Script(IEnumerable<IStatement> statements)
        {
            this.Statements = statements.ToList();
        }

        public static Script Empty => new Script(Enumerable.Empty<IStatement>());
    }
}
