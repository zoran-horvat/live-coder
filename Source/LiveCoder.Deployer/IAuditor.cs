namespace LiveCoder.Deployer
{
    public interface IAuditor
    {
        void ComponentDeployed(Artifact artifact);
    }
}
