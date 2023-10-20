#if NET6_0_OR_GREATER

namespace IT.Collections;

internal class UnorderedPriorityQueue<TElement, TPriority> : PriorityQueue<TElement, TPriority>,
    IReadOnlyCollection<(TElement Element, TPriority Priority)>, ICollection
{
    public UnorderedPriorityQueue(IComparer<TPriority>? comparer = null)
        : base(comparer)
    {
    }

    public UnorderedPriorityQueue(int capacity, IComparer<TPriority>? comparer = null)
        : base(capacity == 0 ? throw new ArgumentOutOfRangeException(nameof(capacity), capacity, "capacity is zero") : capacity, comparer)
    {
    }

    #region IReadOnlyCollection

    public IEnumerator<(TElement Element, TPriority Priority)> GetEnumerator()
        => UnorderedItems.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => UnorderedItems.GetEnumerator();

    #endregion

    #region ICollection

    bool ICollection.IsSynchronized => ((ICollection)UnorderedItems).IsSynchronized;

    object ICollection.SyncRoot => ((ICollection)UnorderedItems).SyncRoot;

    void ICollection.CopyTo(Array array, int index)
        => ((ICollection)UnorderedItems).CopyTo(array, index);

    #endregion
}

#endif