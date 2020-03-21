using LiveCoder.Scripting.Events;
using LiveCoder.Scripting.Interfaces;

namespace LiveCoder.Extension.Implementation
{
    class NoScriptEngine : IEngine
    {
        private ILogger Logger { get; }

        public NoScriptEngine(ILogger logger)
        {
            this.Logger = logger;
        }

        public void Step() => 
            this.Logger.Write(new Error("No demo script found."));
    }
}
