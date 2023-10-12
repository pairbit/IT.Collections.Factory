#if NETCOREAPP3_1_OR_GREATER

using System.Collections.Immutable;

namespace IT.Collections.Factory.Factories;

public class ImmutableQueueFactory : IEnumerableFactory
{
    public static readonly ImmutableQueueFactory Default = new();

    public bool IsReadOnly => false;

    public IEnumerable<T> Empty<T>() => ImmutableQueue<T>.Empty;

    public IEnumerable<T> New<T>(int capacity)
    {
        if (capacity == 0) return ImmutableQueue<T>.Empty;

        return ImmutableQueue.Create(new T[capacity]);
    }

    public IEnumerable<T> New<T>(int capacity, EnumerableBuilder<T> builder)
    {
        if (capacity == 0) return ImmutableQueue<T>.Empty;
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var array = new T[capacity];

        builder(array);

        return ImmutableQueue.Create(array);
    }

    public IEnumerable<T> New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state)
    {
        if (capacity == 0) return ImmutableQueue<T>.Empty;
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var array = new T[capacity];

        builder(array, in state);

        return ImmutableQueue.Create(array);
    }
}

#endif