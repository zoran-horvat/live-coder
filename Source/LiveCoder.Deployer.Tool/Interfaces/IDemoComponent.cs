using System.Collections.Generic;

namespace LiveCoder.Deployer.Tool.Interfaces
{
    public interface IDemoComponent
    {
        void DeployTo(IDestination destination);
        IEnumerable<IDeployedComponent> GetDeploymentRoot();
        bool IsDeployed { get; }
    }
}
