using System;
using System.Collections.Generic;
using System.Linq;
using VSExtension.Functional;
using VSExtension.Implementation.Commands;
using VSExtension.Interfaces;

namespace VSExtension.Implementation
{
    class DemoEngine : IEngine
    {
        private Queue<IDemoCommand> Commands { get; } = new Queue<IDemoCommand>();
        private ISolution Solution { get; }

        public DemoEngine(ISolution solution)
        {
            this.Solution = solution ?? throw new ArgumentNullException(nameof(solution));
        }

        private Option<IDemoStep> NextStep =>
            this.Solution.DemoStepsOrdered.FirstOrNone();

        private IEnumerable<IDemoCommand> NextCommands =>
            this.NextStep.Map(step => step.Commands).Reduce(Enumerable.Empty<IDemoCommand>());

        private void PullNewCommands()
        {
            foreach (IDemoCommand command in this.NextCommands)
            {
                this.Commands.Enqueue(command);
            }
        }

        private void PullCommandsIfNoneExecutable()
        {
            if (this.Commands.Count(command => command.CanExecute) == 0)
            {
                this.PullNewCommands();
            }
        }

        private IEnumerable<IDemoCommand> Dequeue()
        {
            while (this.Commands.Count > 0)
            {
                IDemoCommand command = this.Commands.Dequeue();
                if (!command.CanExecute)
                {
                    continue;
                }

                if (command is Pause)
                {
                    yield break;
                }
                yield return command;
            }
        }

        public void Step()
        {
            this.PullCommandsIfNoneExecutable();
            foreach (IDemoCommand command in this.Dequeue())
            {
                command.Execute();
            }
        }
    }
}
