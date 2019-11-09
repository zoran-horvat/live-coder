namespace LiveCoder.Deployer.Interfaces
{
    public interface IFutureDestination
    {
        void PrepareForDeployment();
        IDestination GetDeploymentDestination();
    }
}
