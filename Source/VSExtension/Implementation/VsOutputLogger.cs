using System;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using VSExtension.Interfaces;

namespace VSExtension.Implementation
{
    class VsOutputLogger : ILogger
    {
        private static Guid OutputPaneGuid = new Guid("{60D32F8F-27C4-447D-A157-ED043CD08453}");
        private static string OutputPaneTitle { get; } = "PreZenter";

        private IVsOutputWindow GetOutputWindow() 
            => Package.GetGlobalService(typeof(SVsOutputWindow)) as IVsOutputWindow;

        private IVsOutputWindowPane GetOutputWindowPane()
        {
            IVsOutputWindow outputWindow = this.GetOutputWindow();
            outputWindow.CreatePane(ref OutputPaneGuid, OutputPaneTitle, 1, 1);
            outputWindow.GetPane(ref OutputPaneGuid, out IVsOutputWindowPane customPane);
            return customPane;
        }

        public void Write(IEvent @event)
        {
            this.GetOutputWindowPane().OutputString("--------------" + Environment.NewLine + @event.Label + Environment.NewLine);
        }
    }
}
