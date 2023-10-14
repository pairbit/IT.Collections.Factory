#if NETCOREAPP3_1_OR_GREATER

using System.Collections.Immutable;

namespace IT.Collections.Factory.Factories;

public class ImmutableSortedSetFactory :
#if NET5_0_OR_GREATER
    EnumerableFactory
#else
    IEnumerableFactory
#endif
{
    public static readonly ImmutableSortedSetFactory Default = new();

    public
#if NET5_0_OR_GREATER
        override
#endif
        EnumerableType Type => EnumerableType.Ordered | EnumerableType.Unique;

    public
#if NET5_0_OR_GREATER
        override
#endif
        ImmutableSortedSet<T> Empty<T>() => ImmutableSortedSet<T>.Empty;

    public
#if NET5_0_OR_GREATER
        override
#endif
        ImmutableSortedSet<T> New<T>(int capacity) => ImmutableSortedSet<T>.Empty;

    public
#if NET5_0_OR_GREATER
        override
#endif
        ImmutableSortedSet<T> New<T>(int capacity, EnumerableBuilder<T> builder)
    {
        if (capacity == 0) return ImmutableSortedSet<T>.Empty;
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var sortedSetBuilder = ImmutableSortedSet<T>.Empty.ToBuilder();

        builder(sortedSetBuilder.Add);

        return sortedSetBuilder.ToImmutable();
    }

    public
#if NET5_0_OR_GREATER
        override
#endif
        ImmutableSortedSet<T> New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state)
    {
        if (capacity == 0) return ImmutableSortedSet<T>.Empty;
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var sortedSetBuilder = ImmutableSortedSet<T>.Empty.ToBuilder();

        builder(sortedSetBuilder.Add, in state);

        return sortedSetBuilder.ToImmutable();
    }
#if !NET5_0_OR_GREATER
    IEnumerable<T> IEnumerableFactory.Empty<T>() => Empty<T>();
    IEnumerable<T> IEnumerableFactory.New<T>(int capacity) => New<T>(capacity);
    IEnumerable<T> IEnumerableFactory.New<T>(int capacity, EnumerableBuilder<T> builder) => New(capacity, builder);
    IEnumerable<T> IEnumerableFactory.New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state) => New(capacity, builder, in state);
#endif
}

#endif