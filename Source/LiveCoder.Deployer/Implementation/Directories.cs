using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using LiveCoder.Common.IO;
using LiveCoder.Common.Optional;

namespace LiveCoder.Deployer.Implementation
{
    class Directories
    {
        private DirectoryInfo DestinationRoot { get; }
        private DirectoryInfo Source { get; }
     
        private Directories(DirectoryInfo source, DirectoryInfo destinationRoot)
        {
            this.Source = source;
            this.DestinationRoot = destinationRoot;
        }

        public static Option<Directories> TryCreateDestinationIn(DirectoryInfo destinationParent, DirectoryInfo source)
        {
            int repeats = 10;
            int pauseMsec = 100;

            while (repeats > 0)
            {
                if (TryCreate(GetDestinationCandidate(destinationParent)) is Some<DirectoryInfo> created)
                    return new Directories(source, created.Content);
                repeats -= 1;
                Thread.Sleep(pauseMsec);
            }

            return None.Value;
        }

        private static Option<DirectoryInfo> TryCreate(DirectoryInfo directory)
        {
            try
            {
                directory.Create();
                return directory;
            }
            catch
            {
                return None.Value;
            }
        }

        private static DirectoryInfo GetDestinationCandidate(DirectoryInfo parent) =>
            new DirectoryInfo(Path.Combine(parent.FullName, Timestamp));

        public DirectoryInfo DestinationFor(DirectoryInfo source) =>
            new DirectoryInfo(this.DestinationPathFor(source));

        private string DestinationPathFor(DirectoryInfo source) =>
            Path.Combine(this.DestinationRoot.FullName, this.RelativeSourcePath(source));

        private string RelativeSourcePath(DirectoryInfo source) =>
            source.RelativeTo(this.Source);
           
        private static string Timestamp =>
            $"{DateTime.UtcNow:yyyyMMddHHmmssfff}.new";

        public override string ToString() =>
            $"Destination [{this.DestinationRoot.FullName}]";
    }
}
