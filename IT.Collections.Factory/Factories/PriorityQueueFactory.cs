#if NET6_0_OR_GREATER

namespace IT.Collections.Factory.Factories;

internal class PriorityQueueFactory : IEnumerableKeyValueTupleFactory
{
    public static readonly PriorityQueueFactory Default = new();

    public Type EnumerableType => typeof(PriorityQueue<,>);

    public EnumerableKind Kind => EnumerableKind.ComparableValue;

    public PriorityQueue<TElement, TPriority> Empty<TElement, TPriority>(in Comparers<TElement, TPriority> comparers = default)
        => new(comparers.ValueComparer);

    public PriorityQueue<TElement, TPriority> New<TElement, TPriority>(int capacity, in Comparers<TElement, TPriority> comparers = default)
        => new(capacity, comparers.ValueComparer);

    public PriorityQueue<TElement, TPriority> New<TElement, TPriority>(int capacity, EnumerableBuilder<(TElement, TPriority)> builder, in Comparers<TElement, TPriority> comparers = default)
    {
        if (capacity == 0) return new(comparers.ValueComparer);
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var priorityQueue = new PriorityQueue<TElement, TPriority>(capacity, comparers.ValueComparer);

        builder(item => { priorityQueue.Enqueue(item.Item1, item.Item2); return true; });

        return priorityQueue;
    }

    public PriorityQueue<TElement, TPriority> New<TElement, TPriority, TState>(int capacity, EnumerableBuilder<(TElement, TPriority), TState> builder, in TState state, in Comparers<TElement, TPriority> comparers = default)
    {
        if (capacity == 0) return new(comparers.ValueComparer);
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var priorityQueue = new PriorityQueue<TElement, TPriority>(capacity, comparers.ValueComparer);

        builder(item => { priorityQueue.Enqueue(item.Item1, item.Item2); return true; }, in state);

        return priorityQueue;
    }

    IEnumerable<(TKey, TValue)> IEnumerableKeyValueTupleFactory.Empty<TKey, TValue>(in Comparers<TKey, TValue> comparers) => Empty(in comparers).UnorderedItems;
    IEnumerable<(TKey, TValue)> IEnumerableKeyValueTupleFactory.New<TKey, TValue>(int capacity, in Comparers<TKey, TValue> comparers) => New(capacity, in comparers).UnorderedItems;
    IEnumerable<(TKey, TValue)> IEnumerableKeyValueTupleFactory.New<TKey, TValue>(int capacity, EnumerableBuilder<(TKey, TValue)> builder, in Comparers<TKey, TValue> comparers) => New(capacity, builder, in comparers).UnorderedItems;
    IEnumerable<(TKey, TValue)> IEnumerableKeyValueTupleFactory.New<TKey, TValue, TState>(int capacity, EnumerableBuilder<(TKey, TValue), TState> builder, in TState state, in Comparers<TKey, TValue> comparers) => New(capacity, builder, in state, in comparers).UnorderedItems;
}

#endif