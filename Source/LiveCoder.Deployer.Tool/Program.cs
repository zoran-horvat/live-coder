using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using LiveCoder.Common;
using LiveCoder.Common.Optional;
using LiveCoder.Common.Text;
using LiveCoder.Deployer.Tool.Interfaces;

namespace LiveCoder.Deployer.Tool
{
    class Program
    {
        private static void ShowUsage() =>
            Console.WriteLine(LoadUsage());

        private static string LoadUsage() =>
            Disposable.Using(TryGetUsageStream)
                .TryMap(ReadText)
                .Map(text => text.Replace("%toolName%", Assembly.GetExecutingAssembly().GetName().Name))
                .Reduce(string.Empty);

        private static Option<Stream> TryGetUsageStream() =>
            TryGetResourceStream(typeof(Program).Assembly, "LiveCoder.Deployer.Tool.res.usage.txt");

        private static Option<Stream> TryGetResourceStream(Assembly assembly, string name) =>
            assembly.GetManifestResourceNames()
                .FirstOrNone(resource => resource == name)
                .Map(assembly.GetManifestResourceStream);

        private static string ReadText(Stream stream) =>
            Disposable.Using(() => new StreamReader(stream)).Map(ReadText);

        private static string ReadText(TextReader reader) =>
            reader.ReadToEnd();

        private static IEnumerable<IDeployedComponent> GetDeployedComponents(IEnumerable<IDeployer> deployers) =>
            deployers.SelectMany(deployer => deployer.DeployedComponents);

        private static void Main(string[] args) =>
            Deploy(Arguments.Parse(args));

        private static void Deploy(Arguments arguments) =>
            Option.Of(new DeploymentBuilder())
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
