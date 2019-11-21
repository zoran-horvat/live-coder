using System;
using System.Collections.Generic;
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
        private static void ShowUsage()
        {

            string assemblyName = Assembly.GetExecutingAssembly().GetName().Name;

            string[] usage =
            {
                "Demo deployment tool",
                "--------------------",
                "",
                "Deploys C# solution, code snippets and PowerPoint presentation",
                "to isolated locations from which demonstration can be performed.",
                "",
                "Usage:",
                "",
                $"{assemblyName} -src <directory> [-copy-vs] [-copy-snippets] [-copy-pp] ",
                "        [-copy-all] [-track-snippets] [-normalize-snippets] [-open]",
                "",
                "Meaning of attributes:",
                "",
                "-src                - Indicates source directory from which files are copied",
                "",
                "-copy-vs            - Instructs to copy Visual Studio solution files",
                "",
                "-copy-snippets      - Instructs to copy snippets file to all Visual Studios",
                "",
                "-copy-pp            - Instructs to copy PowerPoint presentations",
                "",
                "-copy-all           - Instructs to copy all files to their corresponding",
                "                      destinations",
                "",
                "-track-snippets     - When present indicates that only .snippet",
                "                      file(s) should be located, deployed and then",
                "                      watched for changes; whenever changed, file(s)",
                "                      will be re-deployed",
                "",
                "-normalize-snippets - Indicates that snippets file(s) should be normalized",
                "                      before deployment, i.e. that all snippet shortcuts",
                "                      should follow uninterrupted sequence ",
                "                      snp01, snp02, snp03, and so on.",
                "",
                "-open               - Indicates that Visual Studio and/or PowerPoint",
                "                      should be open after deployment.",
                ""
            };

            Console.WriteLine(usage.Join(Environment.NewLine));
        }

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
            line?.Equals("exit", StringComparison.OrdinalIgnoreCase) ?? true;

        private static void PromptExit() => 
            Console.WriteLine("Type 'exit' to quit tracking changes");
    }
}
