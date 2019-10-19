using System.Diagnostics.Contracts;
using System.IO;

namespace LiveDemoRunner.Interfaces.Contracts
{
    [ContractClassFor(typeof(IDestination))]
    public abstract class DestinationContracts: IDestination
    {
        public void DeployFile(string name, Stream stream)
        {
            Contract.Requires(!string.IsNullOrEmpty(name), "File name must be non-empty.");
            Contract.Requires(stream != null, "File stream must be non-null.");
            Contract.Requires(name.IndexOfAny(Path.GetInvalidFileNameChars()) < 0, "File name must be a valid file system name.");
        }

        public void DeployDirectory(string name)
        {
            Contract.Requires(!string.IsNullOrEmpty(name), "Directory name must be non-empty.");
            Contract.Requires(name.IndexOfAny(Path.GetInvalidFileNameChars()) < 0, "Directory name must be a valid file system name.");
        }

        public IDestination GetDirectory(string name)
        {
            Contract.Requires(!string.IsNullOrEmpty(name), "Directory name must be non-empty.");
            Contract.Requires(name.IndexOfAny(Path.GetInvalidFileNameChars()) < 0, "Directory name must be a valid file system name.");
            Contract.Requires(this.DirectoryExists(name), "Directory must exist.");
            Contract.Ensures(Contract.Result<IDestination>() != null, "Destination must produce non-null directory after deployment.");
            return null;
        }

        public IDeployedComponent GetFile(string name)
        {
            Contract.Requires(!string.IsNullOrEmpty(name), "File name must be non-empty.");
            Contract.Requires(name.IndexOfAny(Path.GetInvalidFileNameChars()) < 0, "File name must be a valid file system name.");
            Contract.Requires(this.FileExists(name), "File must exist.");
            Contract.Ensures(Contract.Result<IDeployedComponent>() != null, "Destination must return non-null file.");
            return null;
        }

        public bool DirectoryExists(string name)
        {
            Contract.Requires(!string.IsNullOrEmpty(name), "Directory name must be non-empty.");
            Contract.Requires(name.IndexOfAny(Path.GetInvalidFileNameChars()) < 0, "Directory name must be a valid file system name.");
            return false;
        }

        public bool FileExists(string name)
        {
            Contract.Requires(!string.IsNullOrEmpty(name), "File name must be non-empty.");
            Contract.Requires(name.IndexOfAny(Path.GetInvalidFileNameChars()) < 0, "File name must be a valid file system name.");
            return false;
        }
    }
}
