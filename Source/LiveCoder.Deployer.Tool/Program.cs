using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using LiveCoder.Deployer.Tool.Deployers;
using LiveCoder.Deployer.Tool.Infrastructure;
using LiveCoder.Deployer.Tool.Interfaces;
using LiveCoder.Deployer;

namespace LiveCoder.Deployer.Tool
{
    class Program
    {

        private static ILogger Logger { get; } = new ConsoleLogger();

        static void ShowUsage()
        {

            string assemblyName = Assembly.GetExecutingAssembly().GetName().Name;

            string[] usage = new[]
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

            foreach (string line in usage)
                Console.WriteLine(line);

        }

        static IEnumerable<IDeployedComponent> GetDeployedComponents(IEnumerable<IDeployer> deployers) =>
            deployers.SelectMany(deployer => deployer.DeployedComponents);

        static void Main(string[] args)
        {

            Arguments arguments = Arguments.Parse(args);

            if (arguments.IsValid)
            {
                new ParameterizedDeployer(arguments, Logger).Deploy();

                new DeploymentBuilder()
                    .From(arguments.SourceDirectory)
                    .TryBuild()
                    .Do(Deploy, ShowUsage);
            }
            else
            {
                ShowUsage();
            }
        }

        static void Deploy(DeploymentSpecification specification) =>
            specification.Execute().Do(Open);

        static void Open(Deployment deployment)
        {
            Debug.WriteLine("Ready to open deployed components.");
        }
    }
}
