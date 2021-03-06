﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EnvDTE;
using LiveCoder.Api;
using LiveCoder.Common.Optional;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;

namespace LiveCoder.Extension.Implementation
{
    static class VsProjectExtensions
    {
        public static IEnumerable<ISource> GetSourceFiles(this IVsProject project, DTE dte) =>
            project.GetSourceFiles(dte, VSConstants.VSITEMID.Root);

        public static void Open(this IVsProject project, VSConstants.VSITEMID itemId)
        {
            Guid view = Guid.Empty;
            project.OpenItem((uint)itemId, ref view, IntPtr.Zero, out IVsWindowFrame _);
        }

        private static IEnumerable<ISource> GetSourceFiles(this IVsProject inProject, DTE dte,  VSConstants.VSITEMID startingWith) =>
            ((IVsHierarchy)inProject).GetChildrenIds(startingWith).SelectMany(id => inProject.FilesUnder(dte, id));

        private static Option<ISource> TryFindFile(this IVsProject project, DTE dte, VSConstants.VSITEMID itemId) =>
            project.FilePathFor(itemId)
                .When(path => !string.IsNullOrEmpty(path))
                .When(File.Exists)
                .Map(path => new FileInfo(path))
                .Map<ISource>(file => new SourceFile(file, itemId, project, dte));

        private static Option<string> FilePathFor(this IVsProject project, VSConstants.VSITEMID itemId) =>
            project.GetMkDocument((uint)itemId, out string path) == VSConstants.S_OK ? (Option<string>)path : None.Value;

        private static IEnumerable<ISource> FilesUnder(this IVsProject inProject, DTE dte, VSConstants.VSITEMID id)
        {
            Option<ISource> file = inProject.TryFindFile(dte, id);

            IEnumerable<ISource> result = 
                file.OfType<SourceFile>() is Some<SourceFile> some ? new [] {some.Content}.Concat(inProject.GetSourceFiles(some.Content.Dte, some.Content.ItemId))
                : file is Some<ISource> general ? new[] {general.Content}
                : inProject.GetSourceFiles(dte, id);

            return result;
        }
    }
}
