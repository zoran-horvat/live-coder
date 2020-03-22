using System;
using System.ComponentModel.Design;
using LiveCoder.Common.Optional;
using LiveCoder.Extension.Implementation;
using LiveCoder.Scripting.Events;
using LiveCoder.Scripting.Interfaces;
using LiveCoder.Scripting.Snippets;
using Microsoft.VisualStudio.Shell;

namespace LiveCoder.Extension
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class StepCommand
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 0x0100;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("08f19186-2a4b-41ff-998a-80b8be279cc8");

        private Package Package { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StepCommand"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        /// <param name="logger">Logger to be used by this instance.</param>
        private StepCommand(Package package, ILogger logger)
        {
            this.Package = package ?? throw new ArgumentNullException(nameof(package));
            this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.ServiceProvider.TryGetService<OleMenuCommandService>().Do(this.AddMenuItem);
        }

        private void AddMenuItem(OleMenuCommandService service) => 
            this.AddMenuItem(service, new CommandID(CommandSet, CommandId));

        private void AddMenuItem(OleMenuCommandService service, CommandID menuCommandId) => 
            service.AddCommand(new MenuCommand(this.MenuItemCallback, menuCommandId));

        private IMenuCommandService MenuCommandService =>
            this.ServiceProvider.GetService(typeof(IMenuCommandService)) as IMenuCommandService;

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static StepCommand Instance { get; private set; }

        private IEngine DemoEngine { get; set; }
        private ILogger Logger { get; set; }

        /// <summary>
        /// Gets the service provider from the owner package.
        /// </summary>
        private IServiceProvider ServiceProvider => this.Package;

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static void Initialize(Package package)
        {
            Instance = new StepCommand(package, new VsOutputLogger());
        }

        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void MenuItemCallback(object sender, EventArgs e)
        {
            try
            {
                DemoEngine = DemoEngine ?? this.CreateEngine(this.Logger);
                DemoEngine.Step();
            }
            catch (Exception ex)
            {
                this.Logger.Write(new Error(ex.ToString()));
            }
        }

        private IEngine CreateEngine(ILogger logger) =>
            this.TryGetSolution(logger)
                .Map(solution => this.CreateEngine(solution, logger))
                .Reduce(() => new NoSolution(logger));

        private IEngine CreateEngine(ISolution solution, ILogger logger) =>
            CodeSnippetsEngine.TryCreate(solution, logger)
                .Reduce(() => new NoScriptEngine(logger));

        private Option<ISolution> TryGetSolution(ILogger logger) => 
            this.ServiceProvider.TryGetSolution(logger);
    }
}
