using System.Collections.Generic;
using LiveCoder.Deployer.Interfaces;

namespace LiveCoder.Deployer.Models
{
    public class Demo
    {

        private IEnumerable<IDemoComponent> Components { get; } 

        public Demo(IEnumerable<IDemoComponent> components)
        {
            this.Components = new List<IDemoComponent>(components);
        }
    }
}
