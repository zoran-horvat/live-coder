using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EnvDTE;
using LiveCoder.Common.Optional;
using LiveCoder.Scripting;
using LiveCoder.Scripting.Execution;
using LiveCoder.Scripting.Interfaces;
using Microsoft.VisualStudio.Shell.Interop;

namespace LiveCoder.Extension.Implementation
{
    class VsSolutionWrapper : ISolution
    {
        private IVsSolution SolutionInterface { get; }
        private DTE Dte { get; }
        private ILogger Logger { get; }

        public VsSolutionWrapper(IVsSolution solutionInterface, DTE dte, ILogger logger)
        {
            this.SolutionInterface = solutionInterface ?? throw new ArgumentNullException(nameof(SolutionInterface));
            this.Dte = dte ?? throw new ArgumentNullException(nameof(dte));
            this.Logger = logger;
            Option<FileInfo> solutionFile = this.File;
        }

        public Option<FileInfo> File =>
            this.SolutionInterface.GetSolutionInfo(out string solutionDirectory, out string solutionFile, out string userOptionsFile) == 0
                ? solutionFile.FromNullable().Map(path => new FileInfo(path))
                : None.Value;

        public IEnumerable<IProject> Projects =>
            this.SolutionInterface.GetProjects().Select(project => new VsProjectWrapper(project, this.Dte, this.Logger));

        public IEnumerable<IDemoStep> GetDemoStepsOrdered(DemoScript script) =>
            this.Projects.SelectMany(project => project.GetDemoSteps(script)).OrderBy(step => step.SnippetShortcut);
    }
}
