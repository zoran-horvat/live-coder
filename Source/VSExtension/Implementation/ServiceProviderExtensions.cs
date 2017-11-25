using VSExtension.Interfaces;
using System;
using Microsoft.VisualStudio.Shell.Interop;

namespace VSExtension.Implementation
{
    static class ServiceProviderExtensions
    {
        public static ISolution GetSolution(this IServiceProvider serviceProvider) =>
            new VsSolutionWrapper((IVsSolution) serviceProvider.GetService(typeof(SVsSolution)));
    }
}
