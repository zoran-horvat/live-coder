﻿using System.Collections.Generic;

namespace LiveCoder.Common.Optional
{
    public static class DictionaryExtensions
    {
        public static Option<TValue> TryGetValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key) =>
            dictionary.TryGetValue(key, out TValue value) ? (Option<TValue>)value : None.Value;
    }
}
