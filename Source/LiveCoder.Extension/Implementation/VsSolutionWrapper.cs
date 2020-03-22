using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EnvDTE;
using LiveCoder.Scripting.Interfaces;
using Microsoft.VisualStudio.Shell.Interop;

namespace LiveCoder.Extension.Implementation
{
    class VsSolutionWrapper : ISolution
    {
        private IVsSolution SolutionInterface { get; }
        private DTE Dte { get; }
        private ILogger Logger { get; }
        private FileInfo SolutionFile { get; }

        public DirectoryInfo LiveCoderDirectory =>
            new DirectoryInfo(Path.Combine(this.SolutionFile.DirectoryName ?? string.Empty, ".livecoder"));
        
        public VsSolutionWrapper(IVsSolution solutionInterface, DTE dte, ILogger logger)
        {
            this.SolutionInterface = solutionInterface ?? throw new ArgumentNullException(nameof(SolutionInterface));
            this.Dte = dte ?? throw new ArgumentNullException(nameof(dte));
            this.Logger = logger;
            
            this.SolutionInterface.GetSolutionInfo(out string _, out string solutionFile, out string _);
            this.SolutionFile = new FileInfo(solutionFile);
        }

        public IEnumerable<IProject> Projects =>
            this.SolutionInterface.GetProjects().Select(project => new VsProjectWrapper(project, this.Dte, this.Logger));
    }
}
