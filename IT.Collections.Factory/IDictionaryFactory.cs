namespace IT.Collections.Factory;

public interface IDictionaryFactory : IEnumerableFactoryRegistrable
{
    IEnumerable<KeyValuePair<TKey, TValue>> Empty<TKey, TValue>(in Comparers<TKey, TValue> comparers = default) where TKey : notnull;

    IEnumerable<KeyValuePair<TKey, TValue>> New<TKey, TValue>(int capacity, in Comparers<TKey, TValue> comparers = default) where TKey : notnull;

    IEnumerable<KeyValuePair<TKey, TValue>> New<TKey, TValue>(int capacity, EnumerableBuilder<KeyValuePair<TKey, TValue>> builder, in Comparers<TKey, TValue> comparers = default) where TKey : notnull;

    IEnumerable<KeyValuePair<TKey, TValue>> New<TKey, TValue, TState>(int capacity, EnumerableBuilder<KeyValuePair<TKey, TValue>, TState> builder, in TState state, in Comparers<TKey, TValue> comparers = default) where TKey : notnull;
}