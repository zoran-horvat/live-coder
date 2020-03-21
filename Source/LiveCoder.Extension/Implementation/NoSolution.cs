using LiveCoder.Scripting.Events;
using LiveCoder.Scripting.Interfaces;

namespace LiveCoder.Extension.Implementation
{
    class NoSolution : IEngine
    {
        private ILogger Logger { get; }

        public NoSolution(ILogger logger)
        {
            this.Logger = logger;
        }

        public void Step() => 
            this.Logger.Write(new Error("No solution is open."));
    }
}
