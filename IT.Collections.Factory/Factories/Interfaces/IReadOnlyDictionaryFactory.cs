namespace IT.Collections.Factory.Factories;

public interface IReadOnlyDictionaryFactory : IDictionaryFactory
{
    new IReadOnlyDictionary<TKey, TValue> Empty<TKey, TValue>(in Comparers<TKey, TValue> comparers = default) where TKey : notnull;

    new IReadOnlyDictionary<TKey, TValue> New<TKey, TValue>(int capacity, in Comparers<TKey, TValue> comparers = default) where TKey : notnull;

    new IReadOnlyDictionary<TKey, TValue> New<TKey, TValue>(int capacity, EnumerableBuilder<KeyValuePair<TKey, TValue>> builder, in Comparers<TKey, TValue> comparers = default) where TKey : notnull;

    new IReadOnlyDictionary<TKey, TValue> New<TKey, TValue, TState>(int capacity, EnumerableBuilder<KeyValuePair<TKey, TValue>, TState> builder, in TState state, in Comparers<TKey, TValue> comparers = default) where TKey : notnull;
}