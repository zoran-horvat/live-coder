using System;
using System.Collections.Generic;
using EnvDTE;
using LiveCoder.Scripting.Interfaces;
using Microsoft.VisualStudio.Shell.Interop;

namespace LiveCoder.Extension.Implementation
{
    class VsProjectWrapper : IProject
    {
        private IVsProject Project { get; }
        private DTE Dte { get; }
        private ILogger Logger { get; }

        public VsProjectWrapper(IVsProject project, DTE dte, ILogger logger)
        {
            this.Project = project ?? throw new ArgumentNullException(nameof(project));
            this.Dte = dte ?? throw new ArgumentNullException(nameof(dte));
            this.Logger = logger;
        }

        public IEnumerable<ISource> SourceFiles =>
            this.Project.GetSourceFiles(this.Dte);
    }
}
