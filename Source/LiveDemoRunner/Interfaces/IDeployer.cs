using System.Collections.Generic;
using System.Diagnostics.Contracts;
using LiveDemoRunner.Interfaces.Contracts;

namespace LiveDemoRunner.Interfaces
{
    [ContractClass(typeof(DeployerContracts))]
    public interface IDeployer
    {
        void Deploy();
        IEnumerable<IDeployedComponent> DeployedComponents { get; } 
        bool IsDeployed { get; }
    }
}
