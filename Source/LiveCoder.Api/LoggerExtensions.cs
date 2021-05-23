using LiveCoder.Api.Loggers;

namespace LiveCoder.Api
{
    public static class LoggerExtensions
    {
        public static ILogger ForEvents<TEvent>(this ILogger logger) =>
            new TypedLogger<TEvent>(logger);

        public static ILogger And(this ILogger logger, ILogger other) =>
            logger is CompositeLogger composite ? composite.Add(other)
            : new CompositeLogger(logger, other);
    }
}
