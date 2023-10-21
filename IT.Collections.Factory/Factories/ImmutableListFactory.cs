#if NETCOREAPP3_1_OR_GREATER

using System.Collections.Immutable;

namespace IT.Collections.Factory.Factories;

public sealed class ImmutableListFactory : IImmutableListFactory, IListFactory, IEquatable<ImmutableListFactory>
{
    public Type EnumerableType => typeof(ImmutableList<>);

    public EnumerableKind Kind => EnumerableKind.IgnoreCapacity;

    public ImmutableList<T> Empty<T>(in Comparers<T> comparers = default) => ImmutableList<T>.Empty;

    public ImmutableList<T> New<T>(int capacity, in Comparers<T> comparers = default) => ImmutableList<T>.Empty;

    public ImmutableList<T> New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers = default)
    {
        if (capacity == 0) return ImmutableList<T>.Empty;
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var listBuilder = ImmutableList<T>.Empty.ToBuilder();
        
        builder(item => { listBuilder.Add(item); return true; });

        return listBuilder.ToImmutable();
    }

    public ImmutableList<T> New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers = default)
    {
        if (capacity == 0) return ImmutableList<T>.Empty;
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var listBuilder = ImmutableList<T>.Empty.ToBuilder();

        builder(item => { listBuilder.Add(item); return true; }, in state);

        return listBuilder.ToImmutable();
    }

    public override int GetHashCode() => HashCode.Combine(GetType());

    public override bool Equals(object? obj) => Equals(obj as ImmutableListFactory);

    public bool Equals(ImmutableListFactory? other) => this == other || (other != null && other.GetType() == GetType());

    IList<T> IListFactory.Empty<T>(in Comparers<T> comparers) => Empty(in comparers);
    IList<T> IListFactory.New<T>(int capacity, in Comparers<T> comparers) => New(capacity, in comparers);
    IList<T> IListFactory.New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers) => New(capacity, builder, in comparers);
    IList<T> IListFactory.New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers) => New(capacity, builder, in state, in comparers);
    ICollection<T> ICollectionFactory.Empty<T>(in Comparers<T> comparers) => Empty(in comparers);
    ICollection<T> ICollectionFactory.New<T>(int capacity, in Comparers<T> comparers) => New(capacity, in comparers);
    ICollection<T> ICollectionFactory.New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers) => New(capacity, builder, in comparers);
    ICollection<T> ICollectionFactory.New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers) => New(capacity, builder, in state, in comparers);
    IImmutableList<T> IImmutableListFactory.Empty<T>(in Comparers<T> comparers) => Empty(in comparers);
    IImmutableList<T> IImmutableListFactory.New<T>(int capacity, in Comparers<T> comparers) => New(capacity, in comparers);
    IImmutableList<T> IImmutableListFactory.New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers) => New(capacity, builder, in comparers);
    IImmutableList<T> IImmutableListFactory.New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers) => New(capacity, builder, in state, in comparers);
    IReadOnlyList<T> IReadOnlyListFactory.Empty<T>(in Comparers<T> comparers) => Empty(in comparers);
    IReadOnlyList<T> IReadOnlyListFactory.New<T>(int capacity, in Comparers<T> comparers) => New(capacity, in comparers);
    IReadOnlyList<T> IReadOnlyListFactory.New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers) => New(capacity, builder, in comparers);
    IReadOnlyList<T> IReadOnlyListFactory.New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers) => New(capacity, builder, in state, in comparers);
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