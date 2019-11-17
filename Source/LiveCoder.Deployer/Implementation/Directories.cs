using System;
using System.IO;
using System.Threading;
using LiveCoder.Common.Optional;

namespace LiveCoder.Deployer.Implementation
{
    class Directories
    {
        private DirectoryInfo DestinationRoot { get; }
     
        private Directories(DirectoryInfo destinationRoot)
        {
            this.DestinationRoot = destinationRoot;
        }

        public static Option<Directories> TryCreateDestinationIn(DirectoryInfo destinationParent)
        {
            int repeats = 10;
            int pauseMsec = 100;

            while (repeats > 0)
            {
                if (TryCreate(GetDestinationCandidate(destinationParent)) is Some<DirectoryInfo> created)
                    return new Directories(created.Content);
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

        private static string Timestamp =>
            $"{DateTime.UtcNow:yyyyMMddHHmmssfff}.new";
    }
}
