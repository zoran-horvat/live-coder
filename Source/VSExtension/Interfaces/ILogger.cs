namespace VSExtension.Interfaces
{
    interface ILogger
    {
        void Write(IEvent @event);
    }
}
