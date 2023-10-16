#if NET6_0_OR_GREATER

namespace IT.Collections.Factory.Factories;

using Internal;

public class ReadOnlyHashSetFactory : EnumerableFactory
{
    public static readonly ReadOnlyHashSetFactory Default = new();

    public override EnumerableType Type => EnumerableType.ReadOnly | EnumerableType.Unique | EnumerableType.Equatable;

    public override IReadOnlySet<T> Empty<T>(in Comparers<T> comparers = default) => ReadOnlySet<T>.Empty;

    public override IReadOnlySet<T> New<T>(int capacity, in Comparers<T> comparers = default)
    {
        throw new NotSupportedException();
    }

    public override IReadOnlySet<T> New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers = default)
    {
        if (capacity == 0) return ReadOnlySet<T>.Empty;
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var hashSet = new HashSet<T>(capacity, comparers.EqualityComparer);

        builder(hashSet.Add);

        return new ReadOnlySet<T>(hashSet);
    }

    public override IReadOnlySet<T> New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers = default)
    {
        if (capacity == 0) return ReadOnlySet<T>.Empty;
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var hashSet = new HashSet<T>(capacity, comparers.EqualityComparer);

        builder(hashSet.Add, in state);

        return new ReadOnlySet<T>(hashSet);
    }
}

#endif