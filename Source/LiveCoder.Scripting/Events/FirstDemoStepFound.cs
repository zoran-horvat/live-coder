using System;
using LiveCoder.Scripting.Interfaces;
using LiveCoder.Scripting.Snippets;

namespace LiveCoder.Scripting.Events
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
