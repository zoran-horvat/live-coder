﻿using System;

namespace VSExtension.Functional
{
    class None<T> : Option<T>
    {
        private None() { }

        public override Option<TNew> Map<TNew>(Func<T, TNew> map) => None.Value;

        public override T Reduce(T whenNone) => whenNone;
        public override T Reduce(Func<T> whenNone) => whenNone();

        public override Option<T> When(Func<T, bool> predicate) => this;

        public override Option<TNew> OfType<TNew>() => None.Value;

        public override void Do(Action<T> action) { }

        public static None<T> Value { get; } = new None<T>();
    }

    class None
    {
        private None() { }

        public static None Value { get; } = new None();
    }
}