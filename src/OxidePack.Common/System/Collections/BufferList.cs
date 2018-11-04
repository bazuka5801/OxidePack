using System;
using System.Collections;
using System.Collections.Generic;

namespace OxidePack.Collections
{
    public class BufferList<T> : IEnumerable<T>, IEnumerable
    {
        private int count;
        private T[] buffer;

        public T[] Buffer => buffer;
        public int Capacity => buffer.Length;
        public int Count => count;

        public T this[int index]
        {
            get => buffer[index];
            set => buffer[index] = value;
        }

        public BufferList(int capacity = 8)
        {
            buffer = new T[capacity];
        }

        public void Add(T element)
        {
            if (count == buffer.Length)
            {
                Array.Resize(ref buffer, buffer.Length * 2);
            }
            buffer[count] = element;
            count++;
        }

        public void Clear()
        {
            if (count == 0)
            {
                return;
            }
            Array.Clear(buffer, 0, count);
            count = 0;
        }

        public bool Contains(T element)
        {
            return Array.IndexOf(buffer, element) != -1;
        }

        public int IndexOf(T element)
        {
            return Array.IndexOf(buffer, element);
        }

        public int LastIndexOf(T element)
        {
            return Array.LastIndexOf(buffer, element);
        }

        public bool Remove(T element)
        {
            int num = Array.IndexOf(buffer, element);
            if (num == -1)
            {
                return false;
            }
            RemoveAt(num);
            return true;
        }

        public void RemoveAt(int index)
        {
            for (int i = index; i < count - 1; i++)
            {
                buffer[i] = buffer[i + 1];
            }
            buffer[count - 1] = default(T);
            count--;
        }

        public void RemoveUnordered(int index)
        {
            buffer[index] = buffer[count - 1];
            buffer[count - 1] = default(T);
            count--;
        }

        public void Sort()
        {
            Array.Sort(buffer);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < buffer.Length; i++)
            {
                yield return buffer[i];
            }
        }
    }
}