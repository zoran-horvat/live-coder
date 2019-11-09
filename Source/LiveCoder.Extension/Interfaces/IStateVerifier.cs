namespace LiveCoder.Extension.Interfaces
{
    interface IStateVerifier : IDemoCommand
    {
        bool IsStateAsExpected { get; }
        string PrintableReport { get; }
    }
}
