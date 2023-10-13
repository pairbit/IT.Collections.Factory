#if NETCOREAPP3_1_OR_GREATER

using System.Collections.Immutable;

namespace IT.Collections.Factory.Factories;

public class ImmutableSortedDictionaryFactory : IDictionaryFactory
{
    public static readonly ImmutableSortedDictionaryFactory Default = new();

    public EnumerableType Type => EnumerableType.Ordered | EnumerableType.Unique;

    public IDictionary<TKey, TValue> Empty<TKey, TValue>() where TKey : notnull => ImmutableSortedDictionary<TKey, TValue>.Empty;

    public IDictionary<TKey, TValue> New<TKey, TValue>(int capacity) where TKey : notnull
        => ImmutableSortedDictionary<TKey, TValue>.Empty;

    public IDictionary<TKey, TValue> New<TKey, TValue>(int capacity, EnumerableBuilder<KeyValuePair<TKey, TValue>> builder) where TKey : notnull
    {
        if (capacity == 0) return ImmutableSortedDictionary<TKey, TValue>.Empty;
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var dictionaryBuilder = ImmutableSortedDictionary<TKey, TValue>.Empty.ToBuilder();

        builder(item => dictionaryBuilder.TryAdd(item.Key, item.Value), false);

        return dictionaryBuilder.ToImmutable();
    }

    public IDictionary<TKey, TValue> New<TKey, TValue, TState>(int capacity, EnumerableBuilder<KeyValuePair<TKey, TValue>, TState> builder, in TState state) where TKey : notnull
    {
        if (capacity == 0) return ImmutableSortedDictionary<TKey, TValue>.Empty;
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var dictionaryBuilder = ImmutableSortedDictionary<TKey, TValue>.Empty.ToBuilder();

        builder(item => dictionaryBuilder.TryAdd(item.Key, item.Value), false, in state);

        return dictionaryBuilder.ToImmutable();
    }
}

#endif