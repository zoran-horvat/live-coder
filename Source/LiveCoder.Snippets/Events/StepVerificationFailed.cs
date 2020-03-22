using System;
using LiveCoder.Api;
using LiveCoder.Snippets.Interfaces;

namespace LiveCoder.Snippets.Events
{
    class StepVerificationFailed : IEvent
    {
        private IStateVerifier Step { get; }

        public StepVerificationFailed(IStateVerifier step)
        {
            this.Step = step ?? throw new ArgumentNullException(nameof(step));
        }

        public string Label =>
            $"Step verification failed:{Environment.NewLine}{this.Step.PrintableReport}";
    }
}
