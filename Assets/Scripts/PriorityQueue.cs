//=============================================================================
//  
// module:		AStar for Unity3d
// license:		GNU GPL
// author:		Chernomoretc Igor
// contacts:	oxnardxvi@gmail.com
//
//=============================================================================
 
using System.Collections.Generic;
using System.Linq;

namespace AStar
{
    /// <summary>
    ///     Priority sorted queue. Item that pushed there will be sorted by priority. 
    /// </summary>
    /// <typeparam name="P">Priority type</typeparam>
    /// <typeparam name="V">Value type</typeparam>
    class PriorityQueue<P, V>
    {
        private readonly SortedDictionary<P, Queue<V>> _items = new SortedDictionary<P, Queue<V>>();
        private readonly HashSet<V> _itemsLookup = new HashSet<V>();

        private int _itemCount;

        public int Count
        {
            get { return _itemCount; }
        }

        public void Enqueue(P priority, V value)
        {
            Queue<V> q;
            if (!_items.TryGetValue(priority, out q))
            {
                q = new Queue<V>();
                _items.Add(priority, q);
            }
            q.Enqueue(value);
            _itemsLookup.Add(value);
            _itemCount++;
        }

        public V Dequeue()
        {
            var pair = _items.First();
            var v = pair.Value.Dequeue();
            _itemsLookup.Remove(v);
            if (pair.Value.Count == 0)
            {
                _items.Remove(pair.Key);
            }
            _itemCount--;
            return v;
        }

        public bool Contains(V value)
        {
            return _itemsLookup.Contains(value);
        }

        public void Clear()
        {
            _itemsLookup.Clear();
            _items.Clear();
            _itemCount = 0;
        }
    }
}
