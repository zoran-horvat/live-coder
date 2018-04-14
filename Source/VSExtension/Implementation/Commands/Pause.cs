using VSExtension.Interfaces;

namespace VSExtension.Implementation.Commands
{
    class Pause : IDemoCommand
    {
        public bool CanExecute => true;
        public void Execute() { }
        public override string ToString() => "pause";
    }
}
