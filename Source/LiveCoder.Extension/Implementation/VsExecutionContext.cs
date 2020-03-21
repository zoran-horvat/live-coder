using LiveCoder.Scripting;
using LiveCoder.Scripting.Interfaces;

namespace LiveCoder.Extension.Implementation
{
    class VsExecutionContext : IContext
    {
        public ISolution Solution { get; }

        public VsExecutionContext(ISolution solution)
        {
            this.Solution = solution;
        }
    }
}
