namespace IT.Collections.Factory.Tests.Internal;

internal static class xIEnumerable
{
    public static bool TryGetCount<T>(this IEnumerable<T> enumerable, out int count)
    {
        if (enumerable is IReadOnlyCollection<T> readOnlyCollection)
        {
            count = readOnlyCollection.Count;
            return true;
        }
#if NET6_0_OR_GREATER
        if (enumerable.TryGetNonEnumeratedCount(out count)) return true;
        if (enumerable is System.Collections.Immutable.ImmutableArray<T> immutableArray)
        {
            count = immutableArray.Length;
            return true;
        }
#else
        if (enumerable is ICollection<T> collectionGeneric)
        {
            count = collectionGeneric.Count;
            return true;
        }
        if (enumerable is System.Collections.ICollection collection)
        {
            count = collection.Count;
            return true;
        }
#endif
        count = -1;
        return false;
    }

    public static bool TryGetCapacity<T>(this IEnumerable<T> enumerable, out int capacity)
    {
        if (enumerable is List<T> list)
        {
            capacity = list.Capacity;
            return true;
        }
#if NET6_0_OR_GREATER
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
        if (enumerable.TryGetCapacity<KeyValuePair<TKey, TValue>>(out capacity)) return true;
        if (enumerable is SortedList<TKey, TValue> sortedList)
        {
            capacity = sortedList.Capacity;
            return true;
        }
#if NET6_0_OR_GREATER
        if (enumerable is Dictionary<TKey, TValue> dictionary)
        {
            capacity = dictionary.EnsureCapacity(0);
            return true;
        }
#endif
        return false;
    }

    public static bool TryGetCapacity<TKey, TValue>(this IEnumerable<(TKey, TValue)> enumerable, out int capacity)
        where TKey : notnull
    {
        if (enumerable.TryGetCapacity<(TKey, TValue)>(out capacity)) return true;
#if NET6_0_OR_GREATER
        if (enumerable is PriorityQueue<TKey, TValue> priorityQueue)
        {
            capacity = priorityQueue.EnsureCapacity(0);
            return true;
        }
#endif
        return false;
    }
}