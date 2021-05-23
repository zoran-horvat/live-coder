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
        private IDictionary<int, IDemoStep> SnippetShortcutToStep { get; }
        private CodeSnippets Script { get; }
        private ILogger Logger { get; }

        private IEnumerable<(Regex pattern, Func<string, int, string, Option<RunningDemoSteps>> factory)> StepPatterns { get; }

        public RunningDemoSteps(ISource forFile, CodeSnippets script, ILogger logger)
        {
            this.ForFile = forFile ?? throw new ArgumentNullException(nameof(forFile));
            this.Script = script ?? throw new ArgumentNullException(nameof(script));
            this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));

            this.SnippetShortcutToStep = new Dictionary<int, IDemoStep>();

            this.StepPatterns = new (Regex, Func<string, int, string, Option<RunningDemoSteps>>)[]
            {
                (new Regex(@"(\/\/|<!--)\s+(Snippet )?(?<snippetShortcut>snp\d+\.\d+)\s*(?<text>.*)"), AddReminder),
                (new Regex(@"(\/\/|<!--)\s+(?<snippetShortcut>snp\d+) end"), EndSnippet),
                (new Regex(@"(\/\/|<!--)\s+(Snippet )?(?<snippetShortcut>snp\d+)\s*(?<text>.*)"), BeginSnippet),
            };
        }

        private RunningDemoSteps(RunningDemoSteps copy, IDictionary<int, IDemoStep> steps)
        {
            this.ForFile = copy.ForFile;
            this.StepPatterns = copy.StepPatterns;
            this.SnippetShortcutToStep = steps;
        }

        public RunningDemoSteps Add(string line, int lineIndex) =>
            this.TryMatch(line, lineIndex)
                .MapOptional(tuple => tuple.factory(tuple.snippetShortcut, tuple.lineIndex, tuple.text))
                .Reduce(this);

        public IEnumerable<IDemoStep> All => this.SnippetShortcutToStep.Values;

        private Option<(string snippetShortcut, int lineIndex, string text, Func<string, int, string, Option<RunningDemoSteps>> factory)> TryMatch(string line, int lineIndex) =>
            this.StepPatterns
                .Select(pair => (match: pair.pattern.Match(line), pair.factory))
                .Where(pair => pair.match.Success)
                .Select(pair => (
                    shortcut: pair.match.Groups["snippetShortcut"].Value, 
                    lineIndex: lineIndex, 
                    text: pair.match.Groups["text"] is Group text && text.Success ? text.Value : string.Empty,
                    pair.factory))
                .FirstOrNone();

        private Option<RunningDemoSteps> AddReminder(string snippetShortcut, int index, string text) =>
            this.Add(new Reminder(this.Logger, snippetShortcut, this.ForFile, index, text));

        private Option<RunningDemoSteps> BeginSnippet(string snippetShortcut, int index, string text) =>
            this.Script.TryGetSnippet(snippetShortcut)
                .Map(snippet => this.Add(new SnippetReplace(this.Logger, snippet, this.ForFile, index, text)));

        private Option<RunningDemoSteps> EndSnippet(string snippetShortcut, int index, string text) =>
            this.SnippetShortcutToStep
                .TryGetValue(this.SnippetShortcutToNumber(snippetShortcut))
                .OfType<SnippetReplace>()
                .Map(snippet => snippet.EndsOnLine(index))
                .Map(this.Add)
                .Reduce(this);

        private RunningDemoSteps Add(IDemoStep step)
        {
            this.SnippetShortcutToStep[this.SnippetShortcutToNumber(step.SnippetShortcut)] = step;
            return new RunningDemoSteps(this, this.SnippetShortcutToStep);
        }

        private int SnippetShortcutToNumber(string shortcut) => 
            int.Parse(Regex.Match(shortcut, @"(snp)?(?<number>\d+)").Groups["number"].Value);
    }
}
