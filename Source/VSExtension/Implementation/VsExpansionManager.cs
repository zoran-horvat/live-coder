using Microsoft.VisualStudio.TextManager.Interop;
using System;

namespace VSExtension.Implementation
{
    class VsExpansionManager : IExpansionManager
    {
        private IVsExpansionManager Implementation { get; }

        public VsExpansionManager(IVsExpansionManager implementation)
        {
            this.Implementation = implementation ?? throw new ArgumentNullException(nameof(implementation));
        }
    }
}
