namespace IT.Collections.Factory;

public abstract class DictionaryFactoryBase : IDictionaryFactory
{
    public bool IsReadOnly => false;

    public virtual IDictionary<TKey, TValue> Empty<TKey, TValue>() where TKey : notnull => New<TKey, TValue>(0);

    public abstract IDictionary<TKey, TValue> New<TKey, TValue>(int capacity) where TKey : notnull;

    public IDictionary<TKey, TValue> New<TKey, TValue>(int capacity, EnumerableBuilder<KeyValuePair<TKey, TValue>> builder) where TKey : notnull
    {
        if (capacity == 0) return Empty<TKey, TValue>();
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var dictionary = New<TKey, TValue>(capacity);

        builder(dictionary);

        return dictionary;
    }

    public IDictionary<TKey, TValue> New<TKey, TValue, TState>(int capacity, EnumerableBuilder<KeyValuePair<TKey, TValue>, TState> builder, in TState state) where TKey : notnull
    {
        if (capacity == 0) return Empty<TKey, TValue>();
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var dictionary = New<TKey, TValue>(capacity);

        builder(dictionary, in state);

        return dictionary;
    }
}