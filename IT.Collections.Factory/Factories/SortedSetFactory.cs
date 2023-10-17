namespace IT.Collections.Factory.Factories;

public class SortedSetFactory : ISetFactory, IReadOnlyCollectionFactory
#if NET6_0_OR_GREATER
    , IReadOnlySetFactory
#endif
{
    public static readonly SortedSetFactory Default = new();

    public virtual EnumerableType Type => EnumerableType.Ordered | EnumerableType.Unique | EnumerableType.Comparable;

    public virtual SortedSet<T> Empty<T>(in Comparers<T> comparers = default) => new(comparers.Comparer);

    public virtual SortedSet<T> New<T>(int capacity, in Comparers<T> comparers = default) => new(comparers.Comparer);

    public virtual SortedSet<T> New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers = default)
    {
        if (capacity == 0) return new(comparers.Comparer);
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var sortedSet = new SortedSet<T>(comparers.Comparer);

        builder(sortedSet.Add);

        return sortedSet;
    }

    public virtual SortedSet<T> New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers = default)
    {
        if (capacity == 0) return new(comparers.Comparer);
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var sortedSet = new SortedSet<T>(comparers.Comparer);

        builder(sortedSet.Add, in state);

        return sortedSet;
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