using System.Collections;

namespace IT.Collections.Factory;

public abstract class EnumerableFactoryRegistry<TDictionary> : IEnumerableFactoryRegistry
    where TDictionary : IReadOnlyDictionary<Type, object>
{
    protected readonly TDictionary _dictionary;

    protected EnumerableFactoryRegistry(TDictionary dictionary)
    {
        _dictionary = dictionary ?? throw new ArgumentNullException(nameof(dictionary));
    }

    public abstract bool Register(Type type, object factory, bool overwrite);

    public abstract void Clear();

    #region IReadOnlyDictionary

    public object this[Type key] => _dictionary[key];

    public IEnumerable<Type> Keys => _dictionary.Keys;

    public IEnumerable<object> Values => _dictionary.Values;

    public int Count => _dictionary.Count;

    public bool ContainsKey(Type key) => _dictionary.ContainsKey(key);

    public IEnumerator<KeyValuePair<Type, object>> GetEnumerator() => _dictionary.GetEnumerator();

    public bool TryGetValue(Type key,
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
        [System.Diagnostics.CodeAnalysis.MaybeNullWhen(false)]
#endif
        out object value) => _dictionary.TryGetValue(key, out value);

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_dictionary).GetEnumerator();

    #endregion IReadOnlyDictionary
}