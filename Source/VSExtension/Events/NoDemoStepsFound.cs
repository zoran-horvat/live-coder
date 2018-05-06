using VSExtension.Interfaces;

namespace VSExtension.Events
{
    class NoDemoStepsFound : IEvent
    {
        public string Label => "No demo steps found in the solution";
    }
}
