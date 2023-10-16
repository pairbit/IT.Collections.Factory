namespace IT.Collections.Factory.Generic;

public class EnumerableFactoryProxy<TEnumerable, T> : IEnumerableFactory<TEnumerable, T> where TEnumerable : IEnumerable<T>
{
    private readonly IEnumerableFactory _factory;
    private readonly Comparers<T> _comparers;

    public EnumerableType Type => _factory.Type | EnumerableType.Proxy;

    public EnumerableFactoryProxy(IEnumerableFactory factory, in Comparers<T> comparers = default)
    {
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        _comparers = comparers;
    }

    public EnumerableFactoryProxy(IEnumerableFactory factory)
    {
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
    }

    public TEnumerable Empty() => (TEnumerable)_factory.Empty(in _comparers);

    public TEnumerable New(int capacity)
        => (TEnumerable)_factory.New(capacity, in _comparers);

    public TEnumerable New(int capacity, EnumerableBuilder<T> builder)
        => (TEnumerable)_factory.New(capacity, builder, in _comparers);

    public TEnumerable New<TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state)
        => (TEnumerable)_factory.New(capacity, builder, in state, in _comparers);
}