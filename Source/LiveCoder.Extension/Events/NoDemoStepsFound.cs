using LiveCoder.Extension.Interfaces;

namespace LiveCoder.Extension.Events
{
    class NoDemoStepsFound : IEvent
    {
        public string Label => "No demo steps found in the solution";
    }
}
