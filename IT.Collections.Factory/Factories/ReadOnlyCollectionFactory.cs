namespace IT.Collections.Factory.Factories;

public class ReadOnlyCollectionFactory : IListFactory, IReadOnlyListFactory, IEquatable<ReadOnlyCollectionFactory>
{
    public static readonly ReadOnlyCollectionFactory Default = new();

    protected readonly IListFactory _factory;

    public virtual Type EnumerableType => typeof(ReadOnlyCollection<>);

    public virtual EnumerableKind Kind => EnumerableKind.ReadOnly;

    public ReadOnlyCollectionFactory() : this(ListFactory.Default) { }

    public ReadOnlyCollectionFactory(IListFactory factory)
    {
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
    }

    public virtual ReadOnlyCollection<T> Empty<T>(in Comparers<T> comparers = default) =>
#if NET5_0_OR_GREATER
        Cache<T>.Empty;
#else
        NewCollection(null, in comparers);
#endif

    public virtual ReadOnlyCollection<T> New<T>(int capacity, in Comparers<T> comparers = default)
        => throw new NotSupportedException();

    public virtual ReadOnlyCollection<T> New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers = default)
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

    public virtual ReadOnlyCollection<T> New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers = default)
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

    public override bool Equals(object? obj) => Equals(obj as ReadOnlyCollectionFactory);

    public bool Equals(ReadOnlyCollectionFactory? other)
        => this == other || (other != null && other.GetType() == GetType() &&
        (_factory == other._factory || (_factory != null && _factory.Equals(other._factory))));

#if !NET5_0_OR_GREATER
    protected virtual ReadOnlyCollection<T> NewCollection<T>(IList<T>? list, in Comparers<T> comparers)
        => list == null ? Cache<T>.Empty : new(list);
#endif

    static class Cache<T>
    {
        public readonly static ReadOnlyCollection<T> Empty = new(new List<T>());
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