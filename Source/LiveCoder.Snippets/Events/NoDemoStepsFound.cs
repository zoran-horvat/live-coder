using LiveCoder.Api;

namespace LiveCoder.Snippets.Events
{
    class NoDemoStepsFound : DelimitedEvent
    {
        protected override string InnerLabel => 
            "No demo steps found in the solution";
    }
}
