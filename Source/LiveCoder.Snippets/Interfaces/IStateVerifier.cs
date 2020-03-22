namespace LiveCoder.Snippets.Interfaces
{
    interface IStateVerifier : IDemoCommand
    {
        bool IsStateAsExpected { get; }
        string PrintableReport { get; }
    }
}
