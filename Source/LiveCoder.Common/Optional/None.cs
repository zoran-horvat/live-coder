using System;

namespace LiveCoder.Common.Optional
{
    public class None<T> : Option<T>
    {
        public None() { }

        public override Option<TNew> Map<TNew>(Func<T, TNew> map) => new None<TNew>();

        public override Option<TNew> MapNullable<TNew>(Func<T, TNew> map) => new None<TNew>();
        public override Option<TNew> MapOptional<TNew>(Func<T, Option<TNew>> map) => new None<TNew>();

        public override T Reduce(T whenNone) => whenNone;
        public override T Reduce(Func<T> whenNone) => whenNone();
        public override Option<TNew> Reverse<TNew>(TNew whenNone) => new Some<TNew>(whenNone);

        public override Option<T> When(Func<T, bool> predicate) => this;

        public override Option<TNew> OfType<TNew>() => None.Value;

        public override void Do(Action<T> action) { }
        public override void Do(Action<T> action, Action orElse) => orElse();

        public override void Dispose() { }

        public static None<T> Value { get; } = new None<T>();
    }

    public class None
    {
        private None() { }

        public static None Value { get; } = new None();
    }
}