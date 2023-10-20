namespace IT.Collections.Factory.Factories;

public class HashSetFactory : ISetFactory, IReadOnlyCollectionFactory, IEquatable<HashSetFactory>
#if NET6_0_OR_GREATER
    , IReadOnlySetFactory
#endif
{
    public virtual Type EnumerableType => typeof(HashSet<>);

    public virtual EnumerableKind Kind => EnumerableKind.Unique | EnumerableKind.Equatable;

    public virtual HashSet<T> Empty<T>(in Comparers<T> comparers = default) =>
#if NET5_0_OR_GREATER
        new(comparers.EqualityComparer);
#else
        NewSet(0, in comparers);
#endif

    public virtual HashSet<T> New<T>(int capacity, in Comparers<T> comparers = default) =>
#if NET5_0_OR_GREATER
        new(capacity, comparers.EqualityComparer);
#else
        NewSet(capacity, in comparers);
#endif

    public virtual HashSet<T> New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers = default)
    {
        if (capacity == 0) return
#if NET5_0_OR_GREATER
        new(comparers.EqualityComparer);
#else
        NewSet(0, in comparers);
#endif
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var hashSet =
#if NET5_0_OR_GREATER
        new HashSet<T>(capacity, comparers.EqualityComparer);
#else
        NewSet(capacity, in comparers);
#endif

        builder(hashSet.Add);

        return hashSet;
    }

    public virtual HashSet<T> New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers = default)
    {
        if (capacity == 0) return
#if NET5_0_OR_GREATER
        new(comparers.EqualityComparer);
#else
        NewSet(0, in comparers);
#endif
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var hashSet =
#if NET5_0_OR_GREATER
        new HashSet<T>(capacity, comparers.EqualityComparer);
#else
        NewSet(capacity, in comparers);
#endif

        builder(hashSet.Add, in state);

        return hashSet;
    }

    public override int GetHashCode() => HashCode.Combine(GetType());

    public override bool Equals(object? obj) => Equals(obj as HashSetFactory);

    public bool Equals(HashSetFactory? other) => this == other || (other != null && other.GetType() == GetType());

#if !NET5_0_OR_GREATER
    protected virtual HashSet<T> NewSet<T>(int capacity, in Comparers<T> comparers) => new(
#if !NET461 && !NETSTANDARD2_0
            capacity,
#endif
            comparers.EqualityComparer);
#endif

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