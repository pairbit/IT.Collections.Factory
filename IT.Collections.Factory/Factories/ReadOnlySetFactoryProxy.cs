#if NET6_0_OR_GREATER

namespace IT.Collections.Factory.Factories;

using Internal;

public sealed class ReadOnlySetFactoryProxy : IReadOnlySetFactory, IEquatable<ReadOnlySetFactoryProxy>
{
    private readonly IReadOnlySetFactory _factory;

    public Type EnumerableType => typeof(IReadOnlySet<>);

    public EnumerableKind Kind => EnumerableKind.ReadOnly | EnumerableKind.Unique | EnumerableKind.Equatable;

    public ReadOnlySetFactoryProxy() : this(EnumerableFactoryCache<HashSetFactory>.Default) { }

    public ReadOnlySetFactoryProxy(IReadOnlySetFactory factory)
    {
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
    }

    public IReadOnlySet<T> Empty<T>(in Comparers<T> comparers = default) => ReadOnlySet<T>.Empty;

    public IReadOnlySet<T> New<T>(int capacity, in Comparers<T> comparers = default)
        => throw new NotSupportedException();

    public IReadOnlySet<T> New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers = default)
    {
        if (capacity == 0) return ReadOnlySet<T>.Empty;

        return new ReadOnlySet<T>(_factory.New(capacity, builder, in comparers));
    }

    public IReadOnlySet<T> New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers = default)
    {
        if (capacity == 0) return ReadOnlySet<T>.Empty;

        return new ReadOnlySet<T>(_factory.New(capacity, builder, in state, in comparers));
    }

    public override int GetHashCode() => HashCode.Combine(GetType(), _factory);

    public override bool Equals(object? obj) => Equals(obj as ReadOnlySetFactoryProxy);

    public bool Equals(ReadOnlySetFactoryProxy? other)
        => this == other || (other != null && other.GetType() == GetType() &&
        (_factory == other._factory || (_factory != null && _factory.Equals(other._factory))));

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