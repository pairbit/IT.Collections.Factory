using System.Collections.ObjectModel;

namespace IT.Collections.Factory.Factories;

public class ReadOnlyDictionaryFactory :
#if NET5_0_OR_GREATER
    BaseDictionaryFactory
#else
    IDictionaryFactory
#endif
{
    public static readonly ReadOnlyDictionaryFactory Default = new();

    public
#if NET5_0_OR_GREATER
        override
#endif
        EnumerableType Type => EnumerableType.ReadOnly | EnumerableType.Unique;

    public
#if NET5_0_OR_GREATER
        override
#endif
        ReadOnlyDictionary<TKey, TValue> Empty<TKey, TValue>()
#if !NET5_0_OR_GREATER
        where TKey : notnull
#endif
        => Cache<TKey, TValue>.Empty;

    public
#if NET5_0_OR_GREATER
        override
#endif
        ReadOnlyDictionary<TKey, TValue> New<TKey, TValue>(int capacity)
#if !NET5_0_OR_GREATER
        where TKey : notnull
#endif
    {
        throw new NotSupportedException();
    }

    public
#if NET5_0_OR_GREATER
        override
#endif
        ReadOnlyDictionary<TKey, TValue> New<TKey, TValue>(int capacity, EnumerableBuilder<KeyValuePair<TKey, TValue>> builder)
#if !NET5_0_OR_GREATER
        where TKey : notnull
#endif
    {
        if (capacity == 0) return Cache<TKey, TValue>.Empty;
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var dictionary = new Dictionary<TKey, TValue>(capacity);

        builder(item => dictionary.TryAdd(item.Key, item.Value));

        return new(dictionary);
    }

    public
#if NET5_0_OR_GREATER
        override
#endif
        ReadOnlyDictionary<TKey, TValue> New<TKey, TValue, TState>(int capacity, EnumerableBuilder<KeyValuePair<TKey, TValue>, TState> builder, in TState state)
#if !NET5_0_OR_GREATER
        where TKey : notnull
#endif
    {
        if (capacity == 0) return Cache<TKey, TValue>.Empty;
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var dictionary = new Dictionary<TKey, TValue>(capacity);

        builder(item => dictionary.TryAdd(item.Key, item.Value), in state);

        return new(dictionary);
    }

    static class Cache<TKey, TValue> where TKey : notnull
    {
        public readonly static ReadOnlyDictionary<TKey, TValue> Empty = new(new Dictionary<TKey, TValue>());
    }
#if !NET5_0_OR_GREATER
    IEnumerable<KeyValuePair<TKey, TValue>> IDictionaryFactory.Empty<TKey, TValue>() => Empty<TKey, TValue>();
    IEnumerable<KeyValuePair<TKey, TValue>> IDictionaryFactory.New<TKey, TValue>(int capacity) => New<TKey, TValue>(capacity);
    IEnumerable<KeyValuePair<TKey, TValue>> IDictionaryFactory.New<TKey, TValue>(int capacity, EnumerableBuilder<KeyValuePair<TKey, TValue>> builder) => New(capacity, builder);
    IEnumerable<KeyValuePair<TKey, TValue>> IDictionaryFactory.New<TKey, TValue, TState>(int capacity, EnumerableBuilder<KeyValuePair<TKey, TValue>, TState> builder, in TState state) => New(capacity, builder, in state);
#endif
}