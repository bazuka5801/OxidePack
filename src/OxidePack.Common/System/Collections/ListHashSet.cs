using System.Collections;
using System.Collections.Generic;

namespace OxidePack.Collections
{
    public class ListHashSet<T> : IEnumerable<T>, IEnumerable
    {
        private Dictionary<T, int> val2idx;
        private Dictionary<int, T> idx2val;

        private BufferList<T> vals;

        public int Count => vals.Count;

        public BufferList<T> Values => vals;

        public T this[int index]
        {
            get => vals[index];
            set => vals[index] = value;
        }


        public ListHashSet(int capacity = 8)
        {
            val2idx = new Dictionary<T, int>(capacity);
            idx2val = new Dictionary<int, T>(capacity);
            vals = new BufferList<T>(capacity);
        }

        public void Add(T val)
        {
            int count = vals.Count;
            val2idx.Add(val, count);
            idx2val.Add(count, val);
            vals.Add(val);
        }

        public void AddRange(List<T> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                Add(list[i]);
            }
        }

        public void Clear()
        {
            if (Count == 0)
            {
                return;
            }

            val2idx.Clear();
            idx2val.Clear();
            vals.Clear();
        }

        public bool Contains(T val)
        {
            return val2idx.ContainsKey(val);
        }

        public bool Remove(T val)
        {
            int num;
            if (!val2idx.TryGetValue(val, out num))
            {
                return false;
            }

            Remove(num, val);
            return true;
        }

        private void Remove(int idx_remove, T val_remove)
        {
            int count = vals.Count - 1;
            T item = idx2val[count];
            vals.RemoveUnordered(idx_remove);
            val2idx[item] = idx_remove;
            idx2val[idx_remove] = item;
            val2idx.Remove(val_remove);
            idx2val.Remove(count);
        }

        public bool RemoveAt(int idx)
        {
            T t;
            if (!idx2val.TryGetValue(idx, out t))
            {
                return false;
            }

            Remove(idx, t);
            return true;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<T> GetEnumerator()
        {
            for (var i = 0; i < vals.Count; i++)
            {
                yield return vals[i];
            }
        }
    }
}