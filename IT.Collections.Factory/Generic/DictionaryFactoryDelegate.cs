﻿namespace IT.Collections.Factory.Generic;

public class DictionaryFactoryDelegate<TDictionary, TKey, TValue> : IDictionaryFactory<TDictionary, TKey, TValue>
    where TDictionary : IEnumerable<KeyValuePair<TKey, TValue>>
{
    private readonly DictionaryFactory<TDictionary, TKey, TValue> _factory;
    private readonly Action<TDictionary, KeyValuePair<TKey, TValue>> _add;
    private readonly bool _reverse;

    public bool IsReadOnly => false;

    public DictionaryFactoryDelegate(
        DictionaryFactory<TDictionary, TKey, TValue> factory,
        Action<TDictionary, KeyValuePair<TKey, TValue>> add, bool reverse)
    {
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        _add = add ?? throw new ArgumentNullException(nameof(add));
        _reverse = reverse;
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

        builder(item => add(dictionary, item), _reverse);

        return dictionary;
    }

    public TDictionary New<TState>(int capacity, EnumerableBuilder<KeyValuePair<TKey, TValue>, TState> builder, in TState state)
    {
        if (capacity == 0) return _factory(0);
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var dictionary = _factory(capacity);
        var add = _add;

        builder(item => add(dictionary, item), _reverse, in state);

        return dictionary;
    }
}