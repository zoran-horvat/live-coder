using System;
using LiveCoder.Common.Optional;

namespace LiveCoder.Common
{
    public static class Disposable
    {
        public static DisposableFactory<T> Using<T>(Func<T> factory) where T : IDisposable =>
            new DisposableFactory<T>(factory);

        public static OptionalDisposableFactory<T> Using<T>(Func<Option<T>> factory) where T : IDisposable =>
            new OptionalDisposableFactory<T>(factory);
    }
}
