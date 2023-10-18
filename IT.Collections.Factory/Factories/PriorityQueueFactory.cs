#if NET6_0_OR_GREATER

namespace IT.Collections.Factory.Factories;

internal class PriorityQueueFactory : IEnumerableFactory
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

    IEnumerable<T> IEnumerableFactory.Empty<T>(in Comparers<T> comparers)
    {
        throw new NotImplementedException();
    }

    IEnumerable<T> IEnumerableFactory.New<T>(int capacity, in Comparers<T> comparers)
    {
        throw new NotImplementedException();
    }

    IEnumerable<T> IEnumerableFactory.New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers)
    {
        throw new NotImplementedException();
    }

    IEnumerable<T> IEnumerableFactory.New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers)
    {
        throw new NotImplementedException();
    }
}

#endif