namespace LiveCoder.Deployer.Interfaces
{
    public interface IDeployedComponent
    {
        string Name { get; }
        void Open();
    }
}
