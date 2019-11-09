using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using LiveCoder.Common.Optional;
using LiveCoder.Extension.Interfaces;
using Microsoft.VisualStudio.TextManager.Interop;

namespace LiveCoder.Extension.Implementation
{
    class VisualStudioExpansionManager : IExpansionManager
    {
        private IVsExpansionManager Implementation { get; }
        private Guid LanguageId { get; }

        public VisualStudioExpansionManager(IVsExpansionManager implementation, Guid languageId)
        {
            this.Implementation = implementation ?? throw new ArgumentNullException(nameof(implementation));
            this.LanguageId = languageId;
        }

        public Option<ISnippet> FindSnippet(string shortcut) =>
            this.GetExpansions()
                .FirstOrNone(expansion => string.CompareOrdinal(expansion.shortcut, shortcut) == 0)
                .Map<ISnippet>(expansion => new FileSystemSnippet(new FileInfo(expansion.path)));

        private IEnumerable<VsExpansion> GetExpansions()
        {
            int res = this.Implementation.EnumerateExpansions(this.LanguageId, 0, null, 0, 0, 1, out IVsExpansionEnumeration expansions);

            uint count = 0;
            uint fetched = 0;
            VsExpansion expansionInfo = new VsExpansion();
            IntPtr[] pExpansionInfo = new IntPtr[1];
            // Allocate enough memory for one VSExpansion structure.  
            // This memory is filled in by the Next method.  
            pExpansionInfo[0] = Marshal.AllocCoTaskMem(Marshal.SizeOf(expansionInfo));

            expansions.GetCount(out count);
            for (uint i = 0; i < count; i++)
            {
                expansions.Next(1, pExpansionInfo, out fetched);
                if (fetched > 0)
                {
                    // Convert the returned blob of data into a  
                    // structure that can be read in managed code.  
                    expansionInfo = (VsExpansion)
                        Marshal.PtrToStructure(pExpansionInfo[0],
                            typeof(VsExpansion));

                    if (!String.IsNullOrEmpty(expansionInfo.shortcut))
                    {
                        yield return expansionInfo;
                    }
                }
            }
            Marshal.FreeCoTaskMem(pExpansionInfo[0]);
        }
    }
}
