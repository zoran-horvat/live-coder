using LiveCoder.Scripting.Interfaces;

namespace LiveCoder.Scripting.Events
{
    class NoDemoStepsFound : IEvent
    {
        public string Label => "No demo steps found in the solution";
    }
}
