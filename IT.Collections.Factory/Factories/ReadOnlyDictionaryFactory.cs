using System.Collections.ObjectModel;

namespace IT.Collections.Factory.Factories;

public class ReadOnlyDictionaryFactory : IDictionaryFactory
{
    public static readonly ReadOnlyDictionaryFactory Default = new();

    public EnumerableType Type => EnumerableType.ReadOnly;

    public IDictionary<TKey, TValue> Empty<TKey, TValue>() where TKey : notnull => Cache<TKey, TValue>.Empty;

    public IDictionary<TKey, TValue> New<TKey, TValue>(int capacity) where TKey : notnull
    {
        throw new NotSupportedException();
    }

    public IDictionary<TKey, TValue> New<TKey, TValue>(int capacity, EnumerableBuilder<KeyValuePair<TKey, TValue>> builder) where TKey : notnull
    {
        if (capacity == 0) return Cache<TKey, TValue>.Empty;
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        IDictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>(capacity);

        builder(dictionary.Add, false);

        return new ReadOnlyDictionary<TKey, TValue>(dictionary);
    }

    public IDictionary<TKey, TValue> New<TKey, TValue, TState>(int capacity, EnumerableBuilder<KeyValuePair<TKey, TValue>, TState> builder, in TState state) where TKey : notnull
    {
        if (capacity == 0) return Cache<TKey, TValue>.Empty;
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        IDictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>(capacity);

        builder(dictionary.Add, false, in state);

        return new ReadOnlyDictionary<TKey, TValue>(dictionary);
    }

    static class Cache<TKey, TValue> where TKey : notnull
    {
        public readonly static ReadOnlyDictionary<TKey, TValue> Empty = new(new Dictionary<TKey, TValue>());
    }
}