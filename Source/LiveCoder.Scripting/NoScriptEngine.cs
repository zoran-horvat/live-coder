using LiveCoder.Api;
using LiveCoder.Scripting.Events;

namespace LiveCoder.Scripting
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
