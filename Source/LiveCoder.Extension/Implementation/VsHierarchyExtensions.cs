using System.Collections.Generic;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;

namespace LiveCoder.Extension.Implementation
{
    static class VsHierarchyExtensions
    {
        public static IEnumerable<VSConstants.VSITEMID> GetChildrenIds(this IVsHierarchy hierarchy, VSConstants.VSITEMID startingWith)
        {
            hierarchy.GetProperty((uint)startingWith, (int)__VSHPROPID.VSHPROPID_FirstChild, out object idObj);
            while (idObj != null)
            {
                if (idObj is System.Reflection.Missing)
                    break;
                
                VSConstants.VSITEMID id = (VSConstants.VSITEMID)(int)idObj;
                yield return id;

                hierarchy.GetProperty((uint)id, (int)__VSHPROPID.VSHPROPID_NextSibling, out idObj);
            }
        }
    }
}
