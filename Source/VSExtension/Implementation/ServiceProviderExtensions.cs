using VSExtension.Interfaces;
using System;
using Microsoft.VisualStudio.Shell.Interop;
using EnvDTE;

namespace VSExtension.Implementation
{
    static class ServiceProviderExtensions
    {
        public static ISolution GetSolution(this IServiceProvider serviceProvider) =>
            new VsSolutionWrapper(serviceProvider.GetSolutionInterface(), serviceProvider.GetDte());

        private static IVsSolution GetSolutionInterface(this IServiceProvider serviceProvider) =>
            (IVsSolution) serviceProvider.GetService(typeof(IVsSolution));

        private static DTE GetDte(this IServiceProvider serviceProvider) =>
            (DTE) serviceProvider.GetService(typeof(DTE));
    }
}
