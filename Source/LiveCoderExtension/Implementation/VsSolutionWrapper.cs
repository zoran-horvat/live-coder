using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Common.Optional;
using EnvDTE;
using LiveCoderExtension.Interfaces;
using Microsoft.VisualStudio.Shell.Interop;

namespace LiveCoderExtension.Implementation
{
    class VsSolutionWrapper : ISolution
    {
        private IVsSolution SolutionInterface { get; }
        private DTE Dte { get; }
        private IExpansionManager ExpansionManager { get; }

        public VsSolutionWrapper(IVsSolution solutionInterface, DTE dte, IExpansionManager expansionManager)
        {
            this.SolutionInterface = solutionInterface ?? throw new ArgumentNullException(nameof(SolutionInterface));
            this.Dte = dte ?? throw new ArgumentNullException(nameof(dte));
            this.ExpansionManager = expansionManager ?? throw new ArgumentNullException(nameof(expansionManager));
        }

        public Option<FileInfo> SolutionFile =>
            this.SolutionInterface.GetSolutionInfo(out string solutionDirectory, out string solutionFile, out string userOptionsFile) == 0
                ? solutionFile.FromNullable().Map(path => new FileInfo(path))
                : None.Value;

        public IEnumerable<IProject> Projects =>
            this.SolutionInterface.GetProjects().Select(project => new VsProjectWrapper(project, this.Dte, this.ExpansionManager));

        public IEnumerable<IDemoStep> DemoStepsOrdered =>
            this.Projects.SelectMany(project => project.DemoSteps).OrderBy(step => step.SnippetShortcut);
    }
}
