using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace LiveDemoRunner.Interfaces.Contracts
{
    [ContractClassFor(typeof(IDemoComponent))]
    public abstract class DemoComponentContracts: IDemoComponent
    {
        public void DeployTo(IDestination destination)
        {
            Contract.Requires(destination != null, "Destination must be non-null.");
        }

        public bool IsDeployed => false;

        public IDeployedComponent GetDeploymentRoot(IDestination destination)
        {
            Contract.Requires(destination != null, "Deployment destination must be non-null.");
            Contract.Ensures(Contract.Result<IDeployedComponent>() != null, "Component must return non-null deployment root object.");
            return null;
        }

        public IEnumerable<IDeployedComponent> GetDeploymentRoot()
        {
            Contract.Requires(this.IsDeployed, "Component must be deployed.");
            Contract.Ensures(Contract.Result<IEnumerable<IDeployedComponent>>() != null, "Component must return non-null deployed components sequence.");
            return Enumerable.Empty<IDeployedComponent>();
        }

    }
}
