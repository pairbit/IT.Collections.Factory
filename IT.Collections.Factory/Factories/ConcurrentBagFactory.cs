namespace IT.Collections.Factory.Factories;

public class ConcurrentBagFactory : IProducerConsumerCollectionFactory, IReadOnlyCollectionFactory, IEquatable<ConcurrentBagFactory>
{
    public virtual Type EnumerableType => typeof(ConcurrentBag<>);

    public virtual EnumerableKind Kind => EnumerableKind.Reverse | EnumerableKind.Unordered | EnumerableKind.ThreadSafe | EnumerableKind.IgnoreCapacity;

    public virtual ConcurrentBag<T> Empty<T>(in Comparers<T> comparers = default) =>
#if NET5_0_OR_GREATER
        new();
#else
        NewBag(0, in comparers);
#endif

    public virtual ConcurrentBag<T> New<T>(int capacity, in Comparers<T> comparers = default) =>
#if NET5_0_OR_GREATER
        new();
#else
        NewBag(capacity, in comparers);
#endif

    public virtual ConcurrentBag<T> New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers = default)
    {
        if (capacity == 0) return
#if NET5_0_OR_GREATER
        new();
#else
        NewBag(0, in comparers);
#endif
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var bag =
#if NET5_0_OR_GREATER
        new ConcurrentBag<T>();
#else
        NewBag(capacity, in comparers);
#endif

        builder(((IProducerConsumerCollection<T>)bag).TryAdd);

        return bag;
    }

    public virtual ConcurrentBag<T> New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers = default)
    {
        if (capacity == 0) return
#if NET5_0_OR_GREATER
        new();
#else
        NewBag(0, in comparers);
#endif
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var bag =
#if NET5_0_OR_GREATER
        new ConcurrentBag<T>();
#else
        NewBag(capacity, in comparers);
#endif

        builder(((IProducerConsumerCollection<T>)bag).TryAdd, in state);

        return bag;
    }

    public override int GetHashCode() => HashCode.Combine(GetType());

    public override bool Equals(object? obj) => Equals(obj as ConcurrentBagFactory);

    public bool Equals(ConcurrentBagFactory? other) => this == other || (other != null && other.GetType() == GetType());

#if !NET5_0_OR_GREATER
    protected virtual ConcurrentBag<T> NewBag<T>(int capacity, in Comparers<T> comparers) => new();
#endif

    IProducerConsumerCollection<T> IProducerConsumerCollectionFactory.Empty<T>(in Comparers<T> comparers) => Empty(in comparers);
    IProducerConsumerCollection<T> IProducerConsumerCollectionFactory.New<T>(int capacity, in Comparers<T> comparers) => New(capacity, in comparers);
    IProducerConsumerCollection<T> IProducerConsumerCollectionFactory.New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers) => New(capacity, builder, in comparers);
    IProducerConsumerCollection<T> IProducerConsumerCollectionFactory.New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers) => New(capacity, builder, in state, in comparers);
    IReadOnlyCollection<T> IReadOnlyCollectionFactory.Empty<T>(in Comparers<T> comparers) => Empty(in comparers);
    IReadOnlyCollection<T> IReadOnlyCollectionFactory.New<T>(int capacity, in Comparers<T> comparers) => New(capacity, in comparers);
    IReadOnlyCollection<T> IReadOnlyCollectionFactory.New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers) => New(capacity, builder, in comparers);
    IReadOnlyCollection<T> IReadOnlyCollectionFactory.New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers) => New(capacity, builder, in state, in comparers);
    IEnumerable<T> IEnumerableFactory.Empty<T>(in Comparers<T> comparers) => Empty(in comparers);
    IEnumerable<T> IEnumerableFactory.New<T>(int capacity, in Comparers<T> comparers) => New(capacity, in comparers);
    IEnumerable<T> IEnumerableFactory.New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers) => New(capacity, builder, in comparers);
    IEnumerable<T> IEnumerableFactory.New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers) => New(capacity, builder, in state, in comparers);
}