using System;
using LiveCoder.Common.Optional;

namespace LiveCoder.Common
{
    public class OptionalDisposableFactory<T> where T : IDisposable
    {
        private Func<Option<T>> Factory { get; }

        public OptionalDisposableFactory(Func<Option<T>> factory)
        {
            this.Factory = factory;
        }

        public Option<TResult> TryMap<TResult>(Func<T, TResult> map)
        {
            Option<T> target = Option.None<T>();
            try
            {
                target = this.Factory();
                return target.Map(map);
            }
            finally
            {
                if (target is Some<T> some && some.Content is T content)
                    content.Dispose();
            }
        }
    }
}