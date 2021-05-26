using System;
using LiveCoder.Api;
using LiveCoder.Snippets.Interfaces;

namespace LiveCoder.Snippets.Events
{
    class FirstDemoStepFound : DelimitedEvent
    {
        private IDemoStep DemoStep { get; }

        public FirstDemoStepFound(IDemoStep demoStep)
        {
            this.DemoStep = demoStep ?? throw new ArgumentNullException(nameof(demoStep));
        }

        protected override string InnerLabel =>
            $"Next step: {this.DemoStep.Label}";
    }
}
