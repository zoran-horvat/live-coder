using System.Collections.Generic;
using System.Linq;
using LiveCoder.Common.Optional;

namespace LiveCoder.Common
{
    public static class QueueExtensions
    {
        public static Option<TDerived> TryDequeue<T, TDerived>(this Queue<T> queue) where TDerived : T =>
            queue.Any() && queue.Peek() is TDerived && queue.Dequeue() is TDerived derived
                ? Option.Of(derived)
                : Option.None<TDerived>();
    }
}
