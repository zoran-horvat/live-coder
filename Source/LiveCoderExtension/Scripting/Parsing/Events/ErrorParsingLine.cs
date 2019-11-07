using LiveCoderExtension.Interfaces;

namespace LiveCoderExtension.Scripting.Parsing.Events
{
    class ErrorParsingLine : IEvent
    {
        public string Label =>
            $"Error parsing line #{this.CurrentText.LineIndex + 1}: {this.CurrentText.CurrentLine}";

        private Text CurrentText { get; }

        public ErrorParsingLine(Text currentText)
        {
            this.CurrentText = currentText;
        }
    }
}
