namespace IT.Collections.Factory.Generic;

public class EnumerableFactoryDelegate<TEnumerable, T> : IEnumerableFactory<TEnumerable, T>
    where TEnumerable : IEnumerable<T>
{
    private readonly EnumerableFactory<TEnumerable, T> _factory;
    private readonly Action<TEnumerable, T> _add;
    private readonly bool _reverse;

    public bool IsReadOnly => false;

    public EnumerableFactoryDelegate(EnumerableFactory<TEnumerable, T> factory, Action<TEnumerable, T> add, bool reverse)
    {
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        _add = add ?? throw new ArgumentNullException(nameof(add));
        _reverse = reverse;
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
        var add = _add;

        builder(item => add(enumerable, item), _reverse);

        return enumerable;
    }

    public TEnumerable New<TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state)
    {
        if (capacity == 0) return _factory(0);
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var enumerable = _factory(capacity);
        var add = _add;

        builder(item => add(enumerable, item), _reverse, in state);

        return enumerable;
    }
}