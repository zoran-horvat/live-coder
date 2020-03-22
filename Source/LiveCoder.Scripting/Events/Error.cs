using LiveCoder.Api;

namespace LiveCoder.Scripting.Events
{
    public class Error : IEvent
    {
        public string Label => $"ERROR: {this.Message}";
        private string Message { get; }

        public Error(string message)
        {
            this.Message = string.IsNullOrWhiteSpace(message) ? "Unknown error" : message;
        }
    }
}
