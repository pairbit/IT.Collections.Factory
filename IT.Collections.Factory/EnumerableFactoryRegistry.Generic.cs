namespace IT.Collections.Factory;

public abstract class EnumerableFactoryRegistry<TDictionary> : IEnumerableFactoryRegistry
    where TDictionary : IReadOnlyDictionary<Type, IEnumerableFactoryRegistrable>
{
    protected readonly TDictionary _dictionary;

    protected EnumerableFactoryRegistry(TDictionary dictionary)
    {
        _dictionary = dictionary ?? throw new ArgumentNullException(nameof(dictionary));
    }

    public abstract void Clear();

    public abstract bool TryRegister(Type type, IEnumerableFactoryRegistrable factory, RegistrationBehavior behavior);

    public virtual bool TryRegister<TFactory>(TFactory factory, RegistrationBehavior behavior) where TFactory : IEnumerableFactoryRegistrable
        => TryRegister(typeof(TFactory), factory, behavior);

    public virtual TFactory? TryGetFactory<TFactory>() where TFactory : IEnumerableFactoryRegistrable
        => _dictionary.TryGetValue(typeof(TFactory), out var factory) ? (TFactory)factory : default;

    #region IReadOnlyDictionary

    public IEnumerableFactoryRegistrable this[Type key] => _dictionary[key];

    public IEnumerable<Type> Keys => _dictionary.Keys;

    public IEnumerable<IEnumerableFactoryRegistrable> Values => _dictionary.Values;

    public int Count => _dictionary.Count;

    public bool ContainsKey(Type key) => _dictionary.ContainsKey(key);

    public IEnumerator<KeyValuePair<Type, IEnumerableFactoryRegistrable>> GetEnumerator() => _dictionary.GetEnumerator();

    public bool TryGetValue(Type key,
#if NETCOREAPP3_1_OR_GREATER
        [System.Diagnostics.CodeAnalysis.MaybeNullWhen(false)]
#endif
        out IEnumerableFactoryRegistrable value) => _dictionary.TryGetValue(key, out value);

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_dictionary).GetEnumerator();

    #endregion IReadOnlyDictionary
}