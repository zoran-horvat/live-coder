using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LiveCoder.Common;
using LiveCoder.Common.Optional;
using LiveCoder.Extension.Events;
using LiveCoder.Extension.Implementation.Commands;
using LiveCoder.Extension.Interfaces;
using LiveCoder.Extension.Scripting;

namespace LiveCoder.Extension.Implementation
{
    class DemoEngine : IEngine
    {
        private Queue<IDemoCommand> Commands { get; } = new Queue<IDemoCommand>();
        private ISolution Solution { get; }
        private ILogger Logger { get; }

        private Option<DemoScript> Script { get; }

        private Option<FileInfo> ScriptFile =>
            this.Solution.SolutionFile
                .MapNullable(file => file.Directory)
                .Map(dir => Path.Combine(dir.FullName, ".livecoder", "script.lcs"))
                .Map(path => new FileInfo(path))
                .When(file => file.Exists);

        private Option<DemoScript> ParseScript() =>
            this.ScriptFile.MapOptional(file => DemoScript.TryParse(file, this.Logger));

        public DemoEngine(ISolution solution, ILogger logger)
        {
            this.Solution = solution ?? throw new ArgumentNullException(nameof(solution));
            this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.LogScriptFile();
            this.Script = this.ParseScript();
        }

        private void LogScriptFile() =>
            this.ScriptFile.Do(file => this.Logger.Write(new ScriptFileFound(file)));

        private Option<IDemoStep> GetNextStep()
        {
            Option<IDemoStep> step = this.Script.MapOptional(script => this.Solution.GetDemoStepsOrdered(script).FirstOrNone());
            this.Logger.Write(FirstDemoStepFound.FromOptionalDemoStep(step));
            return step;
        }

        private IEnumerable<IDemoCommand> NextCommands =>
            this.GetNextStep()
                .MapOptional(step => this.Script.Map(step.GetCommands))
                .Reduce(Enumerable.Empty<IDemoCommand>());

        private void PullNewCommands() => 
            this.NextCommands.ToList().ForEach(this.Commands.Enqueue);

        private void PurgeIfVerificationFails() =>
            this.DequeueVerifiers()
                .FirstOrNone(verifier => !verifier.IsStateAsExpected)
                .Do(this.OnVerifierFailed);

        private void OnVerifierFailed(IStateVerifier verifier)
        {
            this.Logger.Write(new StepVerificationFailed(verifier));
            this.Commands.Clear();
        }

        private IEnumerable<IStateVerifier> DequeueVerifiers()
        {
            while (this.TryDequeueVerifier() is Some<IStateVerifier> verifier)
            {
                yield return verifier.Content;
            }
        }

        private Option<IStateVerifier> TryDequeueVerifier() =>
            this.Commands.TryDequeue<IDemoCommand, IStateVerifier>();

        private void PullCommandsIfEmpty()
        {
            if (!this.Commands.Any())
            {
                this.PullNewCommands();
            }
        }

        private IEnumerable<IDemoCommand> Dequeue()
        {
            while (this.Commands.Count > 0)
            {
                IDemoCommand command = this.Commands.Dequeue();

                if (command is Pause)
                {
                    yield break;
                }
                yield return command;
            }
        }

        public void Step()
        {
            this.PurgeIfVerificationFails();
            this.PullCommandsIfEmpty();
            foreach (IDemoCommand command in this.Dequeue())
            {
                command.Execute();
            }
        }
    }
}
