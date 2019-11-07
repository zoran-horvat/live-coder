using System;
using Common.Optional;
using LiveCoderExtension.Interfaces;

namespace LiveCoderExtension.Events
{
    class FirstDemoStepFound : IEvent
    {
        private FirstDemoStepFound(IDemoStep demoStep)
        {
            DemoStep = demoStep ?? throw new ArgumentNullException(nameof(demoStep));
        }

        private IDemoStep DemoStep { get; }

        public static IEvent FromOptionalDemoStep(Option<IDemoStep> step) => 
            step.Map<IEvent>(s => new FirstDemoStepFound(s))
                .Reduce(new NoDemoStepsFound());

        public string Label =>
            $"Found the next demo step to execute: {this.DemoStep.Label}";
    }
}
