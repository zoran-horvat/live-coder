using System;
using System.IO;
using Microsoft.VisualStudio;
using VSExtension.Interfaces;

namespace VSExtension.Implementation
{
    class SourceFile : ISource
    {
        public string Name => this.File.Name;

        private FileInfo File { get; }
        private VSConstants.VSITEMID ItemId { get; }

        public SourceFile(VSConstants.VSITEMID itemId, FileInfo file)
        {
            this.ItemId = itemId;
            this.File = file ?? throw new ArgumentNullException(nameof(file));
        }
    }
}
