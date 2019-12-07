using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using LiveCoder.Common;

namespace LiveCoder.Scripting.Parsing.Tree
{
    class GlobalExpression : Node, IEnumerable<Reference>
    {
        private ImmutableList<Reference> References { get; }

        public static GlobalExpression Empty => 
            new GlobalExpression(ImmutableList<Reference>.Empty);

        private GlobalExpression(ImmutableList<Reference> references)
        {
            this.References = references;
        }

        public GlobalExpression Append(Reference reference) =>
            new GlobalExpression(this.References.Add(reference));

        public GlobalExpression Append(IEnumerable<Reference> references) =>
            new GlobalExpression(this.References.AddRange(references));

        public IEnumerator<Reference> GetEnumerator() => 
            this.References.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => 
            GetEnumerator();

        public override string ToString() =>
            this.References.Select(r => r.ToString()).Join(".");
    }
}
