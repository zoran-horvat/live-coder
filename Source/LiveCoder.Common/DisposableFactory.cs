using System;

namespace LiveCoder.Common
{
    public class DisposableFactory<T> where T : IDisposable
    {
        private Func<T> Factory { get; }
     
        public DisposableFactory(Func<T> factory)
        {
            this.Factory = factory;
        }

        public void Do(Action<T> action)
        {
            using (T obj = this.Factory())
            {
                action(obj);
            }
        }

        public TResult Map<TResult>(Func<T, TResult> map)
        {
            using (T obj = this.Factory())
            {
                return map(obj);
            }
        }
    }
}