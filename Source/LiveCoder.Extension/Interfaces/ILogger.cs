using LiveCoder.Scripting;

namespace LiveCoder.Extension.Interfaces
{
    public interface ILogger : IScriptingAuditor
    {
        void Write(IEvent @event);
    }
}
