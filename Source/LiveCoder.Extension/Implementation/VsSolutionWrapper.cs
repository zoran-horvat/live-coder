using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EnvDTE;
using LiveCoder.Common.Optional;
using LiveCoder.Extension.Interfaces;
using Microsoft.VisualStudio.Shell.Interop;

namespace LiveCoder.Extension.Implementation
{
    class VsSolutionWrapper : ISolution
    {
        private IVsSolution SolutionInterface { get; }
        private DTE Dte { get; }
        private IExpansionManager ExpansionManager { get; }
        private ILogger Logger { get; }

        public VsSolutionWrapper(IVsSolution solutionInterface, DTE dte, IExpansionManager expansionManager, ILogger logger)
        {
            this.SolutionInterface = solutionInterface ?? throw new ArgumentNullException(nameof(SolutionInterface));
            this.Dte = dte ?? throw new ArgumentNullException(nameof(dte));
            this.ExpansionManager = expansionManager ?? throw new ArgumentNullException(nameof(expansionManager));
            this.Logger = logger;
        }

        public Option<FileInfo> SolutionFile =>
            this.SolutionInterface.GetSolutionInfo(out string solutionDirectory, out string solutionFile, out string userOptionsFile) == 0
                ? solutionFile.FromNullable().Map(path => new FileInfo(path))
                : None.Value;

        public IEnumerable<IProject> Projects =>
            this.SolutionInterface.GetProjects().Select(project => new VsProjectWrapper(project, this.Dte, this.ExpansionManager, this.Logger));

        public IEnumerable<IDemoStep> DemoStepsOrdered =>
            this.Projects.SelectMany(project => project.DemoSteps).OrderBy(step => step.SnippetShortcut);
    }
}
