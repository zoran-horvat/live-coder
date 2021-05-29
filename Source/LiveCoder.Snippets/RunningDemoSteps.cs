using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using LiveCoder.Api;
using LiveCoder.Common.Optional;
using LiveCoder.Snippets.Interfaces;
using LiveCoder.Snippets.Steps;

namespace LiveCoder.Snippets
{
    class RunningDemoSteps
    {
        private ISource ForFile { get; }
        private IDictionary<string, IDemoStep> SnippetShortcutToStep { get; }
        private CodeSnippets Script { get; }
        private ILogger Logger { get; }

        private IEnumerable<(Regex pattern, Func<StepSourceEntry, Option<RunningDemoSteps>> factory)> StepPatterns { get; }

        public RunningDemoSteps(ISource forFile, CodeSnippets script, ILogger logger)
        {
            this.ForFile = forFile ?? throw new ArgumentNullException(nameof(forFile));
            this.Script = script ?? throw new ArgumentNullException(nameof(script));
            this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));

            this.SnippetShortcutToStep = new Dictionary<string, IDemoStep>();

            this.StepPatterns = new (Regex, Func<StepSourceEntry, Option<RunningDemoSteps>>)[]
            {
                (new Regex(@"\/\/\s*(?<snippetShortcut>snp\d+) end"), EndSnippet),
                (new Regex(@"<!--\s*(?<snippetShortcut>snp\d+)\s+end\s*-->"), EndSnippet),
                (new Regex(@"\/\/\s*(?<snippetShortcut>snp\d+\.\d+)\s*(?<text>(.(?!\s*\/\/\s*snp))*)"), AddReminder),
                (new Regex(@"<!--\s*(?<snippetShortcut>snp\d+\.\d+)\s*(?<text>(.(?!-->))*)-->"), AddReminder),
                (new Regex(@"\/\/\s*(?<snippetShortcut>snp\d+)\s*(?<text>(.(?!\s*\/\/\s*snp))*)"), BeginSnippet),
                (new Regex(@"<!--\s*(?<snippetShortcut>snp\d+)\s*(?<text>(.(?!--!))*)-->"), BeginSnippet),
            };
        }

        private RunningDemoSteps(RunningDemoSteps copy, IDictionary<string, IDemoStep> steps)
        {
            this.ForFile = copy.ForFile;
            this.StepPatterns = copy.StepPatterns;
            this.SnippetShortcutToStep = steps;
        }

        public RunningDemoSteps Add(string line, int lineIndex) =>
            this.TryMatch(line, lineIndex)
                .MapOptional(tuple => tuple.factory(tuple.step))
                .Reduce(this);

        public IEnumerable<IDemoStep> All => this.SnippetShortcutToStep.Values;

        private Option<(StepSourceEntry step, Func<StepSourceEntry, Option<RunningDemoSteps>> factory)> TryMatch(string line, int lineIndex) =>
            this.StepPatterns
                .SelectMany(pair => this.LastMatch(pair.pattern, line).Select(match => (match, pair.factory)))
                .Select(pair => (
                    shortcut: pair.match.Groups["snippetShortcut"].Value, 
                    lineIndex: lineIndex, 
                    text: pair.match.Groups["text"] is Group text && text.Success ? text.Value : string.Empty,
                    code: line.Substring(0, pair.match.Index).TrimEnd(),
                    pair.factory))
                .Select(tuple => (new StepSourceEntry(tuple.shortcut, tuple.lineIndex, tuple.text, tuple.code), tuple.factory))
                .FirstOrNone();

        private IEnumerable<Match> LastMatch(Regex pattern, string line) =>
            pattern.Matches(line) is MatchCollection matches && matches.Count > 0 ? new[] {matches[matches.Count - 1]}
            : Enumerable.Empty<Match>();

        private Option<RunningDemoSteps> AddReminder(StepSourceEntry step) =>
            this.Add(new Reminder(this.Logger, this.ForFile, step));

        private Option<RunningDemoSteps> BeginSnippet(StepSourceEntry step) =>
            this.Script.TryGetSnippet(step.SnippetShortcut)
                .Map(snippet => this.Add(new SnippetReplace(this.Logger, snippet, this.ForFile, step.LineIndex, step.Description)));

        private Option<RunningDemoSteps> EndSnippet(StepSourceEntry step) =>
            this.SnippetShortcutToStep
                .TryGetValue(this.SnippetShortcutToNumber(step.SnippetShortcut))
                .OfType<SnippetReplace>()
                .Map(snippet => snippet.EndsOnLine(step.LineIndex))
                .Map(this.Add)
                .Reduce(this);

        private RunningDemoSteps Add(IDemoStep step)
        {
            this.SnippetShortcutToStep[this.SnippetShortcutToNumber(step.SnippetShortcut)] = step;
            return new RunningDemoSteps(this, this.SnippetShortcutToStep);
        }

        private string SnippetShortcutToNumber(string shortcut) =>
            this.RawSnippetNumber(shortcut).ToString("0.0##");

        private float RawSnippetNumber(string shortcut) =>
            float.Parse(Regex.Match(shortcut, @"(snp)?(?<number>\d+(.\d*)?)").Groups["number"].Value);
    }
}
