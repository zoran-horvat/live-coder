using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using LiveCoder.Common.Optional;
using LiveCoder.Deployer.Implementation.Artifacts;

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

        public Option<VisualStudioSolution> SolutionFile =>
            this.Artifacts.OfType<VisualStudioSolution>().FirstOrNone();

        public Option<Slides> SlidesFile =>
            this.Artifacts.OfType<Slides>().FirstOrNone();

        public Option<TranslatedSnippetsScript> XmlSnippets =>
            this.Artifacts.OfType<TranslatedSnippetsScript>().FirstOrNone();
    }
}