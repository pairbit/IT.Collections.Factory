namespace IT.Collections.Factory.Factories;

public class SortedListFactory :
#if NET5_0_OR_GREATER
    BaseDictionaryFactory
#else
    IDictionaryFactory
#endif
{
    public static readonly SortedListFactory Default = new();

    public
#if NET5_0_OR_GREATER
        override
#endif
        EnumerableType Type => EnumerableType.Ordered | EnumerableType.Unique;

    public
#if NET5_0_OR_GREATER
        override
#endif
        SortedList<TKey, TValue> Empty<TKey, TValue>()
#if !NET5_0_OR_GREATER
        where TKey : notnull
#endif
        => new();

    public
#if NET5_0_OR_GREATER
        override
#endif
        SortedList<TKey, TValue> New<TKey, TValue>(int capacity)
#if !NET5_0_OR_GREATER
        where TKey : notnull
#endif
        => new(capacity);

    public
#if NET5_0_OR_GREATER
        override
#endif
        SortedList<TKey, TValue> New<TKey, TValue>(int capacity, EnumerableBuilder<KeyValuePair<TKey, TValue>> builder)
#if !NET5_0_OR_GREATER
        where TKey : notnull
#endif
    {
        if (capacity == 0) return new();
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var dictionary = new SortedList<TKey, TValue>(capacity);

        builder(item => dictionary.TryAdd(item.Key, item.Value));

        return dictionary;
    }

    public
#if NET5_0_OR_GREATER
        override
#endif
        SortedList<TKey, TValue> New<TKey, TValue, TState>(int capacity, EnumerableBuilder<KeyValuePair<TKey, TValue>, TState> builder, in TState state)
#if !NET5_0_OR_GREATER
        where TKey : notnull
#endif
    {
        if (capacity == 0) return new();
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var dictionary = new SortedList<TKey, TValue>(capacity);

        builder(item => dictionary.TryAdd(item.Key, item.Value), in state);

        return dictionary;
    }
#if !NET5_0_OR_GREATER
    IEnumerable<KeyValuePair<TKey, TValue>> IDictionaryFactory.Empty<TKey, TValue>() => Empty<TKey, TValue>();
    IEnumerable<KeyValuePair<TKey, TValue>> IDictionaryFactory.New<TKey, TValue>(int capacity) => New<TKey, TValue>(capacity);
    IEnumerable<KeyValuePair<TKey, TValue>> IDictionaryFactory.New<TKey, TValue>(int capacity, EnumerableBuilder<KeyValuePair<TKey, TValue>> builder) => New(capacity, builder);
    IEnumerable<KeyValuePair<TKey, TValue>> IDictionaryFactory.New<TKey, TValue, TState>(int capacity, EnumerableBuilder<KeyValuePair<TKey, TValue>, TState> builder, in TState state) => New(capacity, builder, in state);
#endif
}