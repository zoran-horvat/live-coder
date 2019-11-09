namespace LiveCoder.Common.Optional
{
    public static class TypeExtensions
    {
        public static Option<T> OfType<T>(this object obj) where T : class =>
            obj is T derived
                ? (Option<T>)new Some<T>(derived)
                : new None<T>();

    }
}
