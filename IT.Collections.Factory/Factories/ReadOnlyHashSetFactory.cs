﻿#if NET6_0_OR_GREATER

namespace IT.Collections.Factory.Factories;

using Internal;

public class ReadOnlyHashSetFactory : IEnumerableFactory
{
    public static readonly ReadOnlyHashSetFactory Default = new();

    public bool IsReadOnly => true;

    public IEnumerable<T> Empty<T>() => ReadOnlySet<T>.Empty;

    public IEnumerable<T> New<T>(int capacity)
    {
        throw new NotSupportedException();
    }

    public IEnumerable<T> New<T>(int capacity, EnumerableBuilder<T> builder)
    {
        if (capacity == 0) return ReadOnlySet<T>.Empty;
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var hashSet = new HashSet<T>(capacity, null);

        builder(hashSet);

        return new ReadOnlySet<T>(hashSet);
    }

    public IEnumerable<T> New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state)
    {
        if (capacity == 0) return ReadOnlySet<T>.Empty;
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var hashSet = new HashSet<T>(capacity, null);

        builder(hashSet, in state);

        return new ReadOnlySet<T>(hashSet);
    }
}

#endif