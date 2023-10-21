namespace IT.Collections.Factory.Factories;

public class ReadOnlyObservableCollectionFactory : IListFactory, IReadOnlyListFactory, IEquatable<ReadOnlyObservableCollectionFactory>
{
    protected readonly ObservableCollectionFactory _factory;

    public virtual Type EnumerableType => typeof(ReadOnlyObservableCollection<>);
    //TODO: костыль, так делать нельзя, по хорошему нужно указать _factory.Kind
    public virtual EnumerableKind Kind => EnumerableKind.ReadOnly | EnumerableKind.IgnoreCapacity;

    public ReadOnlyObservableCollectionFactory() : this(EnumerableFactoryCache<ObservableCollectionFactory>.Default) { }

    public ReadOnlyObservableCollectionFactory(ObservableCollectionFactory factory)
    {
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
    }

    public virtual ReadOnlyObservableCollection<T> Empty<T>(in Comparers<T> comparers = default) =>
#if NET5_0_OR_GREATER
        Cache<T>.Empty;
#else
        NewCollection(null, in comparers);
#endif

    public virtual ReadOnlyObservableCollection<T> New<T>(int capacity, in Comparers<T> comparers = default)
        => throw new NotSupportedException();

    public virtual ReadOnlyObservableCollection<T> New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers = default)
    {
        if (capacity == 0) return
#if NET5_0_OR_GREATER
        Cache<T>.Empty;
#else
        NewCollection(null, in comparers);
#endif

        return
#if NET5_0_OR_GREATER
        new(_factory.New(capacity, builder, in comparers));
#else
        NewCollection(_factory.New(capacity, builder, in comparers), in comparers);
#endif
    }

    public virtual ReadOnlyObservableCollection<T> New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers = default)
    {
        if (capacity == 0) return
#if NET5_0_OR_GREATER
        Cache<T>.Empty;
#else
        NewCollection(null, in comparers);
#endif

        return
#if NET5_0_OR_GREATER
        new(_factory.New(capacity, builder, in state, in comparers));
#else
        NewCollection(_factory.New(capacity, builder, in state, in comparers), in comparers);
#endif
    }

    public override int GetHashCode() => HashCode.Combine(GetType(), _factory);

    public override bool Equals(object? obj) => Equals(obj as ReadOnlyObservableCollectionFactory);

    public bool Equals(ReadOnlyObservableCollectionFactory? other)
        => this == other || (other != null && other.GetType() == GetType() &&
        (_factory == other._factory || (_factory != null && _factory.Equals(other._factory))));

#if !NET5_0_OR_GREATER
    protected virtual ReadOnlyObservableCollection<T> NewCollection<T>(ObservableCollection<T>? collection, in Comparers<T> comparers)
        => collection == null ? Cache<T>.Empty : new(collection);
#endif

    static class Cache<T>
    {
        public readonly static ReadOnlyObservableCollection<T> Empty = new(new ObservableCollection<T>());
    }

    IList<T> IListFactory.Empty<T>(in Comparers<T> comparers) => Empty(in comparers);
    IList<T> IListFactory.New<T>(int capacity, in Comparers<T> comparers) => New(capacity, in comparers);
    IList<T> IListFactory.New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers) => New(capacity, builder, in comparers);
    IList<T> IListFactory.New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers) => New(capacity, builder, in state, in comparers);
    ICollection<T> ICollectionFactory.Empty<T>(in Comparers<T> comparers) => Empty(in comparers);
    ICollection<T> ICollectionFactory.New<T>(int capacity, in Comparers<T> comparers) => New(capacity, in comparers);
    ICollection<T> ICollectionFactory.New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers) => New(capacity, builder, in comparers);
    ICollection<T> ICollectionFactory.New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers) => New(capacity, builder, in state, in comparers);
    IReadOnlyList<T> IReadOnlyListFactory.Empty<T>(in Comparers<T> comparers) => Empty(in comparers);
    IReadOnlyList<T> IReadOnlyListFactory.New<T>(int capacity, in Comparers<T> comparers) => New(capacity, in comparers);
    IReadOnlyList<T> IReadOnlyListFactory.New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers) => New(capacity, builder, in comparers);
    IReadOnlyList<T> IReadOnlyListFactory.New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers) => New(capacity, builder, in state, in comparers);
    IReadOnlyCollection<T> IReadOnlyCollectionFactory.Empty<T>(in Comparers<T> comparers) => Empty(in comparers);
    IReadOnlyCollection<T> IReadOnlyCollectionFactory.New<T>(int capacity, in Comparers<T> comparers) => New(capacity, in comparers);
    IReadOnlyCollection<T> IReadOnlyCollectionFactory.New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers) => New(capacity, builder, in comparers);
    IReadOnlyCollection<T> IReadOnlyCollectionFactory.New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers) => New(capacity, builder, in state, in comparers);
    IEnumerable<T> IEnumerableFactory.Empty<T>(in Comparers<T> comparers) => Empty(in comparers);
    IEnumerable<T> IEnumerableFactory.New<T>(int capacity, in Comparers<T> comparers) => New(capacity, in comparers);
    IEnumerable<T> IEnumerableFactory.New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers) => New(capacity, builder, in comparers);
    IEnumerable<T> IEnumerableFactory.New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers) => New(capacity, builder, in state, in comparers);
}