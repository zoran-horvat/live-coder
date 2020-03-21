namespace LiveCoder.Scripting.Interfaces
{
    public interface ILogger : IScriptingAuditor
    {
        void Write(IEvent @event);
    }
}
