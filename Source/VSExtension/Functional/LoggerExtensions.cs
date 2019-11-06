using System;
using LiveCoderExtension.Interfaces;

namespace LiveCoderExtension.Functional
{
    static class LoggerExtensions
    {
        public static T LogAndReturn<T>(this ILogger logger, Func<T, IEvent> eventFactory, T obj)
        {
            IEvent @event = eventFactory(obj);
            logger.Write(@event);
            return obj;
        }
    }
}
