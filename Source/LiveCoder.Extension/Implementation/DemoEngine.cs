﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LiveCoder.Common;
using LiveCoder.Common.Optional;
using LiveCoder.Extension.Events;
using LiveCoder.Extension.Implementation.Commands;
using LiveCoder.Extension.Interfaces;
using LiveCoder.Scripting;

namespace LiveCoder.Extension.Implementation
{
    class DemoEngine : IEngine
    {
        private Queue<IDemoCommand> Commands { get; } = new Queue<IDemoCommand>();
        private ISolution Solution { get; }
        private ILogger Logger { get; }

        private Option<DemoScript> Script { get; set; }
        private ScriptLiveTracker ScriptTracker { get; }

        private FileInfo ScriptFile { get; }

        private Option<DemoScript> ParseScript() =>
            DemoScript.TryParse(this.ScriptFile, this.Logger);

        public static Option<IEngine> TryCreate(ISolution solution, ILogger logger) =>
            TryFindScriptFile(solution).Map<IEngine>(file => new DemoEngine(solution, logger, file));

        private DemoEngine(ISolution solution, ILogger logger, FileInfo scriptFile)
        {
            this.Solution = solution ?? throw new ArgumentNullException(nameof(solution));
            this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.ScriptFile = scriptFile;
            this.LogScriptFile();
            this.Script = this.ParseScript();
            this.ScriptTracker = new ScriptLiveTracker(this.ScriptFile, this.OnScriptFileModified);
        }

        private static Option<FileInfo> TryFindScriptFile(ISolution solution) =>
            solution.File
                .MapNullable(file => file.Directory)
                .Map(dir => Path.Combine(dir.FullName, ".livecoder", "script.lcs"))
                .Map(path => new FileInfo(path))
                .When(file => File.Exists(file.FullName));

        private void OnScriptFileModified(FileInfo scriptFile)
        {
            lock (this.Script)
            {
                this.Script = this.ParseScript();
            }
        }

        private void LogScriptFile() =>
            this.Logger.Write(new ScriptFileFound(this.ScriptFile));

        private Option<IDemoStep> GetNextStep() =>
            this.ReadScriptOptional(script => this.Solution.GetDemoStepsOrdered(script).FirstOrNone())
                .Audit(s => this.Logger.Write(new FirstDemoStepFound(s)))
                .AuditNone(() => this.Logger.Write(new NoDemoStepsFound()));

        private Option<TResult> ReadScript<TResult>(Func<DemoScript, TResult> map)
        {
            lock (this.Script)
            {
                return this.Script.Map(map);
            }
        }

        private Option<TResult> ReadScriptOptional<TResult>(Func<DemoScript, Option<TResult>> map)
        {
            lock (this.Script)
            {
                return this.Script.MapOptional(map);
            }
        }

        private IEnumerable<IDemoCommand> NextCommands =>
            this.GetNextStep()
                .MapOptional(step => this.ReadScript(step.GetCommands))
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
