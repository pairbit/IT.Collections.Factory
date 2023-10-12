﻿using System.Collections.Concurrent;

namespace IT.Collections.Factory.Factories;

public class BlockingCollectionFactory : IEnumerableFactory
{
    public static readonly BlockingCollectionFactory Default = new();

    public bool IsReadOnly => false;

    public IEnumerable<T> Empty<T>() => new BlockingCollection<T>();

    public IEnumerable<T> New<T>(int capacity)
    {
        if (capacity == 0) return new BlockingCollection<T>();

        return new BlockingCollection<T>(new ConcurrentQueue<T>(), capacity);
    }

    public IEnumerable<T> New<T>(int capacity, EnumerableBuilder<T> builder)
    {
        if (capacity == 0) return new BlockingCollection<T>();
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var queue = new ConcurrentQueue<T>();

        builder(queue);

        return new BlockingCollection<T>(queue, capacity);
    }

    public IEnumerable<T> New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state)
    {
        if (capacity == 0) return new BlockingCollection<T>();
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var queue = new ConcurrentQueue<T>();

        builder(queue, in state);

        return new BlockingCollection<T>(queue, capacity);
    }
}