using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio;
using VSExtension.Interfaces;

namespace VSExtension.Implementation
{
    class SourceFile : ISource
    {
        public string Name => this.File.Name;

        public IEnumerable<IDemoStep> DemoSteps =>
            this.Lines.Aggregate(new RunningDemoSteps(this), (steps, tuple) => steps.Add(tuple.line, tuple.index)).All;

        private FileInfo File { get; }
        private VSConstants.VSITEMID ItemId { get; }

        public SourceFile(VSConstants.VSITEMID itemId, FileInfo file)
        {
            this.ItemId = itemId;
            this.File = file ?? throw new ArgumentNullException(nameof(file));
        }

        private IEnumerable<(string line, int index)> Lines =>
            System.IO.File.ReadAllLines(this.File.FullName).Select((line, index) => (line, index));
    }
}
