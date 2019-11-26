using System.IO;

namespace LiveCoder.Scripting
{
    public class DemoEngine
    {
        private DemoEngine() { }

        public static DemoEngine For(FileInfo file, IScriptingAuditor auditor) => 
            new DemoEngine();
    }
}
