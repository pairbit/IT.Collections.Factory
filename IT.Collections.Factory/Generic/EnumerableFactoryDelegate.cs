namespace IT.Collections.Factory.Generic;

public class EnumerableFactoryDelegate<TEnumerable, T> : IEnumerableFactory<TEnumerable, T>
    where TEnumerable : IEnumerable<T>
{
    private readonly EnumerableFactory<TEnumerable, T> _factory;

    public bool IsReadOnly => false;

    public EnumerableFactoryDelegate(EnumerableFactory<TEnumerable, T> factory)
    {
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
    }

    public TEnumerable Empty() => _factory(0);

    public TEnumerable New(int capacity)
    {
        if (capacity == 0) return _factory(0);

        return _factory(capacity);
    }

    public TEnumerable New(int capacity, EnumerableBuilder<T> builder)
    {
        if (capacity == 0) return _factory(0);
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var enumerable = _factory(capacity);

        builder(enumerable);

        return enumerable;
    }

    public TEnumerable New<TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state)
    {
        if (capacity == 0) return _factory(0);
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var enumerable = _factory(capacity);

        builder(enumerable, in state);

        return enumerable;
    }
}