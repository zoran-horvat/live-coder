using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using LiveCoder.Common.Optional;
using LiveCoder.Scripting.Interfaces;
using LiveCoder.Scripting.Snippets.Steps;

namespace LiveCoder.Scripting.Snippets
{
    public class RunningDemoSteps
    {
        private ISource ForFile { get; }
        private IDictionary<string, IDemoStep> SnippetShortcutToStep { get; }
        private CodeSnippets Script { get; }

        private IEnumerable<(Regex pattern, Func<string, int, Option<RunningDemoSteps>> factory)> StepPatterns { get; }

        public RunningDemoSteps(ISource forFile, CodeSnippets script)
        {
            this.ForFile = forFile ?? throw new ArgumentNullException(nameof(forFile));
            this.Script = script ?? throw new ArgumentNullException(nameof(script));

            this.SnippetShortcutToStep = new Dictionary<string, IDemoStep>();

            this.StepPatterns =  new (Regex, Func<string, int, Option<RunningDemoSteps>>)[]
            {
                (new Regex(@"\/\/ (Snippet )?(?<snippetShortcut>snp\d+\.\d+)"), AddReminder),
                (new Regex(@"\/\/ (?<snippetShortcut>snp\d+) end"), EndSnippet),
                (new Regex(@"\/\/ (Snippet )?(?<snippetShortcut>snp\d+)"), BeginSnippet)
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
                .MapOptional(tuple => tuple.factory(tuple.snippetShortcut, tuple.lineIndex))
                .Reduce(this);

        public IEnumerable<IDemoStep> All => this.SnippetShortcutToStep.Values;

        private Option<(string snippetShortcut, int lineIndex, Func<string, int, Option<RunningDemoSteps>> factory)> TryMatch(string line, int lineIndex) =>
            this.StepPatterns
                .Select(pair => (match: pair.pattern.Match(line), pair.factory))
                .Where(pair => pair.match.Success)
                .Select(pair => (shortcut: pair.match.Groups["snippetShortcut"].Value, lineIndex: lineIndex, pair.factory))
                .FirstOrNone();

        private Option<RunningDemoSteps> AddReminder(string snippetShortcut, int index) =>
            this.Add(new Reminder(snippetShortcut, this.ForFile, index));

        private Option<RunningDemoSteps> BeginSnippet(string snippetShortcut, int index) =>
            this.Script.TryGetSnippet(snippetShortcut)
                .Map(snippet => this.Add(new SnippetReplace(snippet, this.ForFile, index)));

        private Option<RunningDemoSteps> EndSnippet(string snippetShortcut, int index) =>
            this.SnippetShortcutToStep
                .TryGetValue(snippetShortcut)
                .OfType<SnippetReplace>()
                .Map(snippet => snippet.EndsOnLine(index))
                .Map(this.Add)
                .Reduce(this);

        private RunningDemoSteps Add(IDemoStep step)
        {
            this.SnippetShortcutToStep[step.SnippetShortcut] = step;
            return new RunningDemoSteps(this, this.SnippetShortcutToStep);
        }
    }
}
