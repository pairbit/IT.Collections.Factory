#if NETCOREAPP3_1_OR_GREATER

using System.Collections.Immutable;

namespace IT.Collections.Factory.Factories;

public class ImmutableSortedSetFactory : IEnumerableFactory
{
    public static readonly ImmutableSortedSetFactory Default = new();

    public EnumerableType Type => EnumerableType.Ordered | EnumerableType.Unique;

    public IEnumerable<T> Empty<T>() => ImmutableSortedSet<T>.Empty;

    public IEnumerable<T> New<T>(int capacity) => ImmutableSortedSet<T>.Empty;

    public IEnumerable<T> New<T>(int capacity, EnumerableBuilder<T> builder)
    {
        if (capacity == 0) return ImmutableSortedSet<T>.Empty;
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var sortedSetBuilder = ImmutableSortedSet<T>.Empty.ToBuilder();

        builder(sortedSetBuilder.Add, false);

        return sortedSetBuilder.ToImmutable();
    }

    public IEnumerable<T> New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state)
    {
        if (capacity == 0) return ImmutableSortedSet<T>.Empty;
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var sortedSetBuilder = ImmutableSortedSet<T>.Empty.ToBuilder();

        builder(sortedSetBuilder.Add, false, in state);

        return sortedSetBuilder.ToImmutable();
    }
}

#endif