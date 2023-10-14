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
        HashSet<T> Empty<T>() => new((IEqualityComparer<T>?)null);

    public
#if NET5_0_OR_GREATER
        override
#endif
        HashSet<T> New<T>(int capacity)
#if NETSTANDARD2_0 || NET461
        => new();
#else
        => new(capacity, null);
#endif

    public
#if NET5_0_OR_GREATER
        override
#endif
        HashSet<T> New<T>(int capacity, EnumerableBuilder<T> builder)
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
        HashSet<T> New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state)
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
    IEnumerable<T> IEnumerableFactory.Empty<T>() => Empty<T>();
    IEnumerable<T> IEnumerableFactory.New<T>(int capacity) => New<T>(capacity);
    IEnumerable<T> IEnumerableFactory.New<T>(int capacity, EnumerableBuilder<T> builder) => New(capacity, builder);
    IEnumerable<T> IEnumerableFactory.New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state) => New(capacity, builder, in state);
#endif
}