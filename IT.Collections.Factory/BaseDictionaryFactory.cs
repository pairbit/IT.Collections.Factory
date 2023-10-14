#if NET5_0_OR_GREATER

namespace IT.Collections.Factory;

public abstract class BaseDictionaryFactory : IDictionaryFactory
{
    public abstract EnumerableType Type { get; }

    public abstract IEnumerable<KeyValuePair<TKey, TValue>> Empty<TKey, TValue>() where TKey : notnull;

    public abstract IEnumerable<KeyValuePair<TKey, TValue>> New<TKey, TValue>(int capacity) where TKey : notnull;

    public abstract IEnumerable<KeyValuePair<TKey, TValue>> New<TKey, TValue>(int capacity, EnumerableBuilder<KeyValuePair<TKey, TValue>> builder) where TKey : notnull;

    public abstract IEnumerable<KeyValuePair<TKey, TValue>> New<TKey, TValue, TState>(int capacity, EnumerableBuilder<KeyValuePair<TKey, TValue>, TState> builder, in TState state) where TKey : notnull;
}

#endif