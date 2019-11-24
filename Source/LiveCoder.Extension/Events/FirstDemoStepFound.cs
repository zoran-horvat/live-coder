using System;
using LiveCoder.Extension.Interfaces;
using LiveCoder.Scripting;

namespace LiveCoder.Extension.Events
{
    class FirstDemoStepFound : IEvent
    {
        private IDemoStep DemoStep { get; }

        public FirstDemoStepFound(IDemoStep demoStep)
        {
            this.DemoStep = demoStep ?? throw new ArgumentNullException(nameof(demoStep));
        }

        public string Label =>
            $"Next step: {this.DemoStep.Label}";
    }
}
