using System.Collections.Generic;
using LiveCoder.Deployer.Tool.Interfaces;

namespace LiveCoder.Deployer.Tool.Models
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
