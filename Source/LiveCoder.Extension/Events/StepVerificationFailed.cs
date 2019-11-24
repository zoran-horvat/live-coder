using System;
using LiveCoder.Extension.Interfaces;
using LiveCoder.Scripting;

namespace LiveCoder.Extension.Events
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
