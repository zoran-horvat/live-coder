using System.Collections.Generic;
using System.Diagnostics.Contracts;
using LiveDemoRunner.Interfaces;

namespace LiveDemoRunner.Models
{
    public class Demo
    {

        private IEnumerable<IDemoComponent> Components { get; } 

        public Demo(IEnumerable<IDemoComponent> components)
        {

            Contract.Requires(components != null, "Demo components must be non-null.");
            Contract.ForAll(components, component => !object.ReferenceEquals(component, null));

            this.Components = new List<IDemoComponent>(components);

        }
    }
}
