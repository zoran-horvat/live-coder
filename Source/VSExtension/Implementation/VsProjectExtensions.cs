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
        public static IEnumerable<ISource> GetSourceFiles(this IVsProject project, DTE dte) =>
            project.GetSourceFiles(dte, VSConstants.VSITEMID.Root);

        private static IEnumerable<ISource> GetSourceFiles(this IVsProject inProject, DTE dte,  VSConstants.VSITEMID startingWith) =>
            ((IVsHierarchy)inProject).GetChildrenIds(startingWith).SelectMany(id => inProject.FilesUnder(dte, id));

        private static Option<ISource> TryFindFile(this IVsProject project, DTE dte, VSConstants.VSITEMID itemId) =>
            project.FilePathFor(itemId)
                .When(path => !string.IsNullOrEmpty(path))
                .When(File.Exists)
                .Map(path => new FileInfo(path))
                .Map<ISource>(file => new SourceFile(file, dte.ReaderFor(file)));

        private static SourceReader ReaderFor(this DTE dte, FileInfo file) =>
            new OpenDocumentReader(dte, file, new FileReader(file));

        private static Option<string> FilePathFor(this IVsProject project, VSConstants.VSITEMID itemId) =>
            project.GetMkDocument((uint)itemId, out string path) == VSConstants.S_OK ? (Option<string>)path : None.Value;

        private static IEnumerable<ISource> FilesUnder(this IVsProject inProject, DTE dte, VSConstants.VSITEMID id) =>
            inProject.TryFindFile(dte, id)
                .Map<IEnumerable<ISource>>(file => new[] { file })
                .Reduce(() => inProject.GetSourceFiles(dte, id));
    }
}
