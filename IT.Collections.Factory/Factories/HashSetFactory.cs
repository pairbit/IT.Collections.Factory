namespace IT.Collections.Factory.Factories;

public class HashSetFactory : IEnumerableFactory
{
    public static readonly HashSetFactory Default = new();

    public EnumerableType Type => EnumerableType.Unique;

    public IEnumerable<T> Empty<T>() => new HashSet<T>((IEqualityComparer<T>?)null);

    public IEnumerable<T> New<T>(int capacity)
#if NETSTANDARD2_0 || NET461
        => new HashSet<T>();
#else
        => new HashSet<T>(capacity, null);
#endif

    public IEnumerable<T> New<T>(int capacity, EnumerableBuilder<T> builder)
    {
        if (capacity == 0) return new HashSet<T>((IEqualityComparer<T>?)null);
        if (builder == null) throw new ArgumentNullException(nameof(builder));

#if NETSTANDARD2_0 || NET461
        var hashSet = new HashSet<T>();
#else
        var hashSet = new HashSet<T>(capacity, null);
#endif

        builder(hashSet.Add);

        return hashSet;
    }

    public IEnumerable<T> New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state)
    {
        if (capacity == 0) return new HashSet<T>((IEqualityComparer<T>?)null);
        if (builder == null) throw new ArgumentNullException(nameof(builder));

#if NETSTANDARD2_0 || NET461
        var hashSet = new HashSet<T>();
#else
        var hashSet = new HashSet<T>(capacity, null);
#endif

        builder(hashSet.Add, in state);

        return hashSet;
    }
}