using Microsoft.VisualStudio.TextManager.Interop;
using System;
using VSExtension.Interfaces;

namespace VSExtension.Implementation
{
    class VisualStudioExpansionManager : IExpansionManager
    {
        private IVsExpansionManager Implementation { get; }

        public VisualStudioExpansionManager(IVsExpansionManager implementation)
        {
            this.Implementation = implementation ?? throw new ArgumentNullException(nameof(implementation));
        }
    }
}
