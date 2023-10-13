namespace IT.Collections.Factory.Generic;

public class EnumerableFactoryDelegate<TEnumerable, T> : IEnumerableFactory<TEnumerable, T>
    where TEnumerable : IEnumerable<T>
{
    private readonly EnumerableFactory<TEnumerable, T> _factory;
    private readonly Func<TEnumerable, T, bool> _tryAdd;
    private readonly EnumerableType _type;

    public EnumerableType Type => _type;

    public EnumerableFactoryDelegate(EnumerableFactory<TEnumerable, T> factory,
        Func<TEnumerable, T, bool> tryAdd,
        EnumerableType type = EnumerableType.None)
    {
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        _tryAdd = tryAdd ?? throw new ArgumentNullException(nameof(tryAdd));
        _type = type;
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
        var tryAdd = _tryAdd;

        builder(item => tryAdd(enumerable, item), _type == EnumerableType.Reverse);

        return enumerable;
    }

    public TEnumerable New<TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state)
    {
        if (capacity == 0) return _factory(0);
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var enumerable = _factory(capacity);
        var tryAdd = _tryAdd;

        builder(item => tryAdd(enumerable, item), _type == EnumerableType.Reverse, in state);

        return enumerable;
    }
}