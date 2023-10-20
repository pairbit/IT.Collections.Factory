#if NETCOREAPP3_1_OR_GREATER

using System.Collections.Immutable;

namespace IT.Collections.Factory.Factories;

public sealed class ImmutableDictionaryFactory : IImmutableDictionaryFactory, IEquatable<ImmutableDictionaryFactory>
{
    public static readonly ImmutableDictionaryFactory Default = new();

    public Type EnumerableType => typeof(ImmutableDictionary<,>);

    public EnumerableKind Kind => EnumerableKind.Ordered | EnumerableKind.Unique | EnumerableKind.Equatable;

    public ImmutableDictionary<TKey, TValue> Empty<TKey, TValue>(in Comparers<TKey, TValue> comparers = default) where TKey : notnull
        => ImmutableDictionary<TKey, TValue>.Empty.WithComparers(comparers.KeyEqualityComparer, comparers.ValueEqualityComparer);

    public ImmutableDictionary<TKey, TValue> New<TKey, TValue>(int capacity, in Comparers<TKey, TValue> comparers = default) where TKey : notnull
        => ImmutableDictionary<TKey, TValue>.Empty.WithComparers(comparers.KeyEqualityComparer, comparers.ValueEqualityComparer);

    public ImmutableDictionary<TKey, TValue> New<TKey, TValue>(int capacity, EnumerableBuilder<KeyValuePair<TKey, TValue>> builder, in Comparers<TKey, TValue> comparers = default) where TKey : notnull
    {
        if (capacity == 0) return ImmutableDictionary<TKey, TValue>.Empty.WithComparers(comparers.KeyEqualityComparer, comparers.ValueEqualityComparer);
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var dictionaryBuilder = ImmutableDictionary<TKey, TValue>.Empty.ToBuilder();
        var keyEqualityComparer = comparers.KeyEqualityComparer;
        if (keyEqualityComparer != null) dictionaryBuilder.KeyComparer = keyEqualityComparer;
        var valueEqualityComparer = comparers.ValueEqualityComparer;
        if (valueEqualityComparer != null) dictionaryBuilder.ValueComparer = valueEqualityComparer;

        builder(item => dictionaryBuilder.TryAdd(item.Key, item.Value));

        return dictionaryBuilder.ToImmutable();
    }

    public ImmutableDictionary<TKey, TValue> New<TKey, TValue, TState>(int capacity, EnumerableBuilder<KeyValuePair<TKey, TValue>, TState> builder, in TState state, in Comparers<TKey, TValue> comparers = default) where TKey : notnull
    {
        if (capacity == 0) return ImmutableDictionary<TKey, TValue>.Empty.WithComparers(comparers.KeyEqualityComparer, comparers.ValueEqualityComparer);
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var dictionaryBuilder = ImmutableDictionary<TKey, TValue>.Empty.ToBuilder();
        var keyEqualityComparer = comparers.KeyEqualityComparer;
        if (keyEqualityComparer != null) dictionaryBuilder.KeyComparer = keyEqualityComparer;
        var valueEqualityComparer = comparers.ValueEqualityComparer;
        if (valueEqualityComparer != null) dictionaryBuilder.ValueComparer = valueEqualityComparer;

        builder(item => dictionaryBuilder.TryAdd(item.Key, item.Value), in state);

        return dictionaryBuilder.ToImmutable();
    }

    public override int GetHashCode() => HashCode.Combine(GetType());

    public override bool Equals(object? obj) => Equals(obj as ImmutableDictionaryFactory);

    public bool Equals(ImmutableDictionaryFactory? other) => this == other || (other != null && other.GetType() == GetType());

    IImmutableDictionary<TKey, TValue> IImmutableDictionaryFactory.Empty<TKey, TValue>(in Comparers<TKey, TValue> comparers) => Empty(in comparers);
    IImmutableDictionary<TKey, TValue> IImmutableDictionaryFactory.New<TKey, TValue>(int capacity, in Comparers<TKey, TValue> comparers) => New(capacity, in comparers);
    IImmutableDictionary<TKey, TValue> IImmutableDictionaryFactory.New<TKey, TValue>(int capacity, EnumerableBuilder<KeyValuePair<TKey, TValue>> builder, in Comparers<TKey, TValue> comparers) => New(capacity, builder, in comparers);
    IImmutableDictionary<TKey, TValue> IImmutableDictionaryFactory.New<TKey, TValue, TState>(int capacity, EnumerableBuilder<KeyValuePair<TKey, TValue>, TState> builder, in TState state, in Comparers<TKey, TValue> comparers) => New(capacity, builder, in state, in comparers);
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

#endif