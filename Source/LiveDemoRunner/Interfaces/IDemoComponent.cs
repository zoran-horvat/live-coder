using System.Collections.Generic;
using System.Diagnostics.Contracts;
using LiveDemoRunner.Interfaces.Contracts;

namespace LiveDemoRunner.Interfaces
{
    [ContractClass(typeof(DemoComponentContracts))]
    public interface IDemoComponent
    {
        void DeployTo(IDestination destination);
        IEnumerable<IDeployedComponent> GetDeploymentRoot();
        bool IsDeployed { get; }
    }
}
