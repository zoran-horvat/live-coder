using System;

namespace Common.Optional
{
    public static class OptionalLinq
    {
        public static Option<TResult> Select<T, TResult>(this Option<T> obj, Func<T, TResult> map) =>
            obj.Map(map);

        public static Option<TResult> SelectMany<T, T1, TResult>(this Option<T> obj, Func<T, Option<T1>> map, Func<T, T1, TResult> reduce) =>
            obj.MapOptional(content => map(content).Map(result => reduce(content, result)));
    }
}
