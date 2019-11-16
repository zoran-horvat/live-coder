namespace LiveCoder.Deployer.Tool.Interfaces
{
    public interface IDeployedComponent
    {
        string Name { get; }
        void Open();
    }
}
