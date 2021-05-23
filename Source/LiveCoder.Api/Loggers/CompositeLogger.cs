using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace LiveCoder.Api.Loggers
{
    class CompositeLogger : ILogger
    {
        private IEnumerable<ILogger> Loggers { get; }

        public CompositeLogger(params ILogger[] loggers)
        {
            this.Loggers = loggers;
        }

        public void Write(IEvent @event)
        {
            foreach (ILogger logger in this.Loggers)
                logger.Write(@event);
        }

        public ILogger Add(ILogger other) =>
            new CompositeLogger(this.Loggers.Concat(new[] {other}).ToArray());
    }
}
