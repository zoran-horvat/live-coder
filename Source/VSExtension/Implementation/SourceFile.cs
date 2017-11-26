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
        private SourceReader Reader { get; }

        public IEnumerable<IDemoStep> DemoSteps =>
            this.Lines.Aggregate(new RunningDemoSteps(this), (steps, tuple) => steps.Add(tuple.line, tuple.index)).All;

        private FileInfo File { get; }

        public SourceFile(FileInfo file, SourceReader reader)
        {
            this.File = file ?? throw new ArgumentNullException(nameof(file));
            this.Reader = reader ?? throw new ArgumentNullException(nameof(reader));
        }

        private IEnumerable<(string line, int index)> Lines => this.Reader.ReadAllLines();
    }
}
