using System;

namespace VSExtension.Functional
{
    class Some<T> : Option<T>
    {
        private T Content { get; }

        private Some(T content)
        {
            this.Content = content;
        }

        public override Option<TNew> Map<TNew>(Func<T, TNew> map) => map(this);

        public override T Reduce(T whenNone) => this;
        public override T Reduce(Func<T> whenNone) => this;

        public override Option<T> When(Func<T, bool> predicate) =>
            predicate(this) ? (Option<T>)this : None.Value;

        public override Option<TNew> OfType<TNew>() =>
            this.Content is TNew modified ? (Option<TNew>)modified : None.Value;

        public override void Do(Action<T> action) => action(this);

        public static implicit operator T(Some<T> some) => some.Content;
        public static implicit operator Some<T>(T value) => new Some<T>(value);
    }
}