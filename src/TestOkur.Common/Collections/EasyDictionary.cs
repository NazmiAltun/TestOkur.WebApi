namespace TestOkur.Common.Collections
{
    using System;
    using System.Collections.Generic;

    public class EasyDictionary<TKey, TValue>
    {
        private readonly Func<TValue, TKey> _keySelector;
        private readonly Dictionary<TKey, TValue> _dictionary;

        public EasyDictionary()
        {
            _dictionary = new Dictionary<TKey, TValue>();
        }

        public EasyDictionary(Func<TValue, TKey> keySelector, IEnumerable<TValue> sequence)
            : this()
        {
            _keySelector = keySelector;

            foreach (var value in sequence)
            {
                _dictionary.Add(_keySelector(value), value);
            }
        }

        public TValue this[TKey key] => _dictionary[key];
    }
}
