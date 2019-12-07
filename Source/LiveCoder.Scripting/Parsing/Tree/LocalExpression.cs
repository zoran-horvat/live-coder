using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using LiveCoder.Common;

namespace LiveCoder.Scripting.Parsing.Tree
{
    class LocalExpression : Node, IEnumerable<Reference>
    {
        private ImmutableList<Reference> References { get; }

        public static LocalExpression Empty => 
            new LocalExpression(ImmutableList<Reference>.Empty);

        private LocalExpression(ImmutableList<Reference> references)
        {
            this.References = references;
        }

        public LocalExpression Append(Reference reference) =>
            new LocalExpression(this.References.Add(reference));

        public IEnumerator<Reference> GetEnumerator() =>
            this.References.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => 
            GetEnumerator();

        public override string ToString() =>
            this.References.Select(r => r.ToString()).Join(".");
    }
}
