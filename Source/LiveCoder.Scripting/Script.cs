using System.Collections.Generic;
using System.Linq;

namespace LiveCoder.Scripting
{
    public class Script
    {
        public IEnumerable<ICommand> Commands { get; }

        private Script(IEnumerable<ICommand> commands)
        {
            this.Commands = commands.ToList();
        }

        public static Script Empty => new Script(Enumerable.Empty<ICommand>());
    }
}
