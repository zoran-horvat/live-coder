using LiveCoder.Api;

namespace LiveCoder.Snippets.Events
{
    public class SnippetText : IEvent
    {
        public string Label { get; }
     
        public SnippetText(string label, int currentSnippet, int totalSnippets)
        {
            this.Label = $"{label} ({currentSnippet}/{totalSnippets})";
        }
    }
}
