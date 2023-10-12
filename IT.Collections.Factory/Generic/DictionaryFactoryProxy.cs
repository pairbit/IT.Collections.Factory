namespace IT.Collections.Factory.Generic;

public class DictionaryFactoryProxy<TDictionary, TKey, TValue> : IDictionaryFactory<TDictionary, TKey, TValue>
    where TKey : notnull
    where TDictionary : IEnumerable<KeyValuePair<TKey, TValue>>
{
    private readonly IDictionaryFactory _factory;

    public bool IsReadOnly => _factory.IsReadOnly;

    public DictionaryFactoryProxy(IDictionaryFactory factory)
    {
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
    }

    public TDictionary Empty() => (TDictionary)_factory.Empty<TKey, TValue>();

    public TDictionary New(int capacity)
        => (TDictionary)_factory.New<TKey, TValue>(capacity);

    public TDictionary New(int capacity, EnumerableBuilder<KeyValuePair<TKey, TValue>> builder)
        => (TDictionary)_factory.New(capacity, builder);

    public TDictionary New<TState>(int capacity, EnumerableBuilder<KeyValuePair<TKey, TValue>, TState> builder, in TState state)
        => (TDictionary)_factory.New(capacity, builder, in state);
}