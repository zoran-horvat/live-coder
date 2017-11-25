using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using VSExtension.Functional;
using VSExtension.Interfaces;

namespace VSExtension.Implementation
{
    static class VsProjectExtensions
    {
        public static IEnumerable<ISource> GetSourceFiles(this IVsProject project) =>
            project.GetSourceFiles(VSConstants.VSITEMID.Root);

        private static IEnumerable<ISource> GetSourceFiles(this IVsProject inProject, VSConstants.VSITEMID startingWith) =>
            ((IVsHierarchy)inProject).GetChildrenIds(startingWith).SelectMany(inProject.FilesUnder);

        private static Option<ISource> TryFindFile(this IVsProject project, VSConstants.VSITEMID itemId) =>
            project.FilePathFor(itemId)
                .When(path => !string.IsNullOrEmpty(path))
                .When(File.Exists)
                .Map(path => new FileInfo(path))
                .Map<ISource>(file => new SourceFile(itemId, file));

        private static Option<string> FilePathFor(this IVsProject project, VSConstants.VSITEMID itemId) =>
            project.GetMkDocument((uint)itemId, out string path) == VSConstants.S_OK ? (Option<string>)path : None.Value;

        private static IEnumerable<ISource> FilesUnder(this IVsProject inProject, VSConstants.VSITEMID id) =>
            inProject.TryFindFile(id)
                .Map<IEnumerable<ISource>>(file => new[] { file })
                .Reduce(() => GetSourceFiles(inProject, id));
    }
}
