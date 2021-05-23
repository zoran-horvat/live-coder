using LiveCoder.Api;

namespace LiveCoder.Snippets.Events
{
    class SnippetText : IEvent
    {
        public string Label { get; }
     
        public SnippetText(string label)
        {
            this.Label = label;
        }
    }
}
