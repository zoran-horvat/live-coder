using System;

namespace LiveCoder.Deployer.Tool
{
    class ConsoleAuditor : IAuditor
    {
        public void ComponentDeployed(Artifact artifact) => 
            Console.WriteLine($"Deployed {artifact}");

        public void FailedToCreateDestination(string message) => 
            Console.WriteLine($"Failed to create deployment directories: {message}");
    }
}
