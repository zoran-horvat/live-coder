using LiveCoder.Snippets.Interfaces;

namespace LiveCoder.Snippets.Commands
{
    abstract class VerifyStep : IStateVerifier
    {
        public void Execute() { }

        public abstract bool IsStateAsExpected { get; }

        public abstract string PrintableReport { get; }
    }
}
