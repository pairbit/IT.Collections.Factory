#if NETCOREAPP3_1_OR_GREATER

using System.Collections.Immutable;

namespace IT.Collections.Factory.Factories;

public class ImmutableDictionaryFactory :
#if NET5_0_OR_GREATER
    BaseDictionaryFactory
#else
    IEnumerableKeyValueFactory
#endif
{
    public static readonly ImmutableDictionaryFactory Default = new();

    public
#if NET5_0_OR_GREATER
        override
#endif
        EnumerableType Type => EnumerableType.Ordered | EnumerableType.Unique | EnumerableType.Equatable;

    public
#if NET5_0_OR_GREATER
        override
#endif
        ImmutableDictionary<TKey, TValue> Empty<TKey, TValue>(in Comparers<TKey, TValue> comparers = default)
#if !NET5_0_OR_GREATER
        where TKey : notnull
#endif
        => ImmutableDictionary<TKey, TValue>.Empty.WithComparers(comparers.KeyEqualityComparer, comparers.ValueEqualityComparer);

    public
#if NET5_0_OR_GREATER
        override
#endif
        ImmutableDictionary<TKey, TValue> New<TKey, TValue>(int capacity, in Comparers<TKey, TValue> comparers = default)
#if !NET5_0_OR_GREATER
        where TKey : notnull
#endif
        => ImmutableDictionary<TKey, TValue>.Empty.WithComparers(comparers.KeyEqualityComparer, comparers.ValueEqualityComparer);

    public
#if NET5_0_OR_GREATER
        override
#endif
        ImmutableDictionary<TKey, TValue> New<TKey, TValue>(int capacity, EnumerableBuilder<KeyValuePair<TKey, TValue>> builder, in Comparers<TKey, TValue> comparers = default)
#if !NET5_0_OR_GREATER
        where TKey : notnull
#endif
    {
        if (capacity == 0) return ImmutableDictionary<TKey, TValue>.Empty.WithComparers(comparers.KeyEqualityComparer, comparers.ValueEqualityComparer);
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var dictionaryBuilder = ImmutableDictionary<TKey, TValue>.Empty.ToBuilder();
        var keyEqualityComparer = comparers.KeyEqualityComparer;
        if (keyEqualityComparer != null) dictionaryBuilder.KeyComparer = keyEqualityComparer;
        var valueEqualityComparer = comparers.ValueEqualityComparer;
        if (valueEqualityComparer != null) dictionaryBuilder.ValueComparer = valueEqualityComparer;

        builder(item => dictionaryBuilder.TryAdd(item.Key, item.Value));

        return dictionaryBuilder.ToImmutable();
    }

    public
#if NET5_0_OR_GREATER
        override
#endif
        ImmutableDictionary<TKey, TValue> New<TKey, TValue, TState>(int capacity, EnumerableBuilder<KeyValuePair<TKey, TValue>, TState> builder, in TState state, in Comparers<TKey, TValue> comparers = default)
#if !NET5_0_OR_GREATER
        where TKey : notnull
#endif
    {
        if (capacity == 0) return ImmutableDictionary<TKey, TValue>.Empty.WithComparers(comparers.KeyEqualityComparer, comparers.ValueEqualityComparer);
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var dictionaryBuilder = ImmutableDictionary<TKey, TValue>.Empty.ToBuilder();
        var keyEqualityComparer = comparers.KeyEqualityComparer;
        if (keyEqualityComparer != null) dictionaryBuilder.KeyComparer = keyEqualityComparer;
        var valueEqualityComparer = comparers.ValueEqualityComparer;
        if (valueEqualityComparer != null) dictionaryBuilder.ValueComparer = valueEqualityComparer;

        builder(item => dictionaryBuilder.TryAdd(item.Key, item.Value), in state);

        return dictionaryBuilder.ToImmutable();
    }
#if !NET5_0_OR_GREATER
    IEnumerable<KeyValuePair<TKey, TValue>> IEnumerableKeyValueFactory.Empty<TKey, TValue>(in Comparers<TKey, TValue> comparers) => Empty(in comparers);
    IEnumerable<KeyValuePair<TKey, TValue>> IEnumerableKeyValueFactory.New<TKey, TValue>(int capacity, in Comparers<TKey, TValue> comparers) => New(capacity, in comparers);
    IEnumerable<KeyValuePair<TKey, TValue>> IEnumerableKeyValueFactory.New<TKey, TValue>(int capacity, EnumerableBuilder<KeyValuePair<TKey, TValue>> builder, in Comparers<TKey, TValue> comparers) => New(capacity, builder, in comparers);
    IEnumerable<KeyValuePair<TKey, TValue>> IEnumerableKeyValueFactory.New<TKey, TValue, TState>(int capacity, EnumerableBuilder<KeyValuePair<TKey, TValue>, TState> builder, in TState state, in Comparers<TKey, TValue> comparers) => New(capacity, builder, in state, in comparers);
#endif
}

#endif