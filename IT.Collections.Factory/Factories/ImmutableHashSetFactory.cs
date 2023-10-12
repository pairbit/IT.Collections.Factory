#if NETCOREAPP3_1_OR_GREATER

using System.Collections.Immutable;

namespace IT.Collections.Factory.Factories;

public class ImmutableHashSetFactory : IEnumerableFactory
{
    public static readonly ImmutableHashSetFactory Default = new();

    public bool IsReadOnly => false;

    public IEnumerable<T> Empty<T>() => ImmutableHashSet<T>.Empty;

    public IEnumerable<T> New<T>(int capacity)
    {
        if (capacity == 0) return ImmutableHashSet<T>.Empty;

        return ImmutableHashSet<T>.Empty.Union(new T[capacity]);
    }

    public IEnumerable<T> New<T>(int capacity, EnumerableBuilder<T> builder)
    {
        if (capacity == 0) return ImmutableHashSet<T>.Empty;
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var array = new T[capacity];

        builder(array);

        return ImmutableHashSet<T>.Empty.Union(array);
    }

    public IEnumerable<T> New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state)
    {
        if (capacity == 0) return ImmutableHashSet<T>.Empty;
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var array = new T[capacity];

        builder(array, in state);

        return ImmutableHashSet<T>.Empty.Union(array);
    }
}

#endif