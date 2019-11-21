using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LiveCoder.Common.Optional;
using LiveCoder.Common.Resources;
using LiveCoder.Common.Text;

namespace LiveCoder.Deployer.Tool
{
    class Program
    {
        private static void ShowUsage() =>
            Console.WriteLine(LoadUsage());

        private static string LoadUsage() =>
            typeof(Program).Assembly.TryReadEmbeddedResourceText("LiveCoder.Deployer.Tool.res.usage.txt")
                .Map(text => text.Replace("%toolName%", Assembly.GetExecutingAssembly().GetName().Name))
                .Reduce(string.Empty);

        private static void Main(string[] args) =>
            Deploy(Arguments.Parse(args));

        private static void Deploy(Arguments arguments) =>
            Option.Of(new DeploymentBuilder(new ConsoleAuditor()))
                .When(_ => arguments.IsValid)
                .Map(builder => builder.From(arguments.SourceDirectory))
                .MapOptional(builder => builder.TryBuild())
                .Do(spec => Deploy(spec, arguments), ShowUsage);

        private static void Deploy(DeploymentSpecification specification, Arguments arguments) =>
            specification.Execute().Do(deployment => PostDeploy(deployment, arguments));

        private static void PostDeploy(Deployment deployment, Arguments arguments)
        {
            OpenFiles(deployment, arguments);
            TrackSnippets(deployment, arguments);
        }

        private static void OpenFiles(Deployment deployment, Arguments arguments)
        {
            if (!arguments.OpenFiles) return;
            deployment.SolutionFile.Do(solution => solution.Open());
            deployment.SlidesFile.Do(slides => slides.Open());
        }

        private static void TrackSnippets(Deployment deployment, Arguments arguments)
        {
            if (!arguments.LiveTrackSnippets) return;
            deployment.XmlSnippets.Do(snippets => snippets.RedeployOnChange());
            WaitForExit();
        }

        private static IEnumerable<string> WaitForExit() =>
            Console.In.GetLines(PromptExit)
                .TakeWhile(NotExitCommand)
                .ToList();

        private static bool NotExitCommand(string line) =>
            !IsExitCommand(line);

        private static bool IsExitCommand(string line) =>
            line?.Equals("exit", StringComparison.OrdinalIgnoreCase) ?? true;

        private static void PromptExit() => 
            Console.WriteLine("Type 'exit' to quit tracking changes");
    }
}
