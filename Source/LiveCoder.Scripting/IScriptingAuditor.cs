namespace LiveCoder.Scripting
{
    public interface IScriptingAuditor
    {
        void ErrorParsingLine(int lineNumber, string content);
    }
}
