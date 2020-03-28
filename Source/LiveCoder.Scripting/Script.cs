using System.Collections.Generic;
using System.Linq;

namespace LiveCoder.Scripting
{
    public class Script
    {
        public IEnumerable<IStatement> Statements { get; }

        private Script(IEnumerable<IStatement> commands)
        {
            this.Statements = commands.ToList();
        }

        public static Script Empty => new Script(Enumerable.Empty<IStatement>());
    }
}
