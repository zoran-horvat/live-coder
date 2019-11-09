using System;
using EnvDTE;
using LiveCoder.Common.Optional;
using LiveCoder.Extension.Interfaces;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TextManager.Interop;

namespace LiveCoder.Extension.Implementation
{
    static class ServiceProviderExtensions
    {
        public static ISolution GetSolution(this IServiceProvider serviceProvider, ILogger logger) =>
            new VsSolutionWrapper(serviceProvider.GetSolutionInterface(), serviceProvider.GetDte(), GetExpansionManager(), logger);

        private static IVsSolution GetSolutionInterface(this IServiceProvider serviceProvider) =>
            (IVsSolution)serviceProvider.GetService(typeof(IVsSolution));

        private static DTE GetDte(this IServiceProvider serviceProvider) =>
            (DTE)serviceProvider.GetService(typeof(DTE));

        private static Option<IVsTextManager2> GetTextManager() =>
            ((IVsTextManager2)Package.GetGlobalService(typeof(SVsTextManager))).FromNullable();

        private static Option<IVsExpansionManager> GetVsExpansionManager() =>
            GetTextManager().Map(GetExpansionManager);

        private static IExpansionManager GetExpansionManager() =>
            GetVsExpansionManager()
                .Map(vsImplementation => (IExpansionManager)new VisualStudioExpansionManager(vsImplementation, CSharpLanguageId))
                .Reduce(() => new NoExpansionManager());

        private static IVsExpansionManager GetExpansionManager(IVsTextManager2 textManager)
        {
            textManager.GetExpansionManager(out IVsExpansionManager expansionManager);
            return expansionManager;
        }

        private static Guid CSharpLanguageId =>
            new Guid("{694dd9b6-b865-4c5b-ad85-86356e9c88dc}");
    }
}
