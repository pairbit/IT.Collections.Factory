namespace IT.Collections.Factory.Factories;

public class DictionaryFactory : IDictionaryFactory, IReadOnlyDictionaryFactory, IEquatable<DictionaryFactory>
{
    public static readonly DictionaryFactory Default = new();

    public virtual Type EnumerableType => typeof(Dictionary<,>);

    public virtual EnumerableKind Kind => EnumerableKind.Unique | EnumerableKind.EquatableKey;

    public virtual Dictionary<TKey, TValue> Empty<TKey, TValue>(in Comparers<TKey, TValue> comparers = default) where TKey : notnull =>
#if NET5_0_OR_GREATER
        new(0, comparers.KeyEqualityComparer);
#else
        NewDictionary(0, in comparers);
#endif

    public virtual Dictionary<TKey, TValue> New<TKey, TValue>(int capacity, in Comparers<TKey, TValue> comparers = default) where TKey : notnull =>
#if NET5_0_OR_GREATER
        new(capacity, comparers.KeyEqualityComparer);
#else
        NewDictionary(capacity, in comparers);
#endif

    public virtual Dictionary<TKey, TValue> New<TKey, TValue>(int capacity, EnumerableBuilder<KeyValuePair<TKey, TValue>> builder, in Comparers<TKey, TValue> comparers = default) where TKey : notnull
    {
        if (capacity == 0) return
#if NET5_0_OR_GREATER
        new(0, comparers.KeyEqualityComparer);
#else
        NewDictionary(0, in comparers);
#endif
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var dictionary =
#if NET5_0_OR_GREATER
        new Dictionary<TKey, TValue>(capacity, comparers.KeyEqualityComparer);
#else
        NewDictionary(capacity, in comparers);
#endif

        builder(item => dictionary.TryAdd(item.Key, item.Value));

        return dictionary;
    }

    public virtual Dictionary<TKey, TValue> New<TKey, TValue, TState>(int capacity, EnumerableBuilder<KeyValuePair<TKey, TValue>, TState> builder, in TState state, in Comparers<TKey, TValue> comparers = default) where TKey : notnull
    {
        if (capacity == 0) return
#if NET5_0_OR_GREATER
        new(0, comparers.KeyEqualityComparer);
#else
        NewDictionary(0, in comparers);
#endif
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var dictionary =
#if NET5_0_OR_GREATER
        new Dictionary<TKey, TValue>(capacity, comparers.KeyEqualityComparer);
#else
        NewDictionary(capacity, in comparers);
#endif

        builder(item => dictionary.TryAdd(item.Key, item.Value), in state);

        return dictionary;
    }

    public override int GetHashCode() => HashCode.Combine(GetType());

    public override bool Equals(object? obj) => Equals(obj as DictionaryFactory);

    public bool Equals(DictionaryFactory? other) => this == other || (other != null && other.GetType() == GetType());

#if !NET5_0_OR_GREATER
    protected virtual Dictionary<TKey, TValue> NewDictionary<TKey, TValue>(int capacity, in Comparers<TKey, TValue> comparers) where TKey : notnull
        => new(capacity, comparers.KeyEqualityComparer);
#endif

    IDictionary<TKey, TValue> IDictionaryFactory.Empty<TKey, TValue>(in Comparers<TKey, TValue> comparers) => Empty(in comparers);
    IDictionary<TKey, TValue> IDictionaryFactory.New<TKey, TValue>(int capacity, in Comparers<TKey, TValue> comparers) => New(capacity, in comparers);
    IDictionary<TKey, TValue> IDictionaryFactory.New<TKey, TValue>(int capacity, EnumerableBuilder<KeyValuePair<TKey, TValue>> builder, in Comparers<TKey, TValue> comparers) => New(capacity, builder, in comparers);
    IDictionary<TKey, TValue> IDictionaryFactory.New<TKey, TValue, TState>(int capacity, EnumerableBuilder<KeyValuePair<TKey, TValue>, TState> builder, in TState state, in Comparers<TKey, TValue> comparers) => New(capacity, builder, in state, in comparers);
    IReadOnlyDictionary<TKey, TValue> IReadOnlyDictionaryFactory.Empty<TKey, TValue>(in Comparers<TKey, TValue> comparers) => Empty(in comparers);
    IReadOnlyDictionary<TKey, TValue> IReadOnlyDictionaryFactory.New<TKey, TValue>(int capacity, in Comparers<TKey, TValue> comparers) => New(capacity, in comparers);
    IReadOnlyDictionary<TKey, TValue> IReadOnlyDictionaryFactory.New<TKey, TValue>(int capacity, EnumerableBuilder<KeyValuePair<TKey, TValue>> builder, in Comparers<TKey, TValue> comparers) => New(capacity, builder, in comparers);
    IReadOnlyDictionary<TKey, TValue> IReadOnlyDictionaryFactory.New<TKey, TValue, TState>(int capacity, EnumerableBuilder<KeyValuePair<TKey, TValue>, TState> builder, in TState state, in Comparers<TKey, TValue> comparers) => New(capacity, builder, in state, in comparers);
#pragma warning disable CS8714
    IEnumerable<KeyValuePair<TKey, TValue>> IEnumerableKeyValueFactory.Empty<TKey, TValue>(in Comparers<TKey, TValue> comparers) => Empty(in comparers);
    IEnumerable<KeyValuePair<TKey, TValue>> IEnumerableKeyValueFactory.New<TKey, TValue>(int capacity, in Comparers<TKey, TValue> comparers) => New(capacity, in comparers);
    IEnumerable<KeyValuePair<TKey, TValue>> IEnumerableKeyValueFactory.New<TKey, TValue>(int capacity, EnumerableBuilder<KeyValuePair<TKey, TValue>> builder, in Comparers<TKey, TValue> comparers) => New(capacity, builder, in comparers);
    IEnumerable<KeyValuePair<TKey, TValue>> IEnumerableKeyValueFactory.New<TKey, TValue, TState>(int capacity, EnumerableBuilder<KeyValuePair<TKey, TValue>, TState> builder, in TState state, in Comparers<TKey, TValue> comparers) => New(capacity, builder, in state, in comparers);
#pragma warning restore CS8714
}