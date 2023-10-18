#if NET6_0_OR_GREATER

namespace IT.Collections.Factory.Factories;

using Internal;

public class ReadOnlyHashSetFactory : IReadOnlySetFactory
{
    public static readonly ReadOnlyHashSetFactory Default = new();

    public virtual Type EnumerableType => typeof(IReadOnlySet<>);

    public virtual EnumerableKind Kind => EnumerableKind.ReadOnly | EnumerableKind.Unique | EnumerableKind.Equatable;

    public virtual IReadOnlySet<T> Empty<T>(in Comparers<T> comparers = default) => ReadOnlySet<T>.Empty;

    public virtual IReadOnlySet<T> New<T>(int capacity, in Comparers<T> comparers = default)
    {
        throw new NotSupportedException();
    }

    public virtual IReadOnlySet<T> New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers = default)
    {
        if (capacity == 0) return ReadOnlySet<T>.Empty;
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var hashSet = new HashSet<T>(capacity, comparers.EqualityComparer);

        builder(hashSet.Add);

        return new ReadOnlySet<T>(hashSet);
    }

    public virtual IReadOnlySet<T> New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers = default)
    {
        if (capacity == 0) return ReadOnlySet<T>.Empty;
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var hashSet = new HashSet<T>(capacity, comparers.EqualityComparer);

        builder(hashSet.Add, in state);

        return new ReadOnlySet<T>(hashSet);
    }

    IReadOnlyCollection<T> IReadOnlyCollectionFactory.Empty<T>(in Comparers<T> comparers) => Empty(in comparers);
    IReadOnlyCollection<T> IReadOnlyCollectionFactory.New<T>(int capacity, in Comparers<T> comparers) => New(capacity, in comparers);
    IReadOnlyCollection<T> IReadOnlyCollectionFactory.New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers) => New(capacity, builder, in comparers);
    IReadOnlyCollection<T> IReadOnlyCollectionFactory.New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers) => New(capacity, builder, in state, in comparers);
    IEnumerable<T> IEnumerableFactory.Empty<T>(in Comparers<T> comparers) => Empty(in comparers);
    IEnumerable<T> IEnumerableFactory.New<T>(int capacity, in Comparers<T> comparers) => New(capacity, in comparers);
    IEnumerable<T> IEnumerableFactory.New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers) => New(capacity, builder, in comparers);
    IEnumerable<T> IEnumerableFactory.New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers) => New(capacity, builder, in state, in comparers);
}

#endif