namespace TestOkur.Common.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using TestOkur.Common.Collections;

    public static class EnumerableExtensions
    {
        [DebuggerStepThrough]
        public static EasyDictionary<TKey, TValue> ToEasyDictionary<TKey, TValue>(
            this IEnumerable<TValue> sequence, Func<TValue, TKey> keySelector)
            => new EasyDictionary<TKey, TValue>(keySelector, sequence);
    }
}
