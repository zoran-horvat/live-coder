using System.Diagnostics.Contracts;

namespace LiveDemoRunner.Interfaces.Contracts
{
    public abstract class FutureDestinationContracts: IFutureDestination
    {
        public void PrepareForDeployment()
        {
        }

        public IDestination GetDeploymentDestination()
        {
            Contract.Ensures(Contract.Result<IDestination>() != null, "Returned destination must be non-null.");
            return null;
        }
    }
}
