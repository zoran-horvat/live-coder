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
                (new Regex(@"\/\/\s*(?<snippetShortcut>snp\d+)\s+end"), EndSnippet),
                (new Regex(@"<!--\s*(?<snippetShortcut>snp\d+)\s+end\s*-->"), EndSnippet),
                (new Regex(@"\/\/\s*(?<snippetShortcut>snp\d+\.\d+)\s*(?<text>(.(?!\/\/\s*snp))*)"), AddReminder),
                (new Regex(@"<!--\s*(?<snippetShortcut>snp\d+\.\d+)\s*(?<text>(.(?!-->))*)\s*-->"), AddReminder),
                (new Regex(@"\/\/\s*(?<snippetShortcut>snp\d+)\s+(?<text>(.(?!\/\/\s*snp))*)"), BeginSnippet),
                (new Regex(@"<!--\s*(?<snippetShortcut>snp\d+)\s+(?<text>(.(?!-->))*)\s*-->"), BeginSnippet),
            };
        }

        private RunningDemoSteps(RunningDemoSteps copy, IDictionary<string, IDemoStep> steps)
        {
            this.ForFile = copy.ForFile;
            this.StepPatterns = copy.StepPatterns;
            this.SnippetShortcutToStep = steps;
        }

        public RunningDemoSteps Add(string line, int lineIndex) =>
            this.GetMatches(line, lineIndex).Aggregate(this, (steps, tuple) => tuple.factory(tuple.step).Reduce(this));

        public IEnumerable<IDemoStep> All => this.SnippetShortcutToStep.Values;

        private IEnumerable<(StepSourceEntry step, Func<StepSourceEntry, Option<RunningDemoSteps>> factory)> GetMatches(string line, int lineIndex)
        {
            int offset = 0;
            while (offset < line.Length && 
                   this.TryFindFirstMatch(line, offset) is Some<(Match match, Func<StepSourceEntry, Option<RunningDemoSteps>> factory)> some)
            {
                (Match match, Func<StepSourceEntry, Option<RunningDemoSteps>> factory) = some.Content;
                string shortcut = match.Groups["snippetShortcut"].Value;
                string text = match.Groups["text"].Value.Trim();
                string code = line.Substring(0, match.Index).TrimEnd();
                StepSourceEntry entry = new StepSourceEntry(shortcut, lineIndex, text, code);
                yield return (entry, factory);

                offset = match.Index + match.Length;
            }
        }

        private Option<(Match match, Func<StepSourceEntry, Option<RunningDemoSteps>> factory)> TryFindFirstMatch(string line, int offset) =>
            this.StepPatterns
                .Select(pair => (match: pair.pattern.Match(line, offset), factory: pair.factory))
                .Where(pair => pair.match.Success)
                .WithMinOrNone(pair => pair.match.Index);

        private IEnumerable<Match> Matches(Regex pattern, string line)
        {
            foreach (Match match in pattern.Matches(line))
                if (match.Success)
                    yield return match;
        }

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
