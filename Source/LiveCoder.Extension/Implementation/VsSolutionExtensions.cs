using System;
using System.Collections.Generic;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;

namespace LiveCoder.Extension.Implementation
{
    static class VsSolutionExtensions
    {
        public static IEnumerable<IVsProject> GetProjects(this IVsSolution solution)
        {
            if (solution.GetProjectEnum((uint)__VSENUMPROJFLAGS.EPF_LOADEDINSOLUTION, Guid.Empty, out IEnumHierarchies projects) != VSConstants.S_OK)
                yield break;

            IVsHierarchy[] projectArray = new IVsHierarchy[1];

            while (projects.Next((uint)projectArray.Length, projectArray, out uint fetched) == VSConstants.S_OK && fetched > 0)
            {
                yield return (IVsProject)projectArray[0];
            }
        }
    }
}
