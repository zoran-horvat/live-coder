using System;

namespace VSExtension.Functional
{
    abstract class Option<T>
    {
        public abstract Option<TNew> Map<TNew>(Func<T, TNew> map);
        public abstract T Reduce(Func<T> whenNone);
        public abstract Option<T> When(Func<T, bool> predicate);

        public static implicit operator Option<T>(T value) => (Some<T>)value;
        public static implicit operator Option<T>(None none) => None<T>.Value;
    }
}
