using VSExtension.Interfaces;

namespace VSExtension.Implementation.Commands
{
    abstract class VerifyStep : IStateVerifier
    {
        public void Execute() { }

        public abstract bool IsStateAsExpected { get; }

        public abstract string PrintableReport { get; }
    }
}
