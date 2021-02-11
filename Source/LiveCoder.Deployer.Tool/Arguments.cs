using LiveCoder.Common.Optional;
using System.IO;
using System.Linq;

namespace LiveCoder.Deployer.Tool
{
    internal class Arguments
    {

        public bool IsValid { get; private set; }
        public DirectoryInfo SourceDirectory { get; private set; }
        public Option<DirectoryInfo> DestinationDirectory { get; private set; } = None.Value;
        public bool CopyVisualStudioFiles { get; private set; }
        public bool LiveTrackSnippets { get; private set; }
        public bool StartApplications { get; private set; }
        public bool CopyFiles => this.CopyVisualStudioFiles || this.CopyPowerPointFiles || this.CopySql;
        public bool CopySnippets { get; private set; }
        public bool CopyPowerPointFiles { get; private set; }
        public bool CopySql { get; private set; }
        public bool NormalizeSnippets { get; private set; }
        public bool OpenFiles { get; private set; }

        private ArgumentFlagHandler[] Handlers { get; }

        private Arguments()
        {
            this.Handlers = new[]
            {
                new ArgumentFlagHandler("-copy-vs", () => this.CopyVisualStudioFiles = true), 
                new ArgumentFlagHandler("-copy-snippets", () => this.CopySnippets = true), 
                new ArgumentFlagHandler("-copy-pp", () => this.CopyPowerPointFiles = true), 
                new ArgumentFlagHandler("-copy-sql", () => this.CopySql = true), 
                new ArgumentFlagHandler("-copy-all", this.CopyAll),
                new ArgumentFlagHandler("-track-snippets", this.CopyAndTrackSnippets),
                new ArgumentFlagHandler("-normalize-snippets", () => this.NormalizeSnippets = true), 
                new ArgumentFlagHandler("-open", () => this.OpenFiles = true), 
            };
        }

        private void CopyAll()
        {
            this.CopyVisualStudioFiles = true;
            this.CopySnippets = true;
            this.CopyPowerPointFiles = true;
            this.CopySql = true;
        }

        private void CopyAndTrackSnippets()
        {
            this.CopySnippets = true;
            this.LiveTrackSnippets = true;
        }

        private bool CanHandleFlag(string argument) => 
            this.Handlers.Any(handler => handler.CanHandle(argument));

        private void HandleFlag(string argument) => 
            this.Handlers.First(handler => handler.CanHandle(argument)).Handle(argument);

        private bool CanHandleSourceDirectory(string[] args, int pos) =>
            this.CanHandleDirectory(args, pos, "src");

        private bool CanHandleDestinationDirectory(string[] args, int pos) =>
            this.CanHandleDirectory(args, pos, "dst");

        private bool CanHandleDirectory(string[] args, int pos, string flag) =>
            args[pos] == $"-{flag}" && args.Length > pos + 1 && Directory.Exists(args[pos + 1]);

        private void HandleSourceDirectory(string[] args, int pos) => 
            this.SourceDirectory = new DirectoryInfo(args[pos + 1]);

        private void HandleDestinationDirectory(string[] args, int pos) =>
            this.DestinationDirectory = new DirectoryInfo(args[pos + 1]);

        public static Arguments Parse(string[] args)
        {

            int pos = 0;
            Arguments arguments = new Arguments()
            {
                StartApplications = true
            };

            if (args == null)
                return arguments;

            while (pos < args.Length)
            {
                if (arguments.CanHandleSourceDirectory(args, pos))
                {
                    arguments.HandleSourceDirectory(args, pos);
                    pos += 2;
                }
                else if (arguments.CanHandleDestinationDirectory(args, pos))
                {
                    arguments.HandleDestinationDirectory(args, pos);
                    pos += 2;
                }
                else if (arguments.CanHandleFlag(args[pos]))
                {
                    arguments.HandleFlag(args[pos]);
                    pos += 1;
                }
                else
                {
                    return arguments;
                }
            }

            if (arguments.SourceDirectory != null)
            {
                arguments.IsValid = true;
            }

            return arguments;

        }

    }
}
