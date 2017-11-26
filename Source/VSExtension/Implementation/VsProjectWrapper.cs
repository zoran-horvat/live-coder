using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.Shell.Interop;
using VSExtension.Interfaces;

namespace VSExtension.Implementation
{
    class VsProjectWrapper : IProject
    {
        private IVsProject Project { get; }

        public VsProjectWrapper(IVsProject project)
        {
            this.Project = project;
        }

        public IEnumerable<ISource> SourceFiles =>
            this.Project.GetSourceFiles();

        public IEnumerable<IDemoStep> DemoSteps =>
            this.SourceFiles.SelectMany(file => file.DemoSteps);
    }
}
