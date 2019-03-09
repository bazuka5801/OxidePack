using System.Collections.Generic;

namespace OxidePack.CoreLib
{
    public static class DictionaryEx
    {
        public static void AddRange<TKey, TValue>(this Dictionary<TKey, TValue> src, Dictionary<TKey, TValue> added)
        {
            foreach (var kvp in added)
            {
                src.Add(kvp.Key, kvp.Value);
            }
        }
    }
}