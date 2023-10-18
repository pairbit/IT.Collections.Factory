namespace IT.Collections.Factory.Generic;

public class DictionaryFactoryDelegate<TDictionary, TKey, TValue> : IDictionaryFactory<TDictionary, TKey, TValue>
    where TDictionary : IEnumerable<KeyValuePair<TKey, TValue>>
{
    private readonly DictionaryFactory<TDictionary, TKey, TValue> _factory;
    private readonly Func<TDictionary, KeyValuePair<TKey, TValue>, bool> _tryAdd;
    private readonly EnumerableKind _kind;

    public EnumerableKind Kind => _kind;

    public DictionaryFactoryDelegate(
        DictionaryFactory<TDictionary, TKey, TValue> factory,
        Func<TDictionary, KeyValuePair<TKey, TValue>, bool> tryAdd,
        EnumerableKind kind = EnumerableKind.None)
    {
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        _tryAdd = tryAdd ?? throw new ArgumentNullException(nameof(tryAdd));
        _kind = kind;
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
        var tryAdd = _tryAdd;

        builder(item => tryAdd(dictionary, item));

        return dictionary;
    }

    public TDictionary New<TState>(int capacity, EnumerableBuilder<KeyValuePair<TKey, TValue>, TState> builder, in TState state)
    {
        if (capacity == 0) return _factory(0);
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var dictionary = _factory(capacity);
        var tryAdd = _tryAdd;

        builder(item => tryAdd(dictionary, item), in state);

        return dictionary;
    }
}