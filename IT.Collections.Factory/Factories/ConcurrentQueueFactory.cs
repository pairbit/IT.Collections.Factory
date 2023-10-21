namespace IT.Collections.Factory.Factories;

public class ConcurrentQueueFactory : IProducerConsumerCollectionFactory, IReadOnlyCollectionFactory, IEquatable<ConcurrentQueueFactory>
{
    public virtual Type EnumerableType => typeof(ConcurrentQueue<>);

    public virtual EnumerableKind Kind => EnumerableKind.ThreadSafe | EnumerableKind.IgnoreCapacity;

    public virtual ConcurrentQueue<T> Empty<T>(in Comparers<T> comparers = default) =>
#if NET5_0_OR_GREATER
        new();
#else
        NewQueue(0, in comparers);
#endif

    public virtual ConcurrentQueue<T> New<T>(int capacity, in Comparers<T> comparers = default) =>
#if NET5_0_OR_GREATER
        new();
#else
        NewQueue(capacity, in comparers);
#endif

    public virtual ConcurrentQueue<T> New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers = default)
    {
        if (capacity == 0) return
#if NET5_0_OR_GREATER
        new();
#else
        NewQueue(0, in comparers);
#endif
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var queue =
#if NET5_0_OR_GREATER
        new ConcurrentQueue<T>();
#else
        NewQueue(capacity, in comparers);
#endif

        builder(((IProducerConsumerCollection<T>)queue).TryAdd);

        return queue;
    }

    public virtual ConcurrentQueue<T> New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers = default)
    {
        if (capacity == 0) return
#if NET5_0_OR_GREATER
        new();
#else
        NewQueue(0, in comparers);
#endif
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var queue =
#if NET5_0_OR_GREATER
        new ConcurrentQueue<T>();
#else
        NewQueue(capacity, in comparers);
#endif

        builder(((IProducerConsumerCollection<T>)queue).TryAdd, in state);

        return queue;
    }

    public override int GetHashCode() => HashCode.Combine(GetType());

    public override bool Equals(object? obj) => Equals(obj as ConcurrentQueueFactory);

    public bool Equals(ConcurrentQueueFactory? other) => this == other || (other != null && other.GetType() == GetType());

#if !NET5_0_OR_GREATER
    protected virtual ConcurrentQueue<T> NewQueue<T>(int capacity, in Comparers<T> comparers) => new();
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