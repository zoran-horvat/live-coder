using LiveCoder.Api;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace LiveCoder.Extension.Implementation
{
    class VsStatusBarLogger : ILogger
    {
        private IVsStatusbar StatusBar { get; }

        public VsStatusBarLogger()
        {
            this.StatusBar = Package.GetGlobalService(typeof(SVsStatusbar)) as IVsStatusbar;
        }

        public void Write(IEvent @event) => 
            this.StatusBar?.SetText(@event.Label);
    }
}
