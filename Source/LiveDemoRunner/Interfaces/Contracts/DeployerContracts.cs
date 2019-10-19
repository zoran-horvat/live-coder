using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace LiveDemoRunner.Interfaces.Contracts
{
    [ContractClassFor(typeof(IDeployer))]
    public abstract class DeployerContracts: IDeployer
    {
        public void Deploy()
        {
        }

        public IEnumerable<IDeployedComponent> DeployedComponents
        {
            get
            {
                Contract.Requires(this.IsDeployed, "Components must be deployed first.");
                Contract.Ensures(Contract.Result<IEnumerable<IDeployedComponent>>() != null, "Deployer must return non-null sequence of deployed components.");
                Contract.Ensures(Contract.Result<IEnumerable<IDeployedComponent>>().All(component => !object.ReferenceEquals(component, null)), "All deployed components must be non-null.");
                return null;
            }
        }

        public bool IsDeployed => false;

    }
}
