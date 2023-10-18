namespace IT.Collections.Factory.Generic;

public class DictionaryFactoryProxy<TDictionary, TKey, TValue> : IDictionaryFactory<TDictionary, TKey, TValue>
    where TKey : notnull
    where TDictionary : IEnumerable<KeyValuePair<TKey, TValue>>
{
    private readonly IEnumerableKeyValueFactory _factory;
    private readonly Comparers<TKey, TValue> _comparers;

    public Type EnumerableType => typeof(TDictionary);

    public EnumerableKind Kind => _factory.Kind | EnumerableKind.Proxy;

    public DictionaryFactoryProxy(IEnumerableKeyValueFactory factory, in Comparers<TKey, TValue> comparers = default)
    {
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        _comparers = comparers;
    }

    public DictionaryFactoryProxy(IEnumerableKeyValueFactory factory)
    {
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
    }

    public TDictionary Empty() => (TDictionary)_factory.Empty(in _comparers);

    public TDictionary New(int capacity)
        => (TDictionary)_factory.New(capacity, in _comparers);

    public TDictionary New(int capacity, EnumerableBuilder<KeyValuePair<TKey, TValue>> builder)
        => (TDictionary)_factory.New(capacity, builder, in _comparers);

    public TDictionary New<TState>(int capacity, EnumerableBuilder<KeyValuePair<TKey, TValue>, TState> builder, in TState state)
        => (TDictionary)_factory.New(capacity, builder, in state, in _comparers);
}