using System;

namespace LiveCoder.Common.Optional
{
    public abstract class Option<T> : IDisposable
    {
        public abstract Option<TNew> Map<TNew>(Func<T, TNew> map);
        public abstract Option<TNew> MapNullable<TNew>(Func<T, TNew> map);
        public abstract Option<TNew> MapOptional<TNew>(Func<T, Option<TNew>> map);

        public abstract T Reduce(T whenNone);
        public abstract T Reduce(Func<T> whenNone);
        public abstract Option<TNew> Reverse<TNew>(TNew whenNone);
        public abstract Option<T> When(Func<T, bool> predicate);
        public abstract Option<TNew> OfType<TNew>() where TNew : T;
        public abstract void Do(Action<T> action);
        public abstract void Do(Action<T> action, Action orElse);

        public static implicit operator Option<T>(T value) => (Some<T>)value;
        public static implicit operator Option<T>(None none) => None<T>.Value;

        public abstract void Dispose();
    }

    public static class Option
    {
        public static Option<T> FromNullable<T>(this T value) where T : class =>
            object.ReferenceEquals(value, null) ? (Option<T>)Optional.None.Value : new Some<T>(value);

        public static Option<T> Of<T>(T obj) =>
            new Some<T>(obj);

        public static Option<T> None<T>() =>
            new None<T>();
    }
}
