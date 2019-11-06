using LiveCoderExtension.Interfaces;

namespace LiveCoderExtension.Events
{
    class NoDemoStepsFound : IEvent
    {
        public string Label => "No demo steps found in the solution";
    }
}
