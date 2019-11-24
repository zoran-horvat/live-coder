using LiveCoder.Scripting;

namespace LiveCoder.Extension.Implementation.Commands
{
    class Pause : IDemoCommand
    {
        public void Execute() { }
        public override string ToString() => "pause";
    }
}
