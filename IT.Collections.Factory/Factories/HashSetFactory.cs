namespace IT.Collections.Factory.Factories;

public class HashSetFactory :
#if NET5_0_OR_GREATER
    EnumerableFactory
#else
    IEnumerableFactory
#endif
{
    public static readonly HashSetFactory Default = new();

    public
#if NET5_0_OR_GREATER
        override
#endif
        EnumerableType Type => EnumerableType.Unique;

    public
#if NET5_0_OR_GREATER
        override
#endif
        HashSet<T> Empty<T>(in Comparers<T> comparers = default) => new((IEqualityComparer<T>?)null);

    public
#if NET5_0_OR_GREATER
        override
#endif
        HashSet<T> New<T>(int capacity, in Comparers<T> comparers = default)
#if NETSTANDARD2_0 || NET461
        => new();
#else
        => new(capacity, null);
#endif

    public
#if NET5_0_OR_GREATER
        override
#endif
        HashSet<T> New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers = default)
    {
        if (capacity == 0) return new((IEqualityComparer<T>?)null);
        if (builder == null) throw new ArgumentNullException(nameof(builder));

#if NETSTANDARD2_0 || NET461
        var hashSet = new HashSet<T>();
#else
        var hashSet = new HashSet<T>(capacity, null);
#endif

        builder(hashSet.Add);

        return hashSet;
    }

    public
#if NET5_0_OR_GREATER
        override
#endif
        HashSet<T> New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers = default)
    {
        if (capacity == 0) return new((IEqualityComparer<T>?)null);
        if (builder == null) throw new ArgumentNullException(nameof(builder));

#if NETSTANDARD2_0 || NET461
        var hashSet = new HashSet<T>();
#else
        var hashSet = new HashSet<T>(capacity, null);
#endif

        builder(hashSet.Add, in state);

        return hashSet;
    }
#if !NET5_0_OR_GREATER
    IEnumerable<T> IEnumerableFactory.Empty<T>(in Comparers<T> comparers) => Empty(in comparers);
    IEnumerable<T> IEnumerableFactory.New<T>(int capacity, in Comparers<T> comparers) => New(capacity, in comparers);
    IEnumerable<T> IEnumerableFactory.New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers) => New(capacity, builder, in comparers);
    IEnumerable<T> IEnumerableFactory.New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers) => New(capacity, builder, in state, in comparers);
#endif
}