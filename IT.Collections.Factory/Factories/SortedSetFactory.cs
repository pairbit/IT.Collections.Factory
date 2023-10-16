namespace IT.Collections.Factory.Factories;

public class SortedSetFactory :
#if NET5_0_OR_GREATER
    EnumerableFactory
#else
    IEnumerableFactory
#endif
{
    public static readonly SortedSetFactory Default = new();

    public
#if NET5_0_OR_GREATER
        override
#endif
        EnumerableType Type => EnumerableType.Ordered | EnumerableType.Unique | EnumerableType.Comparable;

    public
#if NET5_0_OR_GREATER
        override
#endif
        SortedSet<T> Empty<T>(in Comparers<T> comparers = default) => new(comparers.Comparer);

    public
#if NET5_0_OR_GREATER
        override
#endif
        SortedSet<T> New<T>(int capacity, in Comparers<T> comparers = default) => new(comparers.Comparer);

    public
#if NET5_0_OR_GREATER
        override
#endif
        SortedSet<T> New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers = default)
    {
        if (capacity == 0) return new(comparers.Comparer);
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var sortedSet = new SortedSet<T>(comparers.Comparer);

        builder(sortedSet.Add);

        return sortedSet;
    }

    public
#if NET5_0_OR_GREATER
        override
#endif
        SortedSet<T> New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers = default)
    {
        if (capacity == 0) return new(comparers.Comparer);
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var sortedSet = new SortedSet<T>(comparers.Comparer);

        builder(sortedSet.Add, in state);

        return sortedSet;
    }
#if !NET5_0_OR_GREATER
    IEnumerable<T> IEnumerableFactory.Empty<T>(in Comparers<T> comparers) => Empty(in comparers);
    IEnumerable<T> IEnumerableFactory.New<T>(int capacity, in Comparers<T> comparers) => New(capacity, in comparers);
    IEnumerable<T> IEnumerableFactory.New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers) => New(capacity, builder, in comparers);
    IEnumerable<T> IEnumerableFactory.New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers) => New(capacity, builder, in state, in comparers);
#endif
}