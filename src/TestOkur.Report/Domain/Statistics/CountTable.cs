using System.Collections.Generic;

namespace TestOkur.Report.Domain.Statistics
{
    internal class CountTable
    {
        public CountTable()
        {
            Dictionary = new Dictionary<int, int>();
        }

        public Dictionary<int, int> Dictionary { get; }

        public void Increment(int key)
        {
            if (!Dictionary.TryAdd(key, 1))
            {
                Dictionary[key]++;
            }
        }
    }
}