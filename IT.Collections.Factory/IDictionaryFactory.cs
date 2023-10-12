namespace IT.Collections.Factory;

public interface IDictionaryFactory
{
    bool IsReadOnly { get; }

    IDictionary<TKey, TValue> Empty<TKey, TValue>() where TKey : notnull;

    IDictionary<TKey, TValue> New<TKey, TValue>(int capacity) where TKey : notnull;

    IDictionary<TKey, TValue> New<TKey, TValue>(int capacity, EnumerableBuilder<KeyValuePair<TKey, TValue>> builder) where TKey : notnull;

    IDictionary<TKey, TValue> New<TKey, TValue, TState>(int capacity, EnumerableBuilder<KeyValuePair<TKey, TValue>, TState> builder, in TState state) where TKey : notnull;
}