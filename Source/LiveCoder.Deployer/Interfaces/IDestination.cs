using System.IO;

namespace LiveCoder.Deployer.Interfaces
{
    public interface IDestination
    {

        void DeployFile(string name, Stream stream);
        void DeployDirectory(string name);
        IDestination GetDirectory(string name);
        IDeployedComponent GetFile(string name);

        bool DirectoryExists(string name);

        bool FileExists(string name);

    }
}
