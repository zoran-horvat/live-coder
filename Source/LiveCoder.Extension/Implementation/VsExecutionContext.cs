
using LiveCoder.Extension.Interfaces;

namespace LiveCoder.Extension.Implementation
{
    class VsExecutionContext : Scripting.IContext
    {
        public Scripting.Execution.ISolution Solution { get; }

        public VsExecutionContext(ISolution solution)
        {
            this.Solution = solution;
        }
    }
}
