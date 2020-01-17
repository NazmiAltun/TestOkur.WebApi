using System;
using System.Collections.Generic;
using System.Linq;

namespace TestOkur.Report.Domain.Statistics
{
    internal class SumTable<TItem, TKey>
    {
        private readonly Dictionary<TKey, List<float>> _dictionary;
        private readonly Func<TItem, TKey> _keySelector;
        private readonly Func<TItem, float> _valueSelector;

        public SumTable(Func<TItem, TKey> keySelector, Func<TItem, float> valueSelector)
        {
            _keySelector = keySelector;
            _valueSelector = valueSelector;
            _dictionary = new Dictionary<TKey, List<float>>();
        }

        public IEnumerable<TKey> Keys => _dictionary.Keys;

        public void Add(TItem item)
        {
            var key = _keySelector(item);
            if (!_dictionary.ContainsKey(key))
            {
                _dictionary.Add(key, new List<float>());
            }

            _dictionary[key].Add(_valueSelector(item));
        }

        public Dictionary<TKey, float> ToAverageDictionary() => _dictionary.ToDictionary(x => x.Key, x => x.Value.Average());

        public float GetAverage(TKey key) => _dictionary[key].Average();
    }
}