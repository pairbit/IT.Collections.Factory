namespace IT.Collections.Factory;

public interface IDictionaryFactory
{
    IDictionary<TKey, TValue> Empty<TKey, TValue>() where TKey : notnull;

    IDictionary<TKey, TValue> New<TKey, TValue, TState>(int capacity, in TState state, EnumerableBuilder<KeyValuePair<TKey, TValue>, TState> builder) where TKey : notnull;
}