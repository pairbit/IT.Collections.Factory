#if NETCOREAPP3_1_OR_GREATER

using System.Collections.Immutable;

namespace IT.Collections.Factory.Factories;

public class ImmutableQueueFactory : IEnumerableFactory
{
    public static readonly ImmutableQueueFactory Default = new();

    public EnumerableType Type => EnumerableType.None;

    public IEnumerable<T> Empty<T>() => ImmutableQueue<T>.Empty;

    public IEnumerable<T> New<T>(int capacity) => ImmutableQueue<T>.Empty;

    public IEnumerable<T> New<T>(int capacity, EnumerableBuilder<T> builder)
    {
        if (capacity == 0) return ImmutableQueue<T>.Empty;
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var queue = ImmutableQueue<T>.Empty;

        builder(item => { queue = queue.Enqueue(item); return true; });

        return queue;
    }

    public IEnumerable<T> New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state)
    {
        if (capacity == 0) return ImmutableQueue<T>.Empty;
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var queue = ImmutableQueue<T>.Empty;

        builder(item => { queue = queue.Enqueue(item); return true; }, in state);

        return queue;
    }
}

#endif