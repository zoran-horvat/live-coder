using LiveCoder.Scripting.Interfaces;

namespace LiveCoder.Scripting.Snippets.Commands
{
    abstract class VerifyStep : IStateVerifier
    {
        public void Execute() { }

        public abstract bool IsStateAsExpected { get; }

        public abstract string PrintableReport { get; }
    }
}
