namespace IT.Collections.Factory.Factories;

public class HashSetFactory : ISetFactory, IReadOnlyCollectionFactory
#if NET6_0_OR_GREATER
    , IReadOnlySetFactory
#endif
{
    public static readonly HashSetFactory Default = new();

    public virtual EnumerableKind Kind => EnumerableKind.Unique | EnumerableKind.Equatable;

    public virtual HashSet<T> Empty<T>(in Comparers<T> comparers = default) => new(comparers.EqualityComparer);

    public virtual HashSet<T> New<T>(int capacity, in Comparers<T> comparers = default)
#if NETSTANDARD2_0 || NET461
        => new(comparers.EqualityComparer);
#else
        => new(capacity, comparers.EqualityComparer);
#endif

    public virtual HashSet<T> New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers = default)
    {
        if (capacity == 0) return new(comparers.EqualityComparer);
        if (builder == null) throw new ArgumentNullException(nameof(builder));

#if NETSTANDARD2_0 || NET461
        var hashSet = new HashSet<T>(comparers.EqualityComparer);
#else
        var hashSet = new HashSet<T>(capacity, comparers.EqualityComparer);
#endif

        builder(hashSet.Add);

        return hashSet;
    }

    public virtual HashSet<T> New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers = default)
    {
        if (capacity == 0) return new(comparers.EqualityComparer);
        if (builder == null) throw new ArgumentNullException(nameof(builder));

#if NETSTANDARD2_0 || NET461
        var hashSet = new HashSet<T>(comparers.EqualityComparer);
#else
        var hashSet = new HashSet<T>(capacity, comparers.EqualityComparer);
#endif

        builder(hashSet.Add, in state);

        return hashSet;
    }

    ISet<T> ISetFactory.Empty<T>(in Comparers<T> comparers) => Empty(in comparers);
    ISet<T> ISetFactory.New<T>(int capacity, in Comparers<T> comparers) => New(capacity, in comparers);
    ISet<T> ISetFactory.New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers) => New(capacity, builder, in comparers);
    ISet<T> ISetFactory.New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers) => New(capacity, builder, in state, in comparers);
    ICollection<T> ICollectionFactory.Empty<T>(in Comparers<T> comparers) => Empty(in comparers);
    ICollection<T> ICollectionFactory.New<T>(int capacity, in Comparers<T> comparers) => New(capacity, in comparers);
    ICollection<T> ICollectionFactory.New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers) => New(capacity, builder, in comparers);
    ICollection<T> ICollectionFactory.New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers) => New(capacity, builder, in state, in comparers);
#if NET6_0_OR_GREATER
    IReadOnlySet<T> IReadOnlySetFactory.Empty<T>(in Comparers<T> comparers) => Empty(in comparers);
    IReadOnlySet<T> IReadOnlySetFactory.New<T>(int capacity, in Comparers<T> comparers) => New(capacity, in comparers);
    IReadOnlySet<T> IReadOnlySetFactory.New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers) => New(capacity, builder, in comparers);
    IReadOnlySet<T> IReadOnlySetFactory.New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers) => New(capacity, builder, in state, in comparers);
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