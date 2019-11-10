﻿using System;
using System.ComponentModel.Design;
using LiveCoder.Extension.Implementation;
using LiveCoder.Extension.Interfaces;
using Microsoft.VisualStudio.Shell;

namespace LiveCoder.Extension
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class Step
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
        /// Initializes a new instance of the <see cref="Step"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        private Step(Package package)
        {
            this.Package = package ?? throw new ArgumentNullException(nameof(package));
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
        public static Step Instance { get; private set; }

        private static IEngine DemoEngine { get; set; }

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
            Instance = new Step(package);
            DemoEngine = null;
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
            ILogger logger = this.CreateLogger();
            DemoEngine = DemoEngine ?? new DemoEngine(this.ServiceProvider.GetSolution(logger), logger);
            DemoEngine.Step();
        }

        private ILogger CreateLogger() =>
            new VsOutputLogger();
    }
}