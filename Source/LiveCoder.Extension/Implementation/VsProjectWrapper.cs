using System;
using System.Collections.Generic;
using System.Linq;
using EnvDTE;
using LiveCoder.Extension.Interfaces;
using Microsoft.VisualStudio.Shell.Interop;

namespace LiveCoder.Extension.Implementation
{
    class VsProjectWrapper : IProject
    {
        private IVsProject Project { get; }
        private DTE Dte { get; }
        private IExpansionManager ExpansionManager { get; }
        private ILogger Logger { get; }

        public VsProjectWrapper(IVsProject project, DTE dte, IExpansionManager expansionManager, ILogger logger)
        {
            this.Project = project ?? throw new ArgumentNullException(nameof(project));
            this.Dte = dte ?? throw new ArgumentNullException(nameof(dte));
            this.ExpansionManager = expansionManager ?? throw new ArgumentNullException(nameof(expansionManager));
            this.Logger = logger;
        }

        public IEnumerable<ISource> SourceFiles =>
            this.Project.GetSourceFiles(this.Dte, this.ExpansionManager, this.Logger);

        public IEnumerable<IDemoStep> DemoSteps =>
            this.SourceFiles.SelectMany(file => file.DemoSteps);
    }
}
