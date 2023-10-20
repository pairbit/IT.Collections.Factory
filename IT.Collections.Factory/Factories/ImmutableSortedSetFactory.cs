#if NETCOREAPP3_1_OR_GREATER

using System.Collections.Immutable;

namespace IT.Collections.Factory.Factories;

public sealed class ImmutableSortedSetFactory : IImmutableSetFactory, IEquatable<ImmutableSortedSetFactory>
{
    public static readonly ImmutableSortedSetFactory Default = new();

    public Type EnumerableType => typeof(ImmutableSortedSet<>);

    public EnumerableKind Kind => EnumerableKind.Ordered | EnumerableKind.Unique | EnumerableKind.Comparable;

    public ImmutableSortedSet<T> Empty<T>(in Comparers<T> comparers = default)
        => ImmutableSortedSet<T>.Empty.WithComparer(comparers.Comparer);

    public ImmutableSortedSet<T> New<T>(int capacity, in Comparers<T> comparers = default)
        => ImmutableSortedSet<T>.Empty.WithComparer(comparers.Comparer);

    public ImmutableSortedSet<T> New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers = default)
    {
        if (capacity == 0) return ImmutableSortedSet<T>.Empty.WithComparer(comparers.Comparer);
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var sortedSetBuilder = ImmutableSortedSet<T>.Empty.ToBuilder();
        var comparer = comparers.Comparer;
        if (comparer != null) sortedSetBuilder.KeyComparer = comparer;

        builder(sortedSetBuilder.Add);

        return sortedSetBuilder.ToImmutable();
    }

    public ImmutableSortedSet<T> New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers = default)
    {
        if (capacity == 0) return ImmutableSortedSet<T>.Empty.WithComparer(comparers.Comparer);
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var sortedSetBuilder = ImmutableSortedSet<T>.Empty.ToBuilder();
        var comparer = comparers.Comparer;
        if (comparer != null) sortedSetBuilder.KeyComparer = comparer;

        builder(sortedSetBuilder.Add, in state);

        return sortedSetBuilder.ToImmutable();
    }

    public override int GetHashCode() => HashCode.Combine(GetType());

    public override bool Equals(object? obj) => Equals(obj as ImmutableSortedSetFactory);

    public bool Equals(ImmutableSortedSetFactory? other) => this == other || (other != null && other.GetType() == GetType());

    IImmutableSet<T> IImmutableSetFactory.Empty<T>(in Comparers<T> comparers) => Empty(in comparers);
    IImmutableSet<T> IImmutableSetFactory.New<T>(int capacity, in Comparers<T> comparers) => New(capacity, in comparers);
    IImmutableSet<T> IImmutableSetFactory.New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers) => New(capacity, builder, in comparers);
    IImmutableSet<T> IImmutableSetFactory.New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers) => New(capacity, builder, in state, in comparers);
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