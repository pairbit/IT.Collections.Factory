﻿#if NETCOREAPP3_1_OR_GREATER

using System.Collections.Immutable;

namespace IT.Collections.Factory.Factories;

public class ImmutableListFactory : IEnumerableFactory
{
    public static readonly ImmutableListFactory Default = new();

    public IEnumerable<T> Empty<T>() => ImmutableList<T>.Empty;

    public IEnumerable<T> New<T, TState>(int capacity, in TState state, EnumerableBuilder<T, TState> builder)
    {
        if (capacity == 0) return ImmutableList<T>.Empty;

        var array = new T[capacity];

        builder(array, in state);

        return ImmutableList<T>.Empty.AddRange(array);
    }
}

#endif