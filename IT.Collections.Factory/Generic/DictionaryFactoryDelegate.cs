namespace IT.Collections.Factory.Generic;

public class DictionaryFactoryDelegate<TDictionary, TKey, TValue> : IDictionaryFactory<TDictionary, TKey, TValue>
    where TDictionary : IEnumerable<KeyValuePair<TKey, TValue>>
{
    private readonly DictionaryFactory<TDictionary, TKey, TValue> _factory;
    private readonly Action<TDictionary, KeyValuePair<TKey, TValue>> _add;
    private readonly EnumerableType _type;

    public EnumerableType Type => _type;

    public DictionaryFactoryDelegate(
        DictionaryFactory<TDictionary, TKey, TValue> factory,
        Action<TDictionary, KeyValuePair<TKey, TValue>> add,
        EnumerableType type = EnumerableType.None)
    {
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        _add = add ?? throw new ArgumentNullException(nameof(add));
        _type = type;
    }

    public TDictionary Empty() => _factory(0);

    public TDictionary New(int capacity)
    {
        if (capacity == 0) return _factory(0);

        return _factory(capacity);
    }

    public TDictionary New(int capacity, EnumerableBuilder<KeyValuePair<TKey, TValue>> builder)
    {
        if (capacity == 0) return _factory(0);
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var dictionary = _factory(capacity);
        var add = _add;

        builder(item => add(dictionary, item), _type == EnumerableType.Reverse);

        return dictionary;
    }

    public TDictionary New<TState>(int capacity, EnumerableBuilder<KeyValuePair<TKey, TValue>, TState> builder, in TState state)
    {
        if (capacity == 0) return _factory(0);
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var dictionary = _factory(capacity);
        var add = _add;

        builder(item => add(dictionary, item), _type == EnumerableType.Reverse, in state);

        return dictionary;
    }
}