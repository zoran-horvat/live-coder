using LiveCoderExtension.Interfaces;

namespace LiveCoderExtension.Implementation.Commands
{
    class Pause : IDemoCommand
    {
        public void Execute() { }
        public override string ToString() => "pause";
    }
}
