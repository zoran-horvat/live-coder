﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Common.Optional;
using LiveCoderExtension.Events;
using LiveCoderExtension.Functional;
using LiveCoderExtension.Implementation.Commands;
using LiveCoderExtension.Interfaces;

namespace LiveCoderExtension.Implementation
{
    class DemoEngine : IEngine
    {
        private Queue<IDemoCommand> Commands { get; } = new Queue<IDemoCommand>();
        private ISolution Solution { get; }
        private ILogger Logger { get; }

        private Option<FileInfo> ScriptFile =>
            this.Solution.SolutionFile
                .MapNullable(file => file.Directory)
                .Map(dir => Path.Combine(dir.FullName, ".livecoder", "script.lcs"))
                .Map(path => new FileInfo(path))
                .When(file => file.Exists);

        public DemoEngine(ISolution solution, ILogger logger)
        {
            this.Solution = solution ?? throw new ArgumentNullException(nameof(solution));
            this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.LogScriptFile();
        }

        private void LogScriptFile() =>
            this.ScriptFile.Do(file => this.Logger.Write(new ScriptFileFound(file)));

        private Option<IDemoStep> NextStep =>
            this.Logger.LogAndReturn(FirstDemoStepFound.FromOptionalDemoStep,
                this.Solution.DemoStepsOrdered.FirstOrNone());

        private IEnumerable<IDemoCommand> NextCommands =>
            this.NextStep.Map(step => step.Commands).Reduce(Enumerable.Empty<IDemoCommand>());

        private void PullNewCommands()
        {
            foreach (IDemoCommand command in this.NextCommands)
            {
                this.Commands.Enqueue(command);
            }
        }

        private void PurgeIfVerificationFails()
        {
            while (this.Commands.Any() && this.Commands.Peek() is IStateVerifier)
            {
                IStateVerifier verifier = this.Commands.Dequeue() as IStateVerifier;
                if (!verifier.IsStateAsExpected)
                {
                    this.Logger.Write(new StepVerificationFailed(verifier));
                    this.Commands.Clear();
                }
            }
        }

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
