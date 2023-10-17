namespace IT.Collections.Factory.Factories;

public class ConcurrentDictionaryFactory : IDictionaryFactory, IReadOnlyDictionaryFactory
{
    public static readonly ConcurrentDictionaryFactory Default = new();

    public virtual EnumerableType Type => EnumerableType.Unordered | EnumerableType.Unique | EnumerableType.EquatableKey;

    public virtual ConcurrentDictionary<TKey, TValue> Empty<TKey, TValue>(in Comparers<TKey, TValue> comparers = default) where TKey : notnull
        => new(comparers.KeyEqualityComparer
#if NET461_OR_GREATER
            ?? EqualityComparer<TKey>.Default
#endif
            );

    public virtual ConcurrentDictionary<TKey, TValue> New<TKey, TValue>(int capacity, in Comparers<TKey, TValue> comparers = default) where TKey : notnull
        => new(Environment.ProcessorCount, capacity, comparers.KeyEqualityComparer
#if NET461_OR_GREATER
            ?? EqualityComparer<TKey>.Default
#endif
            );

    public virtual ConcurrentDictionary<TKey, TValue> New<TKey, TValue>(int capacity, EnumerableBuilder<KeyValuePair<TKey, TValue>> builder, in Comparers<TKey, TValue> comparers = default) where TKey : notnull
    {
        if (capacity == 0) return new(comparers.KeyEqualityComparer
#if NET461_OR_GREATER
            ?? EqualityComparer<TKey>.Default
#endif
            );
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var dictionary = new ConcurrentDictionary<TKey, TValue>(Environment.ProcessorCount, capacity, comparers.KeyEqualityComparer
#if NET461_OR_GREATER
            ?? EqualityComparer<TKey>.Default
#endif
            );

        builder(item => dictionary.TryAdd(item.Key, item.Value));

        return dictionary;
    }

    public virtual ConcurrentDictionary<TKey, TValue> New<TKey, TValue, TState>(int capacity, EnumerableBuilder<KeyValuePair<TKey, TValue>, TState> builder, in TState state, in Comparers<TKey, TValue> comparers = default) where TKey : notnull
    {
        if (capacity == 0) return new(comparers.KeyEqualityComparer
#if NET461_OR_GREATER
            ?? EqualityComparer<TKey>.Default
#endif
            );
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var dictionary = new ConcurrentDictionary<TKey, TValue>(Environment.ProcessorCount, capacity, comparers.KeyEqualityComparer
#if NET461_OR_GREATER
            ?? EqualityComparer<TKey>.Default
#endif
            );

        builder(item => dictionary.TryAdd(item.Key, item.Value), in state);

        return dictionary;
    }

    IDictionary<TKey, TValue> IDictionaryFactory.Empty<TKey, TValue>(in Comparers<TKey, TValue> comparers) => Empty(in comparers);
    IDictionary<TKey, TValue> IDictionaryFactory.New<TKey, TValue>(int capacity, in Comparers<TKey, TValue> comparers) => New(capacity, in comparers);
    IDictionary<TKey, TValue> IDictionaryFactory.New<TKey, TValue>(int capacity, EnumerableBuilder<KeyValuePair<TKey, TValue>> builder, in Comparers<TKey, TValue> comparers) => New(capacity, builder, in comparers);
    IDictionary<TKey, TValue> IDictionaryFactory.New<TKey, TValue, TState>(int capacity, EnumerableBuilder<KeyValuePair<TKey, TValue>, TState> builder, in TState state, in Comparers<TKey, TValue> comparers) => New(capacity, builder, in state, in comparers);
    IReadOnlyDictionary<TKey, TValue> IReadOnlyDictionaryFactory.Empty<TKey, TValue>(in Comparers<TKey, TValue> comparers) => Empty(in comparers);
    IReadOnlyDictionary<TKey, TValue> IReadOnlyDictionaryFactory.New<TKey, TValue>(int capacity, in Comparers<TKey, TValue> comparers) => New(capacity, in comparers);
    IReadOnlyDictionary<TKey, TValue> IReadOnlyDictionaryFactory.New<TKey, TValue>(int capacity, EnumerableBuilder<KeyValuePair<TKey, TValue>> builder, in Comparers<TKey, TValue> comparers) => New(capacity, builder, in comparers);
    IReadOnlyDictionary<TKey, TValue> IReadOnlyDictionaryFactory.New<TKey, TValue, TState>(int capacity, EnumerableBuilder<KeyValuePair<TKey, TValue>, TState> builder, in TState state, in Comparers<TKey, TValue> comparers) => New(capacity, builder, in state, in comparers);
    IEnumerable<KeyValuePair<TKey, TValue>> IEnumerableKeyValueFactory.Empty<TKey, TValue>(in Comparers<TKey, TValue> comparers) => Empty(in comparers);
    IEnumerable<KeyValuePair<TKey, TValue>> IEnumerableKeyValueFactory.New<TKey, TValue>(int capacity, in Comparers<TKey, TValue> comparers) => New(capacity, in comparers);
    IEnumerable<KeyValuePair<TKey, TValue>> IEnumerableKeyValueFactory.New<TKey, TValue>(int capacity, EnumerableBuilder<KeyValuePair<TKey, TValue>> builder, in Comparers<TKey, TValue> comparers) => New(capacity, builder, in comparers);
    IEnumerable<KeyValuePair<TKey, TValue>> IEnumerableKeyValueFactory.New<TKey, TValue, TState>(int capacity, EnumerableBuilder<KeyValuePair<TKey, TValue>, TState> builder, in TState state, in Comparers<TKey, TValue> comparers) => New(capacity, builder, in state, in comparers);
}