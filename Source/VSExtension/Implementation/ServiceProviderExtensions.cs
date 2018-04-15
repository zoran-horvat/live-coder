using VSExtension.Interfaces;
using System;
using Microsoft.VisualStudio.Shell.Interop;
using EnvDTE;
using Microsoft.VisualStudio.TextManager.Interop;
using VSExtension.Functional;

namespace VSExtension.Implementation
{
    static class ServiceProviderExtensions
    {
        public static ISolution GetSolution(this IServiceProvider serviceProvider) =>
            new VsSolutionWrapper(serviceProvider.GetSolutionInterface(), serviceProvider.GetDte(), serviceProvider.GetExpansionManager());

        private static IVsSolution GetSolutionInterface(this IServiceProvider serviceProvider) =>
            (IVsSolution) serviceProvider.GetService(typeof(IVsSolution));

        private static DTE GetDte(this IServiceProvider serviceProvider) =>
            (DTE)serviceProvider.GetService(typeof(DTE));

        private static IExpansionManager GetExpansionManager(this IServiceProvider serviceProvider)
        {
            if (serviceProvider.GetService(typeof(IVsTextManager)) is IVsTextManager2 textManager)
            {
                textManager.GetExpansionManager(out IVsExpansionManager expansionManager);
                return new VsExpansionManager(expansionManager);
            }
            return new NoExpansionManager();
        }
    }
}
