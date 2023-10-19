using IT.Collections.Factory.Generic;

namespace IT.Collections.Factory.Tests;

public class HashSetStringFactory : IEnumerableFactory<HashSet<string?>, string?>
{
    private readonly IEqualityComparer<string?>? _comparer;

    public Type EnumerableType => typeof(HashSet<string?>);

    public EnumerableKind Kind => EnumerableKind.None;

    public IEqualityComparer<string?>? Comparer => _comparer;

    public HashSetStringFactory(IEqualityComparer<string?>? comparer)
    {
        _comparer = comparer;
    }

    public HashSet<string?> Empty() => new(_comparer);

    public HashSet<string?> New(int capacity)
#if NETSTANDARD2_0 || NET462
        => new(_comparer);
#else
        => new(capacity, _comparer);
#endif

    public HashSet<string?> New(int capacity, EnumerableBuilder<string?> builder)
    {
        if (capacity == 0) return new(_comparer);
        if (builder == null) throw new ArgumentNullException(nameof(builder));

#if NETSTANDARD2_0 || NET462
        var hashSet = new HashSet<string?>(_comparer);
#else
        var hashSet = new HashSet<string?>(capacity, _comparer);
#endif

        builder(hashSet.Add);

        return hashSet;
    }

    public HashSet<string?> New<TState>(int capacity, EnumerableBuilder<string?, TState> builder, in TState state)
    {
        if (capacity == 0) return new(_comparer);
        if (builder == null) throw new ArgumentNullException(nameof(builder));

#if NETSTANDARD2_0 || NET462
        var hashSet = new HashSet<string?>(_comparer);
#else
        var hashSet = new HashSet<string?>(capacity, _comparer);
#endif

        builder(hashSet.Add, in state);

        return hashSet;
    }
}