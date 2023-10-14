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
        EnumerableType Type => EnumerableType.Ordered | EnumerableType.Unique;

    public
#if NET5_0_OR_GREATER
        override
#endif
        ImmutableHashSet<T> Empty<T>() => ImmutableHashSet<T>.Empty;

    public
#if NET5_0_OR_GREATER
        override
#endif
        ImmutableHashSet<T> New<T>(int capacity) => ImmutableHashSet<T>.Empty;

    public
#if NET5_0_OR_GREATER
        override
#endif
        ImmutableHashSet<T> New<T>(int capacity, EnumerableBuilder<T> builder)
    {
        if (capacity == 0) return ImmutableHashSet<T>.Empty;
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var hashSetBuilder = ImmutableHashSet<T>.Empty.ToBuilder();

        builder(hashSetBuilder.Add);

        return hashSetBuilder.ToImmutable();
    }

    public
#if NET5_0_OR_GREATER
        override
#endif
        ImmutableHashSet<T> New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state)
    {
        if (capacity == 0) return ImmutableHashSet<T>.Empty;
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var hashSetBuilder = ImmutableHashSet<T>.Empty.ToBuilder();

        builder(hashSetBuilder.Add, in state);

        return hashSetBuilder.ToImmutable();
    }
#if !NET5_0_OR_GREATER
    IEnumerable<T> IEnumerableFactory.Empty<T>() => Empty<T>();
    IEnumerable<T> IEnumerableFactory.New<T>(int capacity) => New<T>(capacity);
    IEnumerable<T> IEnumerableFactory.New<T>(int capacity, EnumerableBuilder<T> builder) => New(capacity, builder);
    IEnumerable<T> IEnumerableFactory.New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state) => New(capacity, builder, in state);
#endif
}

#endif