using LiveCoder.Extension.Interfaces;
using LiveCoder.Scripting;

namespace LiveCoder.Extension.Events
{
    class NoDemoStepsFound : IEvent
    {
        public string Label => "No demo steps found in the solution";
    }
}
