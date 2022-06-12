using LiveCoder.Api;
using LiveCoder.Extension.Implementation.Events;

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
            this.Logger.Write(new NoSolutionOpen());
    }
}
