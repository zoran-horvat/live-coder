using System.ComponentModel.Design;
using LiveCoder.Api;
using LiveCoder.Extension.Implementation;
using LiveCoder.Scripting;
using LiveCoder.Snippets.Events;

namespace LiveCoder.Extension
{
    [Command(PackageIds.StepCommand)]
    internal sealed class StepCommand : BaseCommand<StepCommand>
    {
        public StepCommand() { }

        private IMenuCommandService MenuCommandService =>
            this.ServiceProvider.GetService(typeof(IMenuCommandService)) as IMenuCommandService;

        private IEngine DemoEngine { get; set; }
        private ILogger Logger { get; set; } = CreateLogger();

        private IServiceProvider ServiceProvider => this.Package;

        private static ILogger CreateLogger() =>
            new VsOutputLogger().And(new VsStatusBarLogger().ForEvents<SnippetText>());

        protected override Task ExecuteAsync(OleMenuCmdEventArgs e) => Task.Run(this.MenuItemCallback);

        private void MenuItemCallback()
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
            this.ServiceProvider
                .TryGetSolution(logger)
                .Map(solution => Engine.Create(solution, solution.LiveCoderDirectory, logger))
                .Reduce(() => new NoSolution(logger));
    }
}
