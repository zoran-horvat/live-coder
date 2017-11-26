using VSExtension.Interfaces;

namespace VSExtension.Implementation.Commands
{
    class Pause : IDemoCommand
    {
        public void Execute() { }
        public override string ToString() => "pause";
    }
}
