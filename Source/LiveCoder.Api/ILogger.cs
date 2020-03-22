namespace LiveCoder.Api
{
    public interface ILogger
    {
        void Write(IEvent @event);
    }
}
