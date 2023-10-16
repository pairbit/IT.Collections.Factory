#if NETCOREAPP3_1_OR_GREATER

using System.Collections.Immutable;

namespace IT.Collections.Factory.Factories;

public class ImmutableQueueFactory :
#if NET5_0_OR_GREATER
    EnumerableFactory
#else
    IEnumerableFactory
#endif
{
    public static readonly ImmutableQueueFactory Default = new();

    public
#if NET5_0_OR_GREATER
        override
#endif
        EnumerableType Type => EnumerableType.None;

    public
#if NET5_0_OR_GREATER
        override
#endif
        ImmutableQueue<T> Empty<T>(in Comparers<T> comparers = default) => ImmutableQueue<T>.Empty;

    public
#if NET5_0_OR_GREATER
        override
#endif
        ImmutableQueue<T> New<T>(int capacity, in Comparers<T> comparers = default) => ImmutableQueue<T>.Empty;

    public
#if NET5_0_OR_GREATER
        override
#endif
        ImmutableQueue<T> New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers = default)
    {
        if (capacity == 0) return ImmutableQueue<T>.Empty;
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var queue = ImmutableQueue<T>.Empty;

        builder(item => { queue = queue.Enqueue(item); return true; });

        return queue;
    }

    public
#if NET5_0_OR_GREATER
        override
#endif
        ImmutableQueue<T> New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers = default)
    {
        if (capacity == 0) return ImmutableQueue<T>.Empty;
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var queue = ImmutableQueue<T>.Empty;

        builder(item => { queue = queue.Enqueue(item); return true; }, in state);

        return queue;
    }
#if !NET5_0_OR_GREATER
    IEnumerable<T> IEnumerableFactory.Empty<T>(in Comparers<T> comparers) => Empty(in comparers);
    IEnumerable<T> IEnumerableFactory.New<T>(int capacity, in Comparers<T> comparers) => New(capacity, in comparers);
    IEnumerable<T> IEnumerableFactory.New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers) => New(capacity, builder, in comparers);
    IEnumerable<T> IEnumerableFactory.New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers) => New(capacity, builder, in state, in comparers);
#endif
}

#endif