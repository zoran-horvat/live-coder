﻿using System.Diagnostics.Contracts;
using System.IO;
using LiveDemoRunner.Interfaces.Contracts;

namespace LiveDemoRunner.Interfaces
{
    [ContractClass(typeof(DestinationContracts))]
    public interface IDestination
    {

        void DeployFile(string name, Stream stream);
        void DeployDirectory(string name);
        IDestination GetDirectory(string name);
        IDeployedComponent GetFile(string name);

        [Pure]
        bool DirectoryExists(string name);

        [Pure]
        bool FileExists(string name);

    }
}
