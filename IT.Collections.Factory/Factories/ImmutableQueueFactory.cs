#if NETCOREAPP3_1_OR_GREATER

using System.Collections.Immutable;

namespace IT.Collections.Factory.Factories;

public sealed class ImmutableQueueFactory : IImmutableQueueFactory, IEquatable<ImmutableQueueFactory>
{
    public Type EnumerableType => typeof(ImmutableQueue<>);

    public EnumerableKind Kind => EnumerableKind.IgnoreCapacity;

    public ImmutableQueue<T> Empty<T>(in Comparers<T> comparers = default) => ImmutableQueue<T>.Empty;

    public ImmutableQueue<T> New<T>(int capacity, in Comparers<T> comparers = default) => ImmutableQueue<T>.Empty;

    public ImmutableQueue<T> New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers = default)
    {
        if (capacity == 0) return ImmutableQueue<T>.Empty;
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var queue = ImmutableQueue<T>.Empty;

        builder(item => { queue = queue.Enqueue(item); return true; });

        return queue;
    }

    public ImmutableQueue<T> New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers = default)
    {
        if (capacity == 0) return ImmutableQueue<T>.Empty;
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var queue = ImmutableQueue<T>.Empty;

        builder(item => { queue = queue.Enqueue(item); return true; }, in state);

        return queue;
    }

    public override int GetHashCode() => HashCode.Combine(GetType());

    public override bool Equals(object? obj) => Equals(obj as ImmutableQueueFactory);

    public bool Equals(ImmutableQueueFactory? other) => this == other || (other != null && other.GetType() == GetType());

    IImmutableQueue<T> IImmutableQueueFactory.Empty<T>(in Comparers<T> comparers) => Empty(in comparers);
    IImmutableQueue<T> IImmutableQueueFactory.New<T>(int capacity, in Comparers<T> comparers) => New(capacity, in comparers);
    IImmutableQueue<T> IImmutableQueueFactory.New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers) => New(capacity, builder, in comparers);
    IImmutableQueue<T> IImmutableQueueFactory.New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers) => New(capacity, builder, in state, in comparers);
    IEnumerable<T> IEnumerableFactory.Empty<T>(in Comparers<T> comparers) => Empty(in comparers);
    IEnumerable<T> IEnumerableFactory.New<T>(int capacity, in Comparers<T> comparers) => New(capacity, in comparers);
    IEnumerable<T> IEnumerableFactory.New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers) => New(capacity, builder, in comparers);
    IEnumerable<T> IEnumerableFactory.New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers) => New(capacity, builder, in state, in comparers);
}

#endif