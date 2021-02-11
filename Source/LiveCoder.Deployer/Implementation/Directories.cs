using System;
using System.IO;
using System.Linq;
using System.Threading;
using LiveCoder.Common.IO;
using LiveCoder.Common.Optional;

namespace LiveCoder.Deployer.Implementation
{
    public class Directories
    {
        private DirectoryInfo DestinationRoot { get; }
        private DirectoryInfo Source { get; }
     
        private Directories(DirectoryInfo source, DirectoryInfo destinationRoot)
        {
            this.Source = source;
            this.DestinationRoot = destinationRoot;
        }

        public static Option<Directories> TryCreateDestinationIn(IAuditor auditor, DirectoryInfo destination, DirectoryInfo source)
        {
            int repeats = 10;
            int pauseMsec = 100;

            while (repeats > 0)
            {
                try
                {
                    if (TryCreate(destination) is Some<DirectoryInfo> created)
                    {
                        Empty(created);
                        return new Directories(source, created);
                    }
                    repeats -= 1;
                    Thread.Sleep(pauseMsec);
                }
                catch (Exception ex)
                {
                    auditor.FailedToCreateDestination(ex.Message);
                    return None.Value;
                }
            }

            return None.Value;
        }

        private static void Empty(DirectoryInfo directory)
        {
            directory.GetDirectories().ToList().ForEach(dir => dir.Delete(true));
            directory.GetFiles().ToList().ForEach(file => file.Delete());
        }

        private static Option<DirectoryInfo> TryCreate(DirectoryInfo directory)
        {
            try
            {
                directory.Create();
                return directory;
            }
            catch (IOException)
            {
                return None.Value;
            }
        }

        public static DirectoryInfo GetDestinationDirectoryIn(DirectoryInfo parent) =>
            new DirectoryInfo(Path.Combine(parent.FullName, Timestamp));

        public DirectoryInfo DestinationFor(DirectoryInfo source) =>
            Directory.CreateDirectory(this.DestinationPathFor(source));

        public DirectoryInfo InternalDestination =>
            this.DestinationRoot.CreateSubdirectory(".livecoder");

        private string DestinationPathFor(DirectoryInfo source) =>
            Path.Combine(this.DestinationRoot.FullName, this.RelativeSourcePath(source));

        private string RelativeSourcePath(DirectoryInfo source) =>
            source.RelativeTo(this.Source);
           
        private static string Timestamp =>
            $"{DateTime.UtcNow:yyyyMMddHHmmss}";

        public override string ToString() =>
            $"Destination [{this.DestinationRoot.FullName}]";
    }
}
