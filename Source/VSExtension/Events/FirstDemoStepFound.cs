using System;
using VSExtension.Functional;
using VSExtension.Interfaces;

namespace VSExtension.Events
{
    class FirstDemoStepFound : IEvent
    {
        public FirstDemoStepFound(IDemoStep demoStep)
        {
            DemoStep = demoStep ?? throw new ArgumentNullException(nameof(demoStep));
        }

        private IDemoStep DemoStep { get; }

        public static IEvent FromOptionalDemoStep(Option<IDemoStep> step) => step is Some<IDemoStep> some 
            ? (IEvent)new FirstDemoStepFound(some.Content)
            : new NoDemoStepsFound();

        public string Label => "Found the next demo step to execute: " + this.DemoStep.Label;
    }
}
