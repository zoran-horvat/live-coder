using System;

namespace Common.Optional
{
    public class Some<T> : Option<T>
    {
        public T Content { get; }
        private bool Disposed { get; set; }

        public Some(T content)
        {
            this.Content = content;
        }

        public override Option<TNew> Map<TNew>(Func<T, TNew> map) => 
            map(this.Content);

        public override Option<TNew> MapNullable<TNew>(Func<T, TNew> map) =>
            map(this.Content) is TNew x 
                ? (Option<TNew>)new Some<TNew>(x) 
                : None.Value;

        public override T Reduce(T whenNone) => this.Content;
        public override T Reduce(Func<T> whenNone) => this.Content;

        public override Option<T> When(Func<T, bool> predicate) =>
            predicate(this) ? (Option<T>)this : None.Value;

        public override Option<TNew> OfType<TNew>() =>
            this.Content is TNew modified ? (Option<TNew>)modified : None.Value;

        public override void Do(Action<T> action) => action(this);

        public override void Dispose() => this.Dispose(true);

        private void Dispose(bool disposing)
        {
            if (disposing && !this.Disposed)
            {
                this.DisposeContent(this.Content as IDisposable);
            }

            GC.SuppressFinalize(this);
        }

        private void DisposeContent(IDisposable value) => value?.Dispose();

        public static implicit operator T(Some<T> some) => some.Content;
        public static implicit operator Some<T>(T value) => new Some<T>(value);
    }
}