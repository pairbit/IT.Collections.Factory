#if NETCOREAPP3_1_OR_GREATER

using System.Collections.Immutable;

namespace IT.Collections.Factory.Factories;

public sealed class ImmutableHashSetFactory : ISetFactory, IImmutableSetFactory, IEquatable<ImmutableHashSetFactory>
#if NET6_0_OR_GREATER
    , IReadOnlySetFactory
#endif
{
    public Type EnumerableType => typeof(ImmutableHashSet<>);

    public EnumerableKind Kind => EnumerableKind.Unordered | EnumerableKind.Unique | EnumerableKind.Equatable | EnumerableKind.IgnoreCapacity;

    public ImmutableHashSet<T> Empty<T>(in Comparers<T> comparers = default)
        => ImmutableHashSet<T>.Empty.WithComparer(comparers.EqualityComparer);

    public ImmutableHashSet<T> New<T>(int capacity, in Comparers<T> comparers = default)
        => ImmutableHashSet<T>.Empty.WithComparer(comparers.EqualityComparer);

    public ImmutableHashSet<T> New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers = default)
    {
        if (capacity == 0) return ImmutableHashSet<T>.Empty.WithComparer(comparers.EqualityComparer);
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var hashSetBuilder = ImmutableHashSet<T>.Empty.ToBuilder();
        var equalityComparer = comparers.EqualityComparer;
        if (equalityComparer != null) hashSetBuilder.KeyComparer = equalityComparer;

        builder(hashSetBuilder.Add);

        return hashSetBuilder.ToImmutable();
    }

    public ImmutableHashSet<T> New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers = default)
    {
        if (capacity == 0) return ImmutableHashSet<T>.Empty.WithComparer(comparers.EqualityComparer);
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var hashSetBuilder = ImmutableHashSet<T>.Empty.ToBuilder();
        var equalityComparer = comparers.EqualityComparer;
        if (equalityComparer != null) hashSetBuilder.KeyComparer = equalityComparer;

        builder(hashSetBuilder.Add, in state);

        return hashSetBuilder.ToImmutable();
    }

    public override int GetHashCode() => HashCode.Combine(GetType());

    public override bool Equals(object? obj) => Equals(obj as ImmutableHashSetFactory);

    public bool Equals(ImmutableHashSetFactory? other) => this == other || (other != null && other.GetType() == GetType());

    ISet<T> ISetFactory.Empty<T>(in Comparers<T> comparers) => Empty(in comparers);
    ISet<T> ISetFactory.New<T>(int capacity, in Comparers<T> comparers) => New(capacity, in comparers);
    ISet<T> ISetFactory.New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers) => New(capacity, builder, in comparers);
    ISet<T> ISetFactory.New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers) => New(capacity, builder, in state, in comparers);
    ICollection<T> ICollectionFactory.Empty<T>(in Comparers<T> comparers) => Empty(in comparers);
    ICollection<T> ICollectionFactory.New<T>(int capacity, in Comparers<T> comparers) => New(capacity, in comparers);
    ICollection<T> ICollectionFactory.New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers) => New(capacity, builder, in comparers);
    ICollection<T> ICollectionFactory.New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers) => New(capacity, builder, in state, in comparers);
    IImmutableSet<T> IImmutableSetFactory.Empty<T>(in Comparers<T> comparers) => Empty(in comparers);
    IImmutableSet<T> IImmutableSetFactory.New<T>(int capacity, in Comparers<T> comparers) => New(capacity, in comparers);
    IImmutableSet<T> IImmutableSetFactory.New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers) => New(capacity, builder, in comparers);
    IImmutableSet<T> IImmutableSetFactory.New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers) => New(capacity, builder, in state, in comparers);
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

#endif