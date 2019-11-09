using System;
using System.Diagnostics.Contracts;

namespace LiveCoder.Deployer
{
    class ArgumentFlagHandler
    {

        private string Flag { get; }
        private Action OnMatch { get; }

        public ArgumentFlagHandler(string flag, Action onMatch)
        {

            Contract.Requires(!string.IsNullOrEmpty(flag));
            Contract.Requires(onMatch != null);

            this.Flag = flag;
            this.OnMatch = onMatch;

        }

        [Pure]
        public bool CanHandle(string argument)
        {
            Contract.Requires(argument != null);
            return this.Flag == argument;
        }

        public void Handle(string argument)
        {
            Contract.Requires(this.CanHandle(argument));
            this.OnMatch();
        }
    }
}
