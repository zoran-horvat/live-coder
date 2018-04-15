using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EnvDTE;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using VSExtension.Functional;
using VSExtension.Implementation.Readers;
using VSExtension.Interfaces;

namespace VSExtension.Implementation
{
    static class VsProjectExtensions
    {
        public static IEnumerable<ISource> GetSourceFiles(this IVsProject project, DTE dte, IExpansionManager expansionManager) =>
            project.GetSourceFiles(dte, VSConstants.VSITEMID.Root, expansionManager);

        public static void Open(this IVsProject project, VSConstants.VSITEMID itemId)
        {
            Guid view = Guid.Empty;
            project.OpenItem((uint)itemId, ref view, IntPtr.Zero, out IVsWindowFrame _);
        }

        private static IEnumerable<ISource> GetSourceFiles(this IVsProject inProject, DTE dte,  VSConstants.VSITEMID startingWith, IExpansionManager expansionManager) =>
            ((IVsHierarchy)inProject).GetChildrenIds(startingWith).SelectMany(id => inProject.FilesUnder(dte, id, expansionManager));

        private static Option<ISource> TryFindFile(this IVsProject project, DTE dte, VSConstants.VSITEMID itemId, IExpansionManager expansionManager) =>
            project.FilePathFor(itemId)
                .When(path => !string.IsNullOrEmpty(path))
                .When(File.Exists)
                .Map(path => new FileInfo(path))
                .Map<ISource>(file => new SourceFile(file, itemId, project, dte, expansionManager));

        private static Option<string> FilePathFor(this IVsProject project, VSConstants.VSITEMID itemId) =>
            project.GetMkDocument((uint)itemId, out string path) == VSConstants.S_OK ? (Option<string>)path : None.Value;

        private static IEnumerable<ISource> FilesUnder(this IVsProject inProject, DTE dte, VSConstants.VSITEMID id, IExpansionManager expansionManager) =>
            inProject.TryFindFile(dte, id, expansionManager)
                .Map<IEnumerable<ISource>>(file => new[] { file })
                .Reduce(() => inProject.GetSourceFiles(dte, id, expansionManager));
    }
}
