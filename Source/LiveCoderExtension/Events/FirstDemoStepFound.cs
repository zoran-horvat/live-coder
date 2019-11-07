using System;
using Common.Optional;
using LiveCoderExtension.Interfaces;

namespace LiveCoderExtension.Events
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
