using System.Collections.Generic;
using System.Linq;
using LiveCoder.Deployer.Implementation;

namespace LiveCoder.Deployer
{
    public class Deployment
    {
        private IEnumerable<Artefact> Artefacts { get; }

        public Deployment(IEnumerable<Artefact> artefacts)
        {
            this.Artefacts = artefacts.ToList();
        }
    }
}