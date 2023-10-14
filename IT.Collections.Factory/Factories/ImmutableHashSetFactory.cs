#if NETCOREAPP3_1_OR_GREATER

using System.Collections.Immutable;

namespace IT.Collections.Factory.Factories;

public class ImmutableHashSetFactory : IEnumerableFactory
{
    public static readonly ImmutableHashSetFactory Default = new();

    public EnumerableType Type => EnumerableType.Ordered | EnumerableType.Unique;

    public IEnumerable<T> Empty<T>() => ImmutableHashSet<T>.Empty;

    public IEnumerable<T> New<T>(int capacity) => ImmutableHashSet<T>.Empty;

    public IEnumerable<T> New<T>(int capacity, EnumerableBuilder<T> builder)
    {
        if (capacity == 0) return ImmutableHashSet<T>.Empty;
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var hashSetBuilder = ImmutableHashSet<T>.Empty.ToBuilder();

        builder(hashSetBuilder.Add);

        return hashSetBuilder.ToImmutable();
    }

    public IEnumerable<T> New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state)
    {
        if (capacity == 0) return ImmutableHashSet<T>.Empty;
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var hashSetBuilder = ImmutableHashSet<T>.Empty.ToBuilder();

        builder(hashSetBuilder.Add, in state);

        return hashSetBuilder.ToImmutable();
    }
}

#endif