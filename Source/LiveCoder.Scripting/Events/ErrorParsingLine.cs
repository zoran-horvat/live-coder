using LiveCoder.Scripting.Interfaces;

namespace LiveCoder.Scripting.Events
{
    public class ErrorParsingLine : IEvent
    {
        private int LineNumber { get; }
        private string Content { get; }

        public string Label =>
            $"Error parsing line #{this.LineNumber}: {this.Content}";

        public ErrorParsingLine(int lineNumber, string content)
        {
            this.LineNumber = lineNumber;
            this.Content = content;
        }
    }
}
