namespace VSExtension.Interfaces
{
    interface IDemoCommand
    {
        bool CanExecute { get; }
        void Execute();
    }
}
