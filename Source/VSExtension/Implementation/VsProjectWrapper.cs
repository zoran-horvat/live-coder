using System;
using System.Collections.Generic;
using System.Linq;
using EnvDTE;
using Microsoft.VisualStudio.Shell.Interop;
using VSExtension.Interfaces;

namespace VSExtension.Implementation
{
    class VsProjectWrapper : IProject
    {
        private IVsProject Project { get; }
        private DTE Dte { get; }

        public VsProjectWrapper(IVsProject project, DTE dte)
        {
            this.Project = project ?? throw new ArgumentNullException(nameof(project));
            this.Dte = dte ?? throw new ArgumentNullException(nameof(dte));
        }

        public IEnumerable<ISource> SourceFiles =>
            this.Project.GetSourceFiles(this.Dte);

        public IEnumerable<IDemoStep> DemoSteps =>
            this.SourceFiles.SelectMany(file => file.DemoSteps);
    }
}
