#if NETCOREAPP3_1_OR_GREATER

using System.Collections.Immutable;

namespace IT.Collections.Factory.Factories;

using Internal;

public sealed class ImmutableArrayFactory : IImmutableListFactory, IListFactory, IEquatable<ImmutableArrayFactory>
{
    public Type EnumerableType => typeof(ImmutableArray<>);

    public EnumerableKind Kind => EnumerableKind.Fixed;

    public ImmutableArray<T> Empty<T>(in Comparers<T> comparers = default) => ImmutableArray<T>.Empty;

    public ImmutableArray<T> New<T>(int capacity, in Comparers<T> comparers = default)
    {
        if (capacity == 0) return ImmutableArray<T>.Empty;
        if (capacity < 0) throw Ex.ArgumentNegative(capacity, nameof(capacity));

        return ImmutableArray.Create(new T[capacity]);
    }

    public ImmutableArray<T> New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers = default)
    {
        if (capacity == 0) return ImmutableArray<T>.Empty;
        if (builder == null) throw new ArgumentNullException(nameof(builder));
        if (capacity < 0) throw Ex.ArgumentNegative(capacity, nameof(capacity));

        var array = new T[capacity];
        var index = 0;

        builder(item => { array[index++] = item; return true; });

        return ImmutableArray.Create(array);
    }

    public ImmutableArray<T> New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers = default)
    {
        if (capacity == 0) return ImmutableArray<T>.Empty;
        if (builder == null) throw new ArgumentNullException(nameof(builder));
        if (capacity < 0) throw Ex.ArgumentNegative(capacity, nameof(capacity));

        var array = new T[capacity];
        var index = 0;

        builder(item => { array[index++] = item; return true; }, in state);

        return ImmutableArray.Create(array);
    }

    public override int GetHashCode() => HashCode.Combine(GetType());

    public override bool Equals(object? obj) => Equals(obj as ImmutableArrayFactory);

    public bool Equals(ImmutableArrayFactory? other) => this == other || (other != null && other.GetType() == GetType());

    IImmutableList<T> IImmutableListFactory.Empty<T>(in Comparers<T> comparers) => Empty(in comparers);
    IImmutableList<T> IImmutableListFactory.New<T>(int capacity, in Comparers<T> comparers) => New(capacity, in comparers);
    IImmutableList<T> IImmutableListFactory.New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers) => New(capacity, builder, in comparers);
    IImmutableList<T> IImmutableListFactory.New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers) => New(capacity, builder, in state, in comparers);
    IList<T> IListFactory.Empty<T>(in Comparers<T> comparers) => Empty(in comparers);
    IList<T> IListFactory.New<T>(int capacity, in Comparers<T> comparers) => New(capacity, in comparers);
    IList<T> IListFactory.New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers) => New(capacity, builder, in comparers);
    IList<T> IListFactory.New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers) => New(capacity, builder, in state, in comparers);
    ICollection<T> ICollectionFactory.Empty<T>(in Comparers<T> comparers) => Empty(in comparers);
    ICollection<T> ICollectionFactory.New<T>(int capacity, in Comparers<T> comparers) => New(capacity, in comparers);
    ICollection<T> ICollectionFactory.New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers) => New(capacity, builder, in comparers);
    ICollection<T> ICollectionFactory.New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers) => New(capacity, builder, in state, in comparers);
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