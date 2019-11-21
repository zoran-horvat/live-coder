using System;

namespace LiveCoder.Deployer.Tool
{
    class ConsoleAuditor : IAuditor
    {
        public void ComponentDeployed(Artifact artifact) => 
            Console.WriteLine($"Deployed {artifact}");

        public void FailedToCreateDestination() => 
            Console.WriteLine("Failed to create deployment directories.");
    }
}
