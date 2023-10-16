﻿namespace IT.Collections.Factory.Factories;

public class BlockingCollectionFactory :
#if NET5_0_OR_GREATER
    EnumerableFactory
#else
    IEnumerableFactory
#endif
{
    public static readonly BlockingCollectionFactory Default = new();

    public
#if NET5_0_OR_GREATER
        override
#endif
        EnumerableType Type => EnumerableType.None;

    public
#if NET5_0_OR_GREATER
        override
#endif
        BlockingCollection<T> Empty<T>(in Comparers<T> comparers = default) => new();

    public
#if NET5_0_OR_GREATER
        override
#endif
        BlockingCollection<T> New<T>(int capacity, in Comparers<T> comparers = default)
    {
        if (capacity == 0) return new();

        return new(new ConcurrentQueue<T>(), capacity);
    }

    public
#if NET5_0_OR_GREATER
        override
#endif
        BlockingCollection<T> New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers = default)
    {
        if (capacity == 0) return new();
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var queue = new ConcurrentQueue<T>();

        builder(((IProducerConsumerCollection<T>)queue).TryAdd);

        return new(queue, capacity);
    }

    public
#if NET5_0_OR_GREATER
        override
#endif
        BlockingCollection<T> New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers = default)
    {
        if (capacity == 0) return new();
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var queue = new ConcurrentQueue<T>();

        builder(((IProducerConsumerCollection<T>)queue).TryAdd, in state);

        return new(queue, capacity);
    }
#if !NET5_0_OR_GREATER
    IEnumerable<T> IEnumerableFactory.Empty<T>(in Comparers<T> comparers) => Empty(in comparers);
    IEnumerable<T> IEnumerableFactory.New<T>(int capacity, in Comparers<T> comparers) => New(capacity, in comparers);
    IEnumerable<T> IEnumerableFactory.New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers) => New(capacity, builder, in comparers);
    IEnumerable<T> IEnumerableFactory.New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers) => New(capacity, builder, in state, in comparers);
#endif
}