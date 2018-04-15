namespace VSExtension.Interfaces
{
    interface IStateVerifier : IDemoCommand
    {
        bool IsStateAsExpected { get; }
    }
}
