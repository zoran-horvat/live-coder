using System;
using LiveCoder.Api;
using LiveCoder.Snippets.Interfaces;

namespace LiveCoder.Snippets.Events
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
