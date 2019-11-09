using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using LiveCoder.Common.Optional;
using LiveCoder.Extension.Implementation.Steps;
using LiveCoder.Extension.Interfaces;

namespace LiveCoder.Extension.Implementation
{
    class RunningDemoSteps
    {
        private ISource ForFile { get; }
        private IDictionary<string, IDemoStep> SnippetShortcutToStep { get; }

        public RunningDemoSteps(ISource forFile)
        {
            this.ForFile = forFile ?? throw new ArgumentNullException(nameof(forFile));
            this.SnippetShortcutToStep = new Dictionary<string, IDemoStep>();

            this.StepPatterns =  new (Regex, Func<string, int, RunningDemoSteps>)[]
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

        public RunningDemoSteps Add(string line, int index) =>
            this.TryMatch(line, index)
                .Map(tuple => tuple.factory(tuple.snippetShortcut, tuple.index))
                .Reduce(this);

        public IEnumerable<IDemoStep> All => this.SnippetShortcutToStep.Values;

        private Option<(string snippetShortcut, int index, Func<string, int, RunningDemoSteps> factory)> TryMatch(string line, int index) =>
            this.StepPatterns
                .Select(pattern => (pattern.Item1.Match(line), pattern.Item2))
                .Where(tuple => tuple.Item1.Success)
                .Select(tuple => (tuple.Item1.Groups["snippetShortcut"].Value, index, tuple.Item2))
                .FirstOrNone();


        private IEnumerable<(Regex, Func<string, int, RunningDemoSteps>)> StepPatterns { get; }

        private RunningDemoSteps AddReminder(string snippetShortcut, int index) =>
            this.Add(new Reminder(snippetShortcut, this.ForFile, index));

        private RunningDemoSteps BeginSnippet(string snippetShortcut, int index) =>
            this.Add(new SnippetReplace(snippetShortcut, this.ForFile, index));

        private RunningDemoSteps EndSnippet(string snippetShortcut, int index) =>
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
