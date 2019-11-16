using System.Collections.Generic;

namespace LiveCoder.Deployer.Tool.Interfaces
{
    public interface IDeployer
    {
        void Deploy();
        IEnumerable<IDeployedComponent> DeployedComponents { get; } 
        bool IsDeployed { get; }
    }
}
