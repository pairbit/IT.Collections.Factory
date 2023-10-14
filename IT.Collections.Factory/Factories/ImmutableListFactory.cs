#if NETCOREAPP3_1_OR_GREATER

using System.Collections.Immutable;

namespace IT.Collections.Factory.Factories;

public class ImmutableListFactory :
#if NET5_0_OR_GREATER
    EnumerableFactory
#else
    IEnumerableFactory
#endif
{
    public static readonly ImmutableListFactory Default = new();

    public
#if NET5_0_OR_GREATER
        override
#endif
        EnumerableType Type => EnumerableType.None;

    public
#if NET5_0_OR_GREATER
        override
#endif
        ImmutableList<T> Empty<T>() => ImmutableList<T>.Empty;

    public
#if NET5_0_OR_GREATER
        override
#endif
        ImmutableList<T> New<T>(int capacity) => ImmutableList<T>.Empty;

    public
#if NET5_0_OR_GREATER
        override
#endif
        ImmutableList<T> New<T>(int capacity, EnumerableBuilder<T> builder)
    {
        if (capacity == 0) return ImmutableList<T>.Empty;
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var listBuilder = ImmutableList<T>.Empty.ToBuilder();

        builder(item => { listBuilder.Add(item); return true; });

        return listBuilder.ToImmutable();
    }

    public
#if NET5_0_OR_GREATER
        override
#endif
        ImmutableList<T> New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state)
    {
        if (capacity == 0) return ImmutableList<T>.Empty;
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var listBuilder = ImmutableList<T>.Empty.ToBuilder();

        builder(item => { listBuilder.Add(item); return true; }, in state);

        return listBuilder.ToImmutable();
    }
#if !NET5_0_OR_GREATER
    IEnumerable<T> IEnumerableFactory.Empty<T>() => Empty<T>();
    IEnumerable<T> IEnumerableFactory.New<T>(int capacity) => New<T>(capacity);
    IEnumerable<T> IEnumerableFactory.New<T>(int capacity, EnumerableBuilder<T> builder) => New(capacity, builder);
    IEnumerable<T> IEnumerableFactory.New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state) => New(capacity, builder, in state);
#endif
}

#endif