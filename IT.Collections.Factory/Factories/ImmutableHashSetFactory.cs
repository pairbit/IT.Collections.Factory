#if NETCOREAPP3_1_OR_GREATER

using System.Collections.Immutable;

namespace IT.Collections.Factory.Factories;

public class ImmutableHashSetFactory :
#if NET5_0_OR_GREATER
    EnumerableFactory
#else
    IEnumerableFactory
#endif
{
    public static readonly ImmutableHashSetFactory Default = new();

    public
#if NET5_0_OR_GREATER
        override
#endif
        EnumerableType Type => EnumerableType.Ordered | EnumerableType.Unique | EnumerableType.Equatable;

    public
#if NET5_0_OR_GREATER
        override
#endif
        ImmutableHashSet<T> Empty<T>(in Comparers<T> comparers = default)
            => ImmutableHashSet<T>.Empty.WithComparer(comparers.EqualityComparer);

    public
#if NET5_0_OR_GREATER
        override
#endif
        ImmutableHashSet<T> New<T>(int capacity, in Comparers<T> comparers = default)
            => ImmutableHashSet<T>.Empty.WithComparer(comparers.EqualityComparer);

    public
#if NET5_0_OR_GREATER
        override
#endif
        ImmutableHashSet<T> New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers = default)
    {
        if (capacity == 0) return ImmutableHashSet<T>.Empty.WithComparer(comparers.EqualityComparer);
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var hashSetBuilder = ImmutableHashSet<T>.Empty.ToBuilder();
        var equalityComparer = comparers.EqualityComparer;
        if (equalityComparer != null) hashSetBuilder.KeyComparer = equalityComparer;

        builder(hashSetBuilder.Add);

        return hashSetBuilder.ToImmutable();
    }

    public
#if NET5_0_OR_GREATER
        override
#endif
        ImmutableHashSet<T> New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers = default)
    {
        if (capacity == 0) return ImmutableHashSet<T>.Empty.WithComparer(comparers.EqualityComparer);
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var hashSetBuilder = ImmutableHashSet<T>.Empty.ToBuilder();
        var equalityComparer = comparers.EqualityComparer;
        if (equalityComparer != null) hashSetBuilder.KeyComparer = equalityComparer;

        builder(hashSetBuilder.Add, in state);

        return hashSetBuilder.ToImmutable();
    }
#if !NET5_0_OR_GREATER
    IEnumerable<T> IEnumerableFactory.Empty<T>(in Comparers<T> comparers) => Empty(in comparers);
    IEnumerable<T> IEnumerableFactory.New<T>(int capacity, in Comparers<T> comparers) => New(capacity, in comparers);
    IEnumerable<T> IEnumerableFactory.New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers) => New(capacity, builder, in comparers);
    IEnumerable<T> IEnumerableFactory.New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers) => New(capacity, builder, in state, in comparers);
#endif
}

#endif