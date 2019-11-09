using System;
using LiveCoder.Deployer.Interfaces;

namespace LiveCoder.Deployer.Infrastructure
{
    internal class ConsoleLogger: ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine("{0:HH:mm:ss} {1}", DateTime.Now, message);
        }
    }
}
