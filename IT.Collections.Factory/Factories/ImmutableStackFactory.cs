﻿#if NETCOREAPP3_1_OR_GREATER

using System.Collections.Immutable;

namespace IT.Collections.Factory.Factories;

public sealed class ImmutableStackFactory : IImmutableStackFactory, IEquatable<ImmutableStackFactory>
{
    public Type EnumerableType => typeof(ImmutableStack<>);

    public EnumerableKind Kind => EnumerableKind.Reverse | EnumerableKind.IgnoreCapacity;

    public ImmutableStack<T> Empty<T>(in Comparers<T> comparers = default) => ImmutableStack<T>.Empty;

    public ImmutableStack<T> New<T>(int capacity, in Comparers<T> comparers = default) => ImmutableStack<T>.Empty;

    public ImmutableStack<T> New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers = default)
    {
        if (capacity == 0) return ImmutableStack<T>.Empty;
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var stack = ImmutableStack<T>.Empty;

        builder(item => { stack = stack.Push(item); return true; });

        return stack;
    }

    public ImmutableStack<T> New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers = default)
    {
        if (capacity == 0) return ImmutableStack<T>.Empty;
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var stack = ImmutableStack<T>.Empty;

        builder(item => { stack = stack.Push(item); return true; }, in state);

        return stack;
    }

    public override int GetHashCode() => HashCode.Combine(GetType());

    public override bool Equals(object? obj) => Equals(obj as ImmutableStackFactory);

    public bool Equals(ImmutableStackFactory? other) => this == other || (other != null && other.GetType() == GetType());

    IImmutableStack<T> IImmutableStackFactory.Empty<T>(in Comparers<T> comparers) => Empty(in comparers);
    IImmutableStack<T> IImmutableStackFactory.New<T>(int capacity, in Comparers<T> comparers) => New(capacity, in comparers);
    IImmutableStack<T> IImmutableStackFactory.New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers) => New(capacity, builder, in comparers);
    IImmutableStack<T> IImmutableStackFactory.New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers) => New(capacity, builder, in state, in comparers);
    IEnumerable<T> IEnumerableFactory.Empty<T>(in Comparers<T> comparers) => Empty(in comparers);
    IEnumerable<T> IEnumerableFactory.New<T>(int capacity, in Comparers<T> comparers) => New(capacity, in comparers);
    IEnumerable<T> IEnumerableFactory.New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers) => New(capacity, builder, in comparers);
    IEnumerable<T> IEnumerableFactory.New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers) => New(capacity, builder, in state, in comparers);
}

#endif