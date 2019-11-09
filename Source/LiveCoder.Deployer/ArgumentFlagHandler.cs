using System;

namespace LiveCoder.Deployer
{
    class ArgumentFlagHandler
    {

        private string Flag { get; }
        private Action OnMatch { get; }

        public ArgumentFlagHandler(string flag, Action onMatch)
        {
            this.Flag = flag ?? throw new ArgumentNullException(nameof(flag));
            this.OnMatch = onMatch ?? throw new ArgumentNullException(nameof(onMatch));

        }

        public bool CanHandle(string argument)
        {
            return this.Flag == argument;
        }

        public void Handle(string argument)
        {
            this.OnMatch();
        }
    }
}
