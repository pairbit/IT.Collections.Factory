namespace IT.Collections.Factory.Factories;

public class SortedDictionaryFactory : IDictionaryFactory
{
    public static readonly SortedDictionaryFactory Default = new();

    public virtual EnumerableType Type => EnumerableType.Ordered | EnumerableType.Unique;

    public virtual IDictionary<TKey, TValue> Empty<TKey, TValue>() where TKey : notnull
        => new SortedDictionary<TKey, TValue>();

    public IDictionary<TKey, TValue> New<TKey, TValue>(int capacity) where TKey : notnull
        => new SortedDictionary<TKey, TValue>();

    public IDictionary<TKey, TValue> New<TKey, TValue>(int capacity, EnumerableBuilder<KeyValuePair<TKey, TValue>> builder) where TKey : notnull
    {
        if (capacity == 0) return new SortedDictionary<TKey, TValue>();
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var dictionary = new SortedDictionary<TKey, TValue>();

        builder(item => dictionary.TryAdd(item.Key, item.Value), false);

        return dictionary;
    }

    public IDictionary<TKey, TValue> New<TKey, TValue, TState>(int capacity, EnumerableBuilder<KeyValuePair<TKey, TValue>, TState> builder, in TState state) where TKey : notnull
    {
        if (capacity == 0) return new SortedDictionary<TKey, TValue>();
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var dictionary = new SortedDictionary<TKey, TValue>();

        builder(item => dictionary.TryAdd(item.Key, item.Value), false, in state);

        return dictionary;
    }
}