using System;
using System.Collections.Generic;
using System.Linq;
using EnvDTE;
using LiveCoderExtension.Interfaces;
using Microsoft.VisualStudio.Shell.Interop;

namespace LiveCoderExtension.Implementation
{
    class VsProjectWrapper : IProject
    {
        private IVsProject Project { get; }
        private DTE Dte { get; }
        private IExpansionManager ExpansionManager { get; }

        public VsProjectWrapper(IVsProject project, DTE dte, IExpansionManager expansionManager)
        {
            this.Project = project ?? throw new ArgumentNullException(nameof(project));
            this.Dte = dte ?? throw new ArgumentNullException(nameof(dte));
            this.ExpansionManager = expansionManager ?? throw new ArgumentNullException(nameof(expansionManager));
        }

        public IEnumerable<ISource> SourceFiles =>
            this.Project.GetSourceFiles(this.Dte, this.ExpansionManager);

        public IEnumerable<IDemoStep> DemoSteps =>
            this.SourceFiles.SelectMany(file => file.DemoSteps);
    }
}
