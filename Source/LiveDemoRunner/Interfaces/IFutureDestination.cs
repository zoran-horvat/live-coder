namespace LiveDemoRunner.Interfaces
{
    public interface IFutureDestination
    {
        void PrepareForDeployment();
        IDestination GetDeploymentDestination();
    }
}
