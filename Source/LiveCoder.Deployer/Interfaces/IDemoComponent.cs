using System.Collections.Generic;
using System.Diagnostics.Contracts;
using LiveCoder.Deployer.Interfaces.Contracts;

namespace LiveCoder.Deployer.Interfaces
{
    [ContractClass(typeof(DemoComponentContracts))]
    public interface IDemoComponent
    {
        void DeployTo(IDestination destination);
        IEnumerable<IDeployedComponent> GetDeploymentRoot();
        bool IsDeployed { get; }
    }
}
