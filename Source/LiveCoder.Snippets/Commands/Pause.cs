using LiveCoder.Snippets.Interfaces;

namespace LiveCoder.Snippets.Commands
{
    class Pause : IDemoCommand
    {
        public void Execute() { }
        public override string ToString() => "pause";
    }
}
