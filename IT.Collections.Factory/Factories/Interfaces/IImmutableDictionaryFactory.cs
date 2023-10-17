#if NETCOREAPP3_1_OR_GREATER

using System.Collections.Immutable;

namespace IT.Collections.Factory.Factories;

public interface IImmutableDictionaryFactory : IReadOnlyDictionaryFactory
{
    new IImmutableDictionary<TKey, TValue> Empty<TKey, TValue>(in Comparers<TKey, TValue> comparers = default) where TKey : notnull;

    new IImmutableDictionary<TKey, TValue> New<TKey, TValue>(int capacity, in Comparers<TKey, TValue> comparers = default) where TKey : notnull;

    new IImmutableDictionary<TKey, TValue> New<TKey, TValue>(int capacity, EnumerableBuilder<KeyValuePair<TKey, TValue>> builder, in Comparers<TKey, TValue> comparers = default) where TKey : notnull;

    new IImmutableDictionary<TKey, TValue> New<TKey, TValue, TState>(int capacity, EnumerableBuilder<KeyValuePair<TKey, TValue>, TState> builder, in TState state, in Comparers<TKey, TValue> comparers = default) where TKey : notnull;
}

#endif