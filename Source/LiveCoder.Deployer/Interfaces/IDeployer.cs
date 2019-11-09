using System.Collections.Generic;
using System.Diagnostics.Contracts;
using LiveCoder.Deployer.Interfaces.Contracts;

namespace LiveCoder.Deployer.Interfaces
{
    [ContractClass(typeof(DeployerContracts))]
    public interface IDeployer
    {
        void Deploy();
        IEnumerable<IDeployedComponent> DeployedComponents { get; } 
        bool IsDeployed { get; }
    }
}
