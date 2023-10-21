namespace IT.Collections.Factory.Factories;

using Internal;

public sealed class ReadOnlyCollectionFactoryProxy : IReadOnlyCollectionFactory, IEquatable<ReadOnlyCollectionFactoryProxy>
{
    private readonly IReadOnlyCollectionFactory _factory;

    public Type EnumerableType => typeof(IReadOnlyCollection<>);

    public EnumerableKind Kind => EnumerableKind.ReadOnly | _factory.Kind;

    public ReadOnlyCollectionFactoryProxy() : this(EnumerableFactoryCache<LinkedListFactory>.Default) { }

    public ReadOnlyCollectionFactoryProxy(IReadOnlyCollectionFactory factory)
    {
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
    }

    public IReadOnlyCollection<T> Empty<T>(in Comparers<T> comparers = default) => ReadOnlyCollection<T>.Empty;

    public IReadOnlyCollection<T> New<T>(int capacity, in Comparers<T> comparers = default)
        => throw new NotSupportedException();

    public IReadOnlyCollection<T> New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers = default)
    {
        if (capacity == 0) return ReadOnlyCollection<T>.Empty;

        return new ReadOnlyCollection<T>(_factory.New(capacity, builder, in comparers));
    }

    public IReadOnlyCollection<T> New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers = default)
    {
        if (capacity == 0) return ReadOnlyCollection<T>.Empty;

        return new ReadOnlyCollection<T>(_factory.New(capacity, builder, in state, in comparers));
    }

    public override int GetHashCode() => HashCode.Combine(GetType(), _factory);

    public override bool Equals(object? obj) => Equals(obj as ReadOnlyCollectionFactoryProxy);

    public bool Equals(ReadOnlyCollectionFactoryProxy? other)
        => this == other || (other != null && other.GetType() == GetType() &&
        (_factory == other._factory || (_factory != null && _factory.Equals(other._factory))));

    IEnumerable<T> IEnumerableFactory.Empty<T>(in Comparers<T> comparers) => Empty(in comparers);
    IEnumerable<T> IEnumerableFactory.New<T>(int capacity, in Comparers<T> comparers) => New(capacity, in comparers);
    IEnumerable<T> IEnumerableFactory.New<T>(int capacity, EnumerableBuilder<T> builder, in Comparers<T> comparers) => New(capacity, builder, in comparers);
    IEnumerable<T> IEnumerableFactory.New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state, in Comparers<T> comparers) => New(capacity, builder, in state, in comparers);
}