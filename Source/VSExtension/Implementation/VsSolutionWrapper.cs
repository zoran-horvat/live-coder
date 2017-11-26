using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.Shell.Interop;
using VSExtension.Interfaces;

namespace VSExtension.Implementation
{
    class VsSolutionWrapper : ISolution
    {
        private IVsSolution SolutionInterface { get; }

        public VsSolutionWrapper(IVsSolution solutionInterface)
        {
            SolutionInterface = solutionInterface ?? throw new ArgumentNullException(nameof(SolutionInterface));
        }

        public IEnumerable<IProject> Projects =>
            this.SolutionInterface.GetProjects().Select(project => new VsProjectWrapper(project));

        public IEnumerable<IDemoStep> DemoSteps =>
            this.Projects.SelectMany(project => project.DemoSteps);
    }
}
