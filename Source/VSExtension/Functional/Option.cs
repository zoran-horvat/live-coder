using System;

namespace VSExtension.Functional
{
    abstract class Option<T>
    {
        public abstract Option<TNew> Map<TNew>(Func<T, TNew> map);
        public abstract T Reduce(T whenNone);
        public abstract T Reduce(Func<T> whenNone);
        public abstract Option<T> When(Func<T, bool> predicate);
        public abstract Option<TNew> OfType<TNew>() where TNew : T;
        public abstract void Do(Action<T> action);

        public static implicit operator Option<T>(T value) => (Some<T>)value;
        public static implicit operator Option<T>(None none) => None<T>.Value;
    }

    static class Option
    {
        public static Option<T> FromNullable<T>(T value) where T : class =>
            object.ReferenceEquals(value, null) ? None.Value : (Option<T>)value;
    }
}
