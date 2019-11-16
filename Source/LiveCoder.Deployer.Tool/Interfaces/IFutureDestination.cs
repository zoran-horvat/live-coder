namespace LiveCoder.Deployer.Tool.Interfaces
{
    public interface IFutureDestination
    {
        void PrepareForDeployment();
        IDestination GetDeploymentDestination();
    }
}
