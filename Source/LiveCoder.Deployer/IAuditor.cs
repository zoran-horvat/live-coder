namespace LiveCoder.Deployer
{
    public interface IAuditor
    {
        void ComponentDeployed(Artifact artifact);
        void FailedToCreateDestination(string message);
    }
}
