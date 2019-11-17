using System;

namespace LiveCoder.Common
{
    public static class Disposable
    {
        public static DisposableFactory<T> Using<T>(Func<T> factory) where T : IDisposable =>
            new DisposableFactory<T>(factory);
    }
}
