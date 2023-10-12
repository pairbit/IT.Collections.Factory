namespace IT.Collections.Factory.Generic;

public class EnumerableFactoryProxy<TEnumerable, T> : IEnumerableFactory<TEnumerable, T> where TEnumerable : IEnumerable<T>
{
    private readonly IEnumerableFactory _factory;

    public bool IsReadOnly => _factory.IsReadOnly;

    public EnumerableFactoryProxy(IEnumerableFactory factory)
    {
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
    }

    public TEnumerable Empty() => (TEnumerable)_factory.Empty<T>();

    public TEnumerable New(int capacity)
        => (TEnumerable)_factory.New<T>(capacity);

    public TEnumerable New(int capacity, EnumerableBuilder<T> builder)
        => (TEnumerable)_factory.New(capacity, builder);

    public TEnumerable New<TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state)
        => (TEnumerable)_factory.New(capacity, builder, in state);
}