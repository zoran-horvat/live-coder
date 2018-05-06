using System;
using VSExtension.Implementation.Commands;
using VSExtension.Interfaces;

namespace VSExtension.Events
{
    class StepVerificationFailed : IEvent
    {
        private IStateVerifier Step { get; }

        public StepVerificationFailed(IStateVerifier step)
        {
            this.Step = step ?? throw new ArgumentNullException(nameof(step));
        }

        public string Label =>
            $"Step verification failed:\n{this.Step.PrintableReport}";
    }
}
