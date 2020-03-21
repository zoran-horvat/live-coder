using System;
using LiveCoder.Scripting.Interfaces;

namespace LiveCoder.Scripting.Events
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
