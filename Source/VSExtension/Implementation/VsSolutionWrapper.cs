using System;
using System.Collections.Generic;
using System.Linq;
using EnvDTE;
using Microsoft.VisualStudio.Shell.Interop;
using VSExtension.Interfaces;

namespace VSExtension.Implementation
{
    class VsSolutionWrapper : ISolution
    {
        private IVsSolution SolutionInterface { get; }
        private DTE Dte { get; }

        public VsSolutionWrapper(IVsSolution solutionInterface, DTE dte)
        {
            this.SolutionInterface = solutionInterface ?? throw new ArgumentNullException(nameof(SolutionInterface));
            this.Dte = dte ?? throw new ArgumentNullException(nameof(dte));
        }

        public IEnumerable<IProject> Projects =>
            this.SolutionInterface.GetProjects().Select(project => new VsProjectWrapper(project, this.Dte));

        public IEnumerable<IDemoStep> DemoStepsOrdered =>
            this.Projects.SelectMany(project => project.DemoSteps).OrderBy(step => step.SortKey);
    }
}
