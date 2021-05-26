using System;
using LiveCoder.Api;

namespace LiveCoder.Snippets.Events
{
    abstract class DelimitedEvent : IEvent
    {
        protected abstract string InnerLabel { get; }

        public string Label =>
            $"{new string('-', 20)}{Environment.NewLine}{this.InnerLabel}";
    }
}