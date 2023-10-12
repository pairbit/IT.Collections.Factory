﻿namespace IT.Collections.Factory;

public abstract class DictionaryFactoryBase : IDictionaryFactory
{
    public virtual IDictionary<TKey, TValue> Empty<TKey, TValue>() where TKey : notnull => New<TKey, TValue>(0);

    public IDictionary<TKey, TValue> New<TKey, TValue, TState>(int capacity, in TState state, EnumerableBuilder<KeyValuePair<TKey, TValue>, TState> builder) where TKey : notnull
    {
        if (capacity == 0) return Empty<TKey, TValue>();

        var dictionary = New<TKey, TValue>(capacity);

        builder(dictionary, in state);

        return dictionary;
    }

    protected abstract IDictionary<TKey, TValue> New<TKey, TValue>(int capacity) where TKey : notnull;
}