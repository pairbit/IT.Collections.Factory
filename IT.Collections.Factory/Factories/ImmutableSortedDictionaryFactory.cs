#if NETCOREAPP3_1_OR_GREATER

using System.Collections.Immutable;

namespace IT.Collections.Factory.Factories;

public class ImmutableSortedDictionaryFactory :
#if NET5_0_OR_GREATER
    BaseDictionaryFactory
#else
    IDictionaryFactory
#endif
{
    public static readonly ImmutableSortedDictionaryFactory Default = new();

    public
#if NET5_0_OR_GREATER
        override
#endif
        EnumerableType Type => EnumerableType.Ordered | EnumerableType.Unique;

    public
#if NET5_0_OR_GREATER
        override
#endif
        ImmutableSortedDictionary<TKey, TValue> Empty<TKey, TValue>(in Comparers<TKey, TValue> comparers = default)
#if !NET5_0_OR_GREATER
        where TKey : notnull
#endif
        => ImmutableSortedDictionary<TKey, TValue>.Empty;

    public
#if NET5_0_OR_GREATER
        override
#endif
        ImmutableSortedDictionary<TKey, TValue> New<TKey, TValue>(int capacity, in Comparers<TKey, TValue> comparers = default)
#if !NET5_0_OR_GREATER
        where TKey : notnull
#endif
        => ImmutableSortedDictionary<TKey, TValue>.Empty;

    public
#if NET5_0_OR_GREATER
        override
#endif
        ImmutableSortedDictionary<TKey, TValue> New<TKey, TValue>(int capacity, EnumerableBuilder<KeyValuePair<TKey, TValue>> builder, in Comparers<TKey, TValue> comparers = default)
#if !NET5_0_OR_GREATER
        where TKey : notnull
#endif
    {
        if (capacity == 0) return ImmutableSortedDictionary<TKey, TValue>.Empty;
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var dictionaryBuilder = ImmutableSortedDictionary<TKey, TValue>.Empty.ToBuilder();

        builder(item => dictionaryBuilder.TryAdd(item.Key, item.Value));

        return dictionaryBuilder.ToImmutable();
    }

    public
#if NET5_0_OR_GREATER
        override
#endif
        ImmutableSortedDictionary<TKey, TValue> New<TKey, TValue, TState>(int capacity, EnumerableBuilder<KeyValuePair<TKey, TValue>, TState> builder, in TState state, in Comparers<TKey, TValue> comparers = default)
#if !NET5_0_OR_GREATER
        where TKey : notnull
#endif
    {
        if (capacity == 0) return ImmutableSortedDictionary<TKey, TValue>.Empty;
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var dictionaryBuilder = ImmutableSortedDictionary<TKey, TValue>.Empty.ToBuilder();

        builder(item => dictionaryBuilder.TryAdd(item.Key, item.Value), in state);

        return dictionaryBuilder.ToImmutable();
    }
#if !NET5_0_OR_GREATER
    IEnumerable<KeyValuePair<TKey, TValue>> IDictionaryFactory.Empty<TKey, TValue>(in Comparers<TKey, TValue> comparers) => Empty(in comparers);
    IEnumerable<KeyValuePair<TKey, TValue>> IDictionaryFactory.New<TKey, TValue>(int capacity, in Comparers<TKey, TValue> comparers) => New(capacity, in comparers);
    IEnumerable<KeyValuePair<TKey, TValue>> IDictionaryFactory.New<TKey, TValue>(int capacity, EnumerableBuilder<KeyValuePair<TKey, TValue>> builder, in Comparers<TKey, TValue> comparers) => New(capacity, builder, in comparers);
    IEnumerable<KeyValuePair<TKey, TValue>> IDictionaryFactory.New<TKey, TValue, TState>(int capacity, EnumerableBuilder<KeyValuePair<TKey, TValue>, TState> builder, in TState state, in Comparers<TKey, TValue> comparers) => New(capacity, builder, in state, in comparers);
#endif
}

#endif