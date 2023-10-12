namespace IT.Collections.Factory.Generic;

public class DictionaryFactoryDelegate<TDictionary, TKey, TValue> : IDictionaryFactory<TDictionary, TKey, TValue>
    where TDictionary : IEnumerable<KeyValuePair<TKey, TValue>>
{
    private readonly DictionaryFactory<TDictionary, TKey, TValue> _factory;

    public bool IsReadOnly => false;

    public DictionaryFactoryDelegate(DictionaryFactory<TDictionary, TKey, TValue> factory)
    {
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
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

        builder(dictionary);

        return dictionary;
    }

    public TDictionary New<TState>(int capacity, EnumerableBuilder<KeyValuePair<TKey, TValue>, TState> builder, in TState state)
    {
        if (capacity == 0) return _factory(0);
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var dictionary = _factory(capacity);

        builder(dictionary, in state);

        return dictionary;
    }
}