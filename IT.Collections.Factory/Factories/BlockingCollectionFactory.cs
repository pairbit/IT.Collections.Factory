namespace IT.Collections.Factory.Factories;

public class BlockingCollectionFactory : IReadOnlyCollectionFactory, IEquatable<BlockingCollectionFactory>
{
    private readonly IProducerConsumerCollectionFactory _factory;

    public virtual Type EnumerableType => typeof(BlockingCollection<>);

    public virtual EnumerableKind Kind => EnumerableKind.ThreadSafe;

    public BlockingCollectionFactory() : this(EnumerableFactoryCache<ConcurrentQueueFactory>.Default) { }

    public BlockingCollectionFactory(IProducerConsumerCollectionFactory factory)
    {
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
    }

    public virtual BlockingCollection<T> Empty<T>(in Comparers<T> comparers = default) =>
#if NET5_0_OR_GREATER
        new(_factory.Empty(in comparers));
#else
        NewCollection(_factory.Empty(in comparers), in comparers);
#endif

    public virtual BlockingCollection<T> New<T>(int capacity, in Comparers<T> comparers = default) =>
#if NET5_0_OR_GREATER
        new(_factory.New(capacity, in comparers));
#else
        NewCollection(_factory.New(capacity, in comparers), in comparers);
#endif

    public virtual BlockingCollection<T> New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers = default) =>
#if NET5_0_OR_GREATER
        new(_factory.New(capacity, builder, in comparers));
#else
        NewCollection(_factory.New(capacity, builder, in comparers), in comparers);
#endif

    public virtual BlockingCollection<T> New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers = default) =>
#if NET5_0_OR_GREATER
        new(_factory.New(capacity, builder, in state, in comparers));
#else
        NewCollection(_factory.New(capacity, builder, in state, in comparers), in comparers);
#endif

    public override int GetHashCode() => HashCode.Combine(GetType(), _factory);

    public override bool Equals(object? obj) => Equals(obj as BlockingCollectionFactory);

    public bool Equals(BlockingCollectionFactory? other)
        => this == other || (other != null && other.GetType() == GetType() && _factory.Equals(other._factory));

#if !NET5_0_OR_GREATER
    protected virtual BlockingCollection<T> NewCollection<T>(IProducerConsumerCollection<T> collection, in Comparers<T> comparers)
        => new(collection);
#endif

    IReadOnlyCollection<T> IReadOnlyCollectionFactory.Empty<T>(in Comparers<T> comparers) => Empty(in comparers);
    IReadOnlyCollection<T> IReadOnlyCollectionFactory.New<T>(int capacity, in Comparers<T> comparers) => New(capacity, in comparers);
    IReadOnlyCollection<T> IReadOnlyCollectionFactory.New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers) => New(capacity, builder, in comparers);
    IReadOnlyCollection<T> IReadOnlyCollectionFactory.New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers) => New(capacity, builder, in state, in comparers);
    IEnumerable<T> IEnumerableFactory.Empty<T>(in Comparers<T> comparers) => Empty(in comparers);
    IEnumerable<T> IEnumerableFactory.New<T>(int capacity, in Comparers<T> comparers) => New(capacity, in comparers);
    IEnumerable<T> IEnumerableFactory.New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers) => New(capacity, builder, in comparers);
    IEnumerable<T> IEnumerableFactory.New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers) => New(capacity, builder, in state, in comparers);
}