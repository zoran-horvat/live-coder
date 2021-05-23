namespace LiveCoder.Api.Loggers
{
    class TypedLogger<TEvent> : ILogger
    {
        private ILogger Logger { get; }
        
        public TypedLogger(ILogger logger)
        {
            this.Logger = logger;
        }

        public void Write(IEvent @event)
        {
            if (@event is TEvent)
                this.Logger.Write(@event);
        }
    }
}
