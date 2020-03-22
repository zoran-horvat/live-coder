using System;
using EnvDTE;
using LiveCoder.Common.Optional;
using LiveCoder.Scripting.Interfaces;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TextManager.Interop;

namespace LiveCoder.Extension.Implementation
{
    static class ServiceProviderExtensions
    {
        public static Option<VsSolutionWrapper> TryGetSolution(this IServiceProvider serviceProvider, ILogger logger) =>
            serviceProvider.TryGetSolutionInterface()
                .Map(solution => new VsSolutionWrapper(solution, serviceProvider.GetDte(), logger));

        private static Option<IVsSolution> TryGetSolutionInterface(this IServiceProvider serviceProvider) =>
            serviceProvider.GetRawSolutionInterface() is IVsSolution solution && IsSolutionOpen(solution)
                ? (Option<IVsSolution>) new Some<IVsSolution>(solution)
                : None.Value;

        private static bool IsSolutionOpen(IVsSolution solution) =>
            solution.GetProperty((int) __VSPROPID.VSPROPID_IsSolutionOpen, out object value) == 0 && (bool)value;

        private static IVsSolution GetRawSolutionInterface(this IServiceProvider serviceProvider) =>
            (IVsSolution)serviceProvider.GetService(typeof(IVsSolution));

        private static DTE GetDte(this IServiceProvider serviceProvider) =>
            (DTE)serviceProvider.GetService(typeof(DTE));

        private static Option<IVsTextManager2> GetTextManager() =>
            ((IVsTextManager2)Package.GetGlobalService(typeof(SVsTextManager))).FromNullable();

        private static Option<IVsExpansionManager> GetVsExpansionManager() =>
            GetTextManager().Map(GetExpansionManager);

        private static IVsExpansionManager GetExpansionManager(IVsTextManager2 textManager)
        {
            textManager.GetExpansionManager(out IVsExpansionManager expansionManager);
            return expansionManager;
        }

        private static Guid CSharpLanguageId =>
            new Guid("{694dd9b6-b865-4c5b-ad85-86356e9c88dc}");
    }
}
