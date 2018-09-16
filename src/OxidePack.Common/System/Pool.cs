using System;
using System.Collections.Generic;
using System.IO;

namespace OxidePack
{
    public static class Pool
    {
        public static Dictionary<Type, object> directory = new Dictionary<Type, object>();

        private static int MaxPoolSize = 131072;
        
        #region [Method] Clear
        public static void Clear()
        {
            directory.Clear();
            directory = new Dictionary<Type, object>();
        }
        #endregion

        #region [Method] FillBuffer<T>
        public static void FillBuffer<T>(int count = 2147483647)
            where T : class, new()
        {
            var poolCollection = FindCollection<T>();
            var num = 0;
            while (num < count)
            {
                if (poolCollection.ItemsInStack < poolCollection.buffer.Length)
                {
                    poolCollection.buffer[checked((int)poolCollection.ItemsInStack)] = Activator.CreateInstance<T>();
                    var itemsInStack = poolCollection;
                    itemsInStack.ItemsInStack = itemsInStack.ItemsInStack + 1;
                    num++;
                }
                else
                {
                    break;
                }
            }
        }
        #endregion

        #region [Method] Get<T>
        public static T Get<T>()
            where T : class, new()
        {
            var poolCollection = FindCollection<T>();
            if (poolCollection.ItemsInStack <= 0)
            {
                poolCollection.ItemsCreated++;
                poolCollection.ItemsInUse++;
                return Activator.CreateInstance<T>();
            }

            poolCollection.ItemsInStack--;
            poolCollection.ItemsInUse++;
            var instance = poolCollection.buffer[checked((int) poolCollection.ItemsInStack)];
            poolCollection.buffer[checked((int) poolCollection.ItemsInStack)] = null;
            if ((object) instance is IPooled pooled)
            {
                pooled.LeavePool();
            }

            var itemsTaken = poolCollection;
            itemsTaken.ItemsTaken = itemsTaken.ItemsTaken + 1;
            return instance;
        }
        #endregion

        #region [Method] GetList<T>
        public static List<T> GetList<T>()
        {
            return Get<List<T>>();
        }
        #endregion
        
        #region [Method] Free<T>
        public static void Free<T>(ref T obj)
            where T : class
        {
            if (obj == null)
            {
                throw new ArgumentNullException();
            }
            var poolCollection = FindCollection<T>();
            if (poolCollection.buffer == null)
            {
                ResizeBuffer<T>(2);
            }
            if (poolCollection.ItemsInStack >= poolCollection.buffer.Length)
            {
                ResizeBuffer<T>();
//                if (poolCollection.buffer.Length == MaxPoolSize)
//                {
//                    poolCollection.ItemsSpilled++;
//                    poolCollection.ItemsInUse--;
//                    obj = null;
//                    return;
//                }
            }
            poolCollection.buffer[checked((int)poolCollection.ItemsInStack)] = obj;
            poolCollection.ItemsInStack++;
            poolCollection.ItemsInUse--;
            if ((object)obj is IPooled pooled)
            {
                pooled.EnterPool();
            }
            obj = null;
        }
        #endregion

        #region [Method] FreeList<T>
        public static void FreeList<T>(ref List<T> obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException();
            }
            obj.Clear();
            Free(ref obj);
            if (obj != null)
            {
                throw new SystemException("Pool.Free is not setting object to NULL");
            }
        }
        #endregion

        #region [Method] FreeMemoryStream
        public static void FreeMemoryStream(ref MemoryStream mStream)
        {
            if (mStream == null)
            {
                throw new ArgumentNullException();
            }

            mStream.Position = 0;
            mStream.SetLength(0);
            Free(ref mStream);
            if (mStream != null)
            {
                throw new SystemException("Pool.Free is not setting object to NULL");
            }
        }
        #endregion
        
        #region [Method] FindCollection
        public static PoolCollection<T> FindCollection<T>()
        {
            if (!directory.TryGetValue(typeof(T), out var poolCollection))
            {
                poolCollection = new PoolCollection<T>();
                directory.Add(typeof(T), poolCollection);
            }
            return (PoolCollection<T>)poolCollection;
        }
        #endregion

        #region [Method] ResizeBuffer
        public static void ResizeBuffer<T>(int size = -1)
        {
            Pool.PoolCollection<T> poolCollection = Pool.FindCollection<T>();
            if (size == -1)
            {
                size = poolCollection.buffer?.Length * 2 ?? 2;
            }
            Array.Resize<T>(ref poolCollection.buffer, size);
        }
        #endregion

        #region [Interface] ICollection
        public interface ICollection
        {
            long ItemsCreated { get; }
            long ItemsInStack { get; }
            long ItemsInUse   { get; }
            long ItemsSpilled { get; }
            long ItemsTaken   { get; }
        }
        #endregion

        #region [Interface] IPolled
        public interface IPooled
        {
            void EnterPool();
            void LeavePool();
        }
        #endregion
        #region [Class] PoolCollection<T> : ICollection
        public class PoolCollection<T> : ICollection
        {
            public T[] buffer;

            public long ItemsCreated { get; set; }
            public long ItemsInStack { get; set; }
            public long ItemsInUse { get; set; }
            public long ItemsSpilled { get; set; }
            public long ItemsTaken { get; set; }
        }
        #endregion
    }
}