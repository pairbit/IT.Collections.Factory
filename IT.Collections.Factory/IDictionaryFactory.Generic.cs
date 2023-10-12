namespace IT.Collections.Factory;

public delegate TDictionary DictionaryFactory<TDictionary, TKey, TValue>(int capacity) where TDictionary : IEnumerable<KeyValuePair<TKey, TValue>>;

public interface IDictionaryFactory<TDictionary, TKey, TValue> : IEnumerableFactory<TDictionary, KeyValuePair<TKey, TValue>>
    where TDictionary : IEnumerable<KeyValuePair<TKey, TValue>>
{

}