namespace IT.Collections.Factory.Generic;

public delegate TDictionary DictionaryFactory<TDictionary, TKey, TValue>(int capacity) where TDictionary : IEnumerable<KeyValuePair<TKey, TValue>>;

public interface IDictionaryFactory<out TDictionary, TKey, TValue> : IEnumerableFactory<TDictionary, KeyValuePair<TKey, TValue>>
    where TDictionary : IEnumerable<KeyValuePair<TKey, TValue>>
{

}