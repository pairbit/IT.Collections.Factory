#if NET6_0_OR_GREATER

namespace IT.Collections.Factory.Factories;

internal class UnorderedPriorityQueueFactory : IEnumerableKeyValueTupleFactory
{
    public static readonly UnorderedPriorityQueueFactory Default = new();

    public Type EnumerableType => typeof(UnorderedPriorityQueue<,>);

    public EnumerableKind Kind => EnumerableKind.ComparableValue;

    public UnorderedPriorityQueue<TElement, TPriority> Empty<TElement, TPriority>(in Comparers<TElement, TPriority> comparers = default)
        => new(comparers.ValueComparer);

    public UnorderedPriorityQueue<TElement, TPriority> New<TElement, TPriority>(int capacity, in Comparers<TElement, TPriority> comparers = default)
        => capacity == 0 ? new(comparers.ValueComparer) : new(capacity, comparers.ValueComparer);

    public UnorderedPriorityQueue<TElement, TPriority> New<TElement, TPriority>(int capacity, EnumerableBuilder<(TElement, TPriority)> builder, in Comparers<TElement, TPriority> comparers = default)
    {
        if (capacity == 0) return new(comparers.ValueComparer);
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var priorityQueue = new UnorderedPriorityQueue<TElement, TPriority>(capacity, comparers.ValueComparer);

        builder(item => { priorityQueue.Enqueue(item.Item1, item.Item2); return true; });

        return priorityQueue;
    }

    public UnorderedPriorityQueue<TElement, TPriority> New<TElement, TPriority, TState>(int capacity, EnumerableBuilder<(TElement, TPriority), TState> builder, in TState state, in Comparers<TElement, TPriority> comparers = default)
    {
        if (capacity == 0) return new(comparers.ValueComparer);
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var priorityQueue = new UnorderedPriorityQueue<TElement, TPriority>(capacity, comparers.ValueComparer);

        builder(item => { priorityQueue.Enqueue(item.Item1, item.Item2); return true; }, in state);

        return priorityQueue;
    }

    IEnumerable<(TKey, TValue)> IEnumerableKeyValueTupleFactory.Empty<TKey, TValue>(in Comparers<TKey, TValue> comparers) => Empty(in comparers);
    IEnumerable<(TKey, TValue)> IEnumerableKeyValueTupleFactory.New<TKey, TValue>(int capacity, in Comparers<TKey, TValue> comparers) => New(capacity, in comparers);
    IEnumerable<(TKey, TValue)> IEnumerableKeyValueTupleFactory.New<TKey, TValue>(int capacity, EnumerableBuilder<(TKey, TValue)> builder, in Comparers<TKey, TValue> comparers) => New(capacity, builder, in comparers);
    IEnumerable<(TKey, TValue)> IEnumerableKeyValueTupleFactory.New<TKey, TValue, TState>(int capacity, EnumerableBuilder<(TKey, TValue), TState> builder, in TState state, in Comparers<TKey, TValue> comparers) => New(capacity, builder, in state, in comparers);
}

#endif