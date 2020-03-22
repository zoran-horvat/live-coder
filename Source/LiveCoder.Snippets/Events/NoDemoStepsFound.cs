using LiveCoder.Api;

namespace LiveCoder.Snippets.Events
{
    class NoDemoStepsFound : IEvent
    {
        public string Label => "No demo steps found in the solution";
    }
}
