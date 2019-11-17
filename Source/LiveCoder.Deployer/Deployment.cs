using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using LiveCoder.Deployer.Implementation;

namespace LiveCoder.Deployer
{
    public class Deployment
    {
        private IEnumerable<Artifact> Artifacts { get; }

        public Deployment(IEnumerable<Artifact> artifacts)
        {
            this.Artifacts = artifacts.ToList();
            foreach (Artifact obj in this.Artifacts)
            {
                Debug.WriteLine(obj);
            }
        }
    }
}