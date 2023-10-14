namespace System.Collections.Generic;

internal static class xIEnumerable
{
    public static bool TryGetCapacity<T>(this IEnumerable<T> enumerable, out int capacity)
    {
        if (enumerable is T[] array)
        {
            capacity = array.Length;
            return true;
        }
        if (enumerable is List<T> list)
        {
            capacity = list.Capacity;
            return true;
        }
#if NET6_0_OR_GREATER
        if (enumerable is System.Collections.Immutable.ImmutableArray<T> immutableArray)
        {
            capacity = immutableArray.Length;
            return true;
        }
        if (enumerable is Queue<T> queue)
        {
            capacity = queue.EnsureCapacity(0);
            return true;
        }
        if (enumerable is Stack<T> stack)
        {
            capacity = stack.EnsureCapacity(0);
            return true;
        }
        if (enumerable is HashSet<T> hashSet)
        {
            capacity = hashSet.EnsureCapacity(0);
            return true;
        }
#endif
        capacity = -1;
        return false;
    }

    public static bool TryGetCapacity<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> enumerable, out int capacity)
        where TKey : notnull
    {
        if (TryGetCapacity<KeyValuePair<TKey, TValue>>(enumerable, out capacity)) return true;

#if NET6_0_OR_GREATER
        if (enumerable is Dictionary<TKey, TValue> dictionary)
        {
            capacity = dictionary.EnsureCapacity(0);
            return true;
        }
#endif
        return false;
    }
}