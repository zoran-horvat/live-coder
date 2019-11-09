using System;
using LiveCoder.Common.Optional;
using LiveCoder.Extension.Interfaces;

namespace LiveCoder.Extension.Events
{
    class FirstDemoStepFound : IEvent
    {
        private IDemoStep DemoStep { get; }

        private FirstDemoStepFound(IDemoStep demoStep)
        {
            this.DemoStep = demoStep ?? throw new ArgumentNullException(nameof(demoStep));
        }

        public static IEvent FromOptionalDemoStep(Option<IDemoStep> step) => 
            step.Map<IEvent>(s => new FirstDemoStepFound(s))
                .Reduce(new NoDemoStepsFound());

        public string Label =>
            $"Next step: {this.DemoStep.Label}";
    }
}
