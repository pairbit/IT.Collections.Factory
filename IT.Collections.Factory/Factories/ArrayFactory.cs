﻿namespace IT.Collections.Factory.Factories;

using Internal;

public sealed class ArrayFactory : IEnumerableFactory, IEquatable<ArrayFactory>
{
    public Type EnumerableType => typeof(Array);

    public EnumerableKind Kind => EnumerableKind.Fixed;

    public T[] Empty<T>(in Comparers<T> comparers = default) => Array.Empty<T>();

    public T[] New<T>(int capacity, in Comparers<T> comparers = default)
    {
        if (capacity == 0) return Array.Empty<T>();
        if (capacity < 0) throw Ex.ArgumentNegative(capacity, nameof(capacity));

        return new T[capacity];
    }

    public T[] New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers = default)
    {
        if (capacity == 0) return Array.Empty<T>();
        if (builder == null) throw new ArgumentNullException(nameof(builder));
        if (capacity < 0) throw Ex.ArgumentNegative(capacity, nameof(capacity));

        var array = new T[capacity];
        var index = 0;

        builder(item => { array[index++] = item; return true; });

        return array;
    }

    public T[] New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers = default)
    {
        if (capacity == 0) return Array.Empty<T>();
        if (builder == null) throw new ArgumentNullException(nameof(builder));
        if (capacity < 0) throw Ex.ArgumentNegative(capacity, nameof(capacity));

        var array = new T[capacity];
        var index = 0;

        builder(item => { array[index++] = item; return true; }, in state);

        return array;
    }

    public override int GetHashCode() => HashCode.Combine(GetType());

    public override bool Equals(object? obj) => Equals(obj as ArrayFactory);

    public bool Equals(ArrayFactory? other) => this == other || (other != null && other.GetType() == GetType());

    IEnumerable<T> IEnumerableFactory.Empty<T>(in Comparers<T> comparers) => Empty(in comparers);
    IEnumerable<T> IEnumerableFactory.New<T>(int capacity, in Comparers<T> comparers) => New(capacity, in comparers);
    IEnumerable<T> IEnumerableFactory.New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers) => New(capacity, builder, in comparers);
    IEnumerable<T> IEnumerableFactory.New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers) => New(capacity, builder, in state, in comparers);
}