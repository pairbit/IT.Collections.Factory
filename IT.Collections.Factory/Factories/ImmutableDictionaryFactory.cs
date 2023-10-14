#if NETCOREAPP3_1_OR_GREATER

using System.Collections.Immutable;

namespace IT.Collections.Factory.Factories;

public class ImmutableDictionaryFactory :
#if NET5_0_OR_GREATER
    BaseDictionaryFactory
#else
    IDictionaryFactory
#endif
{
    public static readonly ImmutableDictionaryFactory Default = new();

    public
#if NET5_0_OR_GREATER
        override
#endif
        EnumerableType Type => EnumerableType.Ordered | EnumerableType.Unique;

    public
#if NET5_0_OR_GREATER
        override
#endif
        ImmutableDictionary<TKey, TValue> Empty<TKey, TValue>()
#if !NET5_0_OR_GREATER
        where TKey : notnull
#endif
        => ImmutableDictionary<TKey, TValue>.Empty;

    public
#if NET5_0_OR_GREATER
        override
#endif
        ImmutableDictionary<TKey, TValue> New<TKey, TValue>(int capacity)
#if !NET5_0_OR_GREATER
        where TKey : notnull
#endif
        => ImmutableDictionary<TKey, TValue>.Empty;

    public
#if NET5_0_OR_GREATER
        override
#endif
        ImmutableDictionary<TKey, TValue> New<TKey, TValue>(int capacity, EnumerableBuilder<KeyValuePair<TKey, TValue>> builder)
#if !NET5_0_OR_GREATER
        where TKey : notnull
#endif
    {
        if (capacity == 0) return ImmutableDictionary<TKey, TValue>.Empty;
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var dictionaryBuilder = ImmutableDictionary<TKey, TValue>.Empty.ToBuilder();

        builder(item => dictionaryBuilder.TryAdd(item.Key, item.Value));

        return dictionaryBuilder.ToImmutable();
    }

    public
#if NET5_0_OR_GREATER
        override
#endif
        ImmutableDictionary<TKey, TValue> New<TKey, TValue, TState>(int capacity, EnumerableBuilder<KeyValuePair<TKey, TValue>, TState> builder, in TState state)
#if !NET5_0_OR_GREATER
        where TKey : notnull
#endif
    {
        if (capacity == 0) return ImmutableDictionary<TKey, TValue>.Empty;
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var dictionaryBuilder = ImmutableDictionary<TKey, TValue>.Empty.ToBuilder();

        builder(item => dictionaryBuilder.TryAdd(item.Key, item.Value), in state);

        return dictionaryBuilder.ToImmutable();
    }
#if !NET5_0_OR_GREATER
    IEnumerable<KeyValuePair<TKey, TValue>> IDictionaryFactory.Empty<TKey, TValue>() => Empty<TKey, TValue>();
    IEnumerable<KeyValuePair<TKey, TValue>> IDictionaryFactory.New<TKey, TValue>(int capacity) => New<TKey, TValue>(capacity);
    IEnumerable<KeyValuePair<TKey, TValue>> IDictionaryFactory.New<TKey, TValue>(int capacity, EnumerableBuilder<KeyValuePair<TKey, TValue>> builder) => New(capacity, builder);
    IEnumerable<KeyValuePair<TKey, TValue>> IDictionaryFactory.New<TKey, TValue, TState>(int capacity, EnumerableBuilder<KeyValuePair<TKey, TValue>, TState> builder, in TState state) => New(capacity, builder, in state);
#endif
}

#endif