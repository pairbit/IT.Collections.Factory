namespace IT.Collections.Factory.Factories;

public class ConcurrentDictionaryFactory :
#if NET5_0_OR_GREATER
    BaseDictionaryFactory
#else
    IDictionaryFactory
#endif
{
    public static readonly ConcurrentDictionaryFactory Default = new();

    public
#if NET5_0_OR_GREATER
        override
#endif
        EnumerableType Type => EnumerableType.Unordered | EnumerableType.Unique;

    public
#if NET5_0_OR_GREATER
        override
#endif
        ConcurrentDictionary<TKey, TValue> Empty<TKey, TValue>(in Comparers<TKey, TValue> comparers = default)
#if !NET5_0_OR_GREATER
        where TKey : notnull
#endif
        => new();

    public
#if NET5_0_OR_GREATER
        override
#endif
        ConcurrentDictionary<TKey, TValue> New<TKey, TValue>(int capacity, in Comparers<TKey, TValue> comparers = default)
#if !NET5_0_OR_GREATER
        where TKey : notnull
#endif
        => new(Environment.ProcessorCount, capacity);

    public
#if NET5_0_OR_GREATER
        override
#endif
        ConcurrentDictionary<TKey, TValue> New<TKey, TValue>(int capacity, EnumerableBuilder<KeyValuePair<TKey, TValue>> builder, in Comparers<TKey, TValue> comparers = default)
#if !NET5_0_OR_GREATER
        where TKey : notnull
#endif
    {
        if (capacity == 0) return new();
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var dictionary = new ConcurrentDictionary<TKey, TValue>(Environment.ProcessorCount, capacity);

        builder(item => dictionary.TryAdd(item.Key, item.Value));

        return dictionary;
    }

    public
#if NET5_0_OR_GREATER
        override
#endif
        ConcurrentDictionary<TKey, TValue> New<TKey, TValue, TState>(int capacity, EnumerableBuilder<KeyValuePair<TKey, TValue>, TState> builder, in TState state, in Comparers<TKey, TValue> comparers = default)
#if !NET5_0_OR_GREATER
        where TKey : notnull
#endif
    {
        if (capacity == 0) return new();
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var dictionary = new ConcurrentDictionary<TKey, TValue>(Environment.ProcessorCount, capacity);

        builder(item => dictionary.TryAdd(item.Key, item.Value), in state);

        return dictionary;
    }
#if !NET5_0_OR_GREATER
    IEnumerable<KeyValuePair<TKey, TValue>> IDictionaryFactory.Empty<TKey, TValue>(in Comparers<TKey, TValue> comparers) => Empty(in comparers);
    IEnumerable<KeyValuePair<TKey, TValue>> IDictionaryFactory.New<TKey, TValue>(int capacity, in Comparers<TKey, TValue> comparers) => New(capacity, in comparers);
    IEnumerable<KeyValuePair<TKey, TValue>> IDictionaryFactory.New<TKey, TValue>(int capacity, EnumerableBuilder<KeyValuePair<TKey, TValue>> builder, in Comparers<TKey, TValue> comparers) => New(capacity, builder, in comparers);
    IEnumerable<KeyValuePair<TKey, TValue>> IDictionaryFactory.New<TKey, TValue, TState>(int capacity, EnumerableBuilder<KeyValuePair<TKey, TValue>, TState> builder, in TState state, in Comparers<TKey, TValue> comparers) => New(capacity, builder, in state, in comparers);
#endif
}