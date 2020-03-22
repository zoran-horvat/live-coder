﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LiveCoder.Common;
using LiveCoder.Common.IO;
using LiveCoder.Common.Optional;
using LiveCoder.Scripting.Events;
using LiveCoder.Scripting.Interfaces;
using LiveCoder.Scripting.Snippets.Commands;

namespace LiveCoder.Scripting.Snippets
{
    public class CodeSnippetsEngine : IEngine
    {
        private Queue<IDemoCommand> Commands { get; } = new Queue<IDemoCommand>();
        private ISolution Solution { get; }
        private ILogger Logger { get; }

        private Action ActualStep { get; set; }

        private CodeSnippets Script { get; set; }
        private ScriptLiveTracker ScriptTracker { get; }

        private FileInfo ScriptFile { get; }

        private (CodeSnippets, Action) ParseScript() =>
            CodeSnippets.TryParse(this.ScriptFile, this.Logger)
                .Map<(CodeSnippets, Action)>(script => (script, this.PositiveStep))
                .Reduce((CodeSnippets.Empty, this.ParseFailedStep));

        public static Option<IEngine> TryCreate(ISolution solution, ILogger logger) =>
            TryFindScriptFile(solution).Map<IEngine>(file => new CodeSnippetsEngine(solution, logger, file));

        private CodeSnippetsEngine(ISolution solution, ILogger logger, FileInfo scriptFile)
        {
            this.Solution = solution ?? throw new ArgumentNullException(nameof(solution));
            this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.ScriptFile = scriptFile;
            this.LogScriptFile();
            (this.Script, this.ActualStep) = this.ParseScript();
            this.ScriptTracker = new ScriptLiveTracker(this.ScriptFile, this.OnScriptFileModified);
        }

        private static Option<FileInfo> TryFindScriptFile(ISolution solution) => 
            new FileInfo(SnippetsFilePath(solution)).WhenExists();

        private static string SnippetsFilePath(ISolution solution) => 
            Path.Combine(solution.File.Directory.FullName, ".livecoder", "script.lcs");

        private void OnScriptFileModified(FileInfo scriptFile)
        {
            lock (this.Script)
            {
                (this.Script, this.ActualStep) = this.ParseScript();
            }
        }

        private void LogScriptFile() =>
            this.Logger.Write(new ScriptFileFound(this.ScriptFile));

        private IEnumerable<IDemoStep> GetDemoStepsOrdered() =>
            this.Solution.Projects.SelectMany(project => project.GetDemoSteps(this.Script)).OrderBy(step => step.SnippetShortcut);

        private Option<IDemoStep> GetNextStep() =>
            this.ReadScriptOptional(script => this.GetDemoStepsOrdered().FirstOrNone())
                .Audit(s => this.Logger.Write(new FirstDemoStepFound(s)))
                .AuditNone(() => this.Logger.Write(new NoDemoStepsFound()));

        private TResult ReadScript<TResult>(Func<CodeSnippets, TResult> map)
        {
            lock (this.Script)
            {
                return map(this.Script);
            }
        }

        private Option<TResult> ReadScriptOptional<TResult>(Func<CodeSnippets, Option<TResult>> map)
        {
            lock (this.Script)
            {
                return map(this.Script);
            }
        }

        private IEnumerable<IDemoCommand> NextCommands =>
            this.GetNextStep()
                .Map(step => this.ReadScript(step.GetCommands))
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

        public void Step() =>
            this.ActualStep();

        private void ParseFailedStep() =>
            this.Logger.Write(new Error($"Failed parsing snippets file {this.ScriptFile}"));

        private void PositiveStep()
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
