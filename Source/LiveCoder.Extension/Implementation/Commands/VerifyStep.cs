using LiveCoder.Extension.Interfaces;

namespace LiveCoder.Extension.Implementation.Commands
{
    abstract class VerifyStep : IStateVerifier
    {
        public void Execute() { }

        public abstract bool IsStateAsExpected { get; }

        public abstract string PrintableReport { get; }
    }
}
