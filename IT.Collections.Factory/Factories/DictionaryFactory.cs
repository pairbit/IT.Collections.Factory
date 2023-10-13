namespace IT.Collections.Factory.Factories;

public class DictionaryFactory : IDictionaryFactory
{
    public static readonly DictionaryFactory Default = new();

    public EnumerableType Type => EnumerableType.Unique;

    public IDictionary<TKey, TValue> Empty<TKey, TValue>() where TKey : notnull
        => new Dictionary<TKey, TValue>();

    public IDictionary<TKey, TValue> New<TKey, TValue>(int capacity) where TKey : notnull
        => new Dictionary<TKey, TValue>(capacity, null);

    public IDictionary<TKey, TValue> New<TKey, TValue>(int capacity, EnumerableBuilder<KeyValuePair<TKey, TValue>> builder) where TKey : notnull
    {
        if (capacity == 0) return new Dictionary<TKey, TValue>();
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var dictionary = new Dictionary<TKey, TValue>(capacity, null);

        builder(item => dictionary.TryAdd(item.Key, item.Value), false);

        return dictionary;
    }

    public IDictionary<TKey, TValue> New<TKey, TValue, TState>(int capacity, EnumerableBuilder<KeyValuePair<TKey, TValue>, TState> builder, in TState state) where TKey : notnull
    {
        if (capacity == 0) return new Dictionary<TKey, TValue>();
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var dictionary = new Dictionary<TKey, TValue>(capacity, null);

        builder(item => dictionary.TryAdd(item.Key, item.Value), false, in state);

        return dictionary;
    }
}