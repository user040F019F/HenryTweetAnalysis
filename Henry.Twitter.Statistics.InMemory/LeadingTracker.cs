using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Henry.Twitter.Statistics.InMemory
{
    class LeadingTracker<TValue>
    {
        Dictionary<TValue, int> Counters =
            new Dictionary<TValue, int>();
        SortedDictionary<int, ISet<TValue>> _Leaders =
            new SortedDictionary<int, ISet<TValue>>();
        object mutex = new object();
        public IReadOnlyDictionary<int, ISet<TValue>> Leaders { get => _Leaders; }

        public void Increment(TValue value, int count)
        {
            lock (mutex)
            {
                Counters.TryGetValue(value, out var current);
                if (_Leaders.TryGetValue(current, out var set))
                {
                    set.Remove(value);
                    if (set.Count == 0) _Leaders.Remove(current);
                }
                current += count;
                Counters[value] = current;
                _Leaders.TryAdd(current, new HashSet<TValue>());
                _Leaders[current].Add(value);
            }
        }

        public TValue[] AsArray(Func<IEnumerable<TValue>, IEnumerable<TValue>> alter)
        {
            lock (mutex)
            {
                return alter(Leaders.SelectMany(x => x.Value)).ToArray();
            }
        }
    }
}
