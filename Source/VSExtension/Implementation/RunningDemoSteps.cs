using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using VSExtension.Functional;
using VSExtension.Interfaces;

namespace VSExtension.Implementation
{
    class RunningDemoSteps
    {
        private ISource ForFile { get; }
        private IDictionary<string, IDemoStep> SortKeyToStep { get; }

        public RunningDemoSteps(ISource forFile)
        {
            this.ForFile = forFile ?? throw new ArgumentNullException(nameof(forFile));
            this.SortKeyToStep = new Dictionary<string, IDemoStep>();

            this.StepPatterns =  new (Regex, Func<string, int, RunningDemoSteps>)[]
            {
                (new Regex(@"\/\/ Snippet snp(?<sortKey>\d+\.\d+)"), AddReminder),
                (new Regex(@"\/\/ snp(?<sortKey>\d+) end"), EndSnippet),
                (new Regex(@"\/\/ Snippet snp(?<sortKey>\d+)"), BeginSnippet)
            };
        }

        private RunningDemoSteps(RunningDemoSteps copy, IDictionary<string, IDemoStep> steps)
        {
            this.ForFile = copy.ForFile;
            this.StepPatterns = copy.StepPatterns;
            this.SortKeyToStep = steps;
        }

        public RunningDemoSteps Add(string line, int index) =>
            this.TryMatch(line, index)
                .Map(tuple => tuple.factory(tuple.sortKey, tuple.index))
                .Reduce(this);

        public IEnumerable<IDemoStep> All => this.SortKeyToStep.Values;

        private Option<(string sortKey, int index, Func<string, int, RunningDemoSteps> factory)> TryMatch(string line, int index) =>
            this.StepPatterns
                .Select(pattern => (pattern.Item1.Match(line), pattern.Item2))
                .Where(tuple => tuple.Item1.Success)
                .Select(tuple => (tuple.Item1.Groups["sortKey"].Value, index, tuple.Item2))
                .FirstOrNone();


        private IEnumerable<(Regex, Func<string, int, RunningDemoSteps>)> StepPatterns { get; }

        private RunningDemoSteps AddReminder(string sortKey, int index) =>
            this.Add(new Reminder(sortKey, this.ForFile, index));

        private RunningDemoSteps BeginSnippet(string sortKey, int index) =>
            this.Add(new SnippetReplace(sortKey, this.ForFile, index));

        private RunningDemoSteps EndSnippet(string sortKey, int index) =>
            this.SortKeyToStep
                .TryGetValue(sortKey)
                .OfType<SnippetReplace>()
                .Map(snippet => snippet.EndsOnLine(index))
                .Map(this.Add).Reduce(this);

        private RunningDemoSteps Add(IDemoStep step)
        {
            this.SortKeyToStep[step.SortKey] = step;
            return new RunningDemoSteps(this, this.SortKeyToStep);
        }
    }
}
