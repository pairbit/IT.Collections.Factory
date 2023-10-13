using System.Collections.Concurrent;

namespace IT.Collections.Factory.Factories;

public class ConcurrentDictionaryFactory : IDictionaryFactory
{
    public static readonly ConcurrentDictionaryFactory Default = new();

    public EnumerableType Type => EnumerableType.Unordered | EnumerableType.Unique;

    public IDictionary<TKey, TValue> Empty<TKey, TValue>() where TKey : notnull
        => new ConcurrentDictionary<TKey, TValue>();

    public IDictionary<TKey, TValue> New<TKey, TValue>(int capacity) where TKey : notnull
        => new ConcurrentDictionary<TKey, TValue>(Environment.ProcessorCount, capacity);

    public IDictionary<TKey, TValue> New<TKey, TValue>(int capacity, EnumerableBuilder<KeyValuePair<TKey, TValue>> builder) where TKey : notnull
    {
        if (capacity == 0) return new ConcurrentDictionary<TKey, TValue>();
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var dictionary = new ConcurrentDictionary<TKey, TValue>(Environment.ProcessorCount, capacity);

        builder(item => dictionary.TryAdd(item.Key, item.Value), false);

        return dictionary;
    }

    public IDictionary<TKey, TValue> New<TKey, TValue, TState>(int capacity, EnumerableBuilder<KeyValuePair<TKey, TValue>, TState> builder, in TState state) where TKey : notnull
    {
        if (capacity == 0) return new ConcurrentDictionary<TKey, TValue>();
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var dictionary = new ConcurrentDictionary<TKey, TValue>(Environment.ProcessorCount, capacity);

        builder(item => dictionary.TryAdd(item.Key, item.Value), false, in state);

        return dictionary;
    }
}