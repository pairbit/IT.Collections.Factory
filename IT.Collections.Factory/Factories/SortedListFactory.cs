namespace IT.Collections.Factory.Factories;

public class SortedListFactory : IDictionaryFactory, IReadOnlyDictionaryFactory, IEquatable<SortedListFactory>
{
    public static readonly SortedListFactory Default = new();

    public virtual Type EnumerableType => typeof(SortedList<,>);

    public virtual EnumerableKind Kind => EnumerableKind.Ordered | EnumerableKind.Unique | EnumerableKind.ComparableKey;

    public virtual SortedList<TKey, TValue> Empty<TKey, TValue>(in Comparers<TKey, TValue> comparers = default) where TKey : notnull =>
#if NET5_0_OR_GREATER
        new(comparers.KeyComparer);
#else
        NewSortedList(0, in comparers);
#endif

    public virtual SortedList<TKey, TValue> New<TKey, TValue>(int capacity, in Comparers<TKey, TValue> comparers = default) where TKey : notnull =>
#if NET5_0_OR_GREATER
        new(capacity, comparers.KeyComparer);
#else
        NewSortedList(capacity, in comparers);
#endif

    public virtual SortedList<TKey, TValue> New<TKey, TValue>(int capacity, EnumerableBuilder<KeyValuePair<TKey, TValue>> builder, in Comparers<TKey, TValue> comparers = default) where TKey : notnull
    {
        if (capacity == 0) return
#if NET5_0_OR_GREATER
        new(comparers.KeyComparer);
#else
        NewSortedList(0, in comparers);
#endif
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var dictionary =
#if NET5_0_OR_GREATER
        new SortedList<TKey, TValue>(capacity, comparers.KeyComparer);
#else
        NewSortedList(capacity, in comparers);
#endif

        builder(item => dictionary.TryAdd(item.Key, item.Value));

        return dictionary;
    }

    public virtual SortedList<TKey, TValue> New<TKey, TValue, TState>(int capacity, EnumerableBuilder<KeyValuePair<TKey, TValue>, TState> builder, in TState state, in Comparers<TKey, TValue> comparers = default) where TKey : notnull
    {
        if (capacity == 0) return
#if NET5_0_OR_GREATER
        new(comparers.KeyComparer);
#else
        NewSortedList(0, in comparers);
#endif
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var dictionary =
#if NET5_0_OR_GREATER
        new SortedList<TKey, TValue>(capacity, comparers.KeyComparer);
#else
        NewSortedList(capacity, in comparers);
#endif

        builder(item => dictionary.TryAdd(item.Key, item.Value), in state);

        return dictionary;
    }

    public override int GetHashCode() => HashCode.Combine(GetType());

    public override bool Equals(object? obj) => Equals(obj as SortedListFactory);

    public bool Equals(SortedListFactory? other) => this == other || (other != null && other.GetType() == GetType());

#if !NET5_0_OR_GREATER
    protected virtual SortedList<TKey, TValue> NewSortedList<TKey, TValue>(int capacity, in Comparers<TKey, TValue> comparers) where TKey : notnull
        => capacity == 0 ? new(comparers.KeyComparer) : new(capacity, comparers.KeyComparer);
#endif

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