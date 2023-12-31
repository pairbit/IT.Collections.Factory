﻿#if NETCOREAPP3_1_OR_GREATER

using System.Collections.Immutable;

namespace IT.Collections.Factory.Factories;

public sealed class ImmutableSortedDictionaryFactory : IImmutableDictionaryFactory, IDictionaryFactory, IEquatable<ImmutableSortedDictionaryFactory>
{
    public Type EnumerableType => typeof(ImmutableSortedDictionary<,>);

    public EnumerableKind Kind => EnumerableKind.Ordered | EnumerableKind.Unique | EnumerableKind.ComparableKey | EnumerableKind.EquatableValue | EnumerableKind.IgnoreCapacity;

    public ImmutableSortedDictionary<TKey, TValue> Empty<TKey, TValue>(in Comparers<TKey, TValue> comparers = default) where TKey : notnull
        => ImmutableSortedDictionary<TKey, TValue>.Empty.WithComparers(comparers.KeyComparer, comparers.ValueEqualityComparer);

    public ImmutableSortedDictionary<TKey, TValue> New<TKey, TValue>(int capacity, in Comparers<TKey, TValue> comparers = default) where TKey : notnull
        => ImmutableSortedDictionary<TKey, TValue>.Empty.WithComparers(comparers.KeyComparer, comparers.ValueEqualityComparer);

    public ImmutableSortedDictionary<TKey, TValue> New<TKey, TValue>(int capacity, EnumerableBuilder<KeyValuePair<TKey, TValue>> builder, in Comparers<TKey, TValue> comparers = default) where TKey : notnull
    {
        if (capacity == 0) return ImmutableSortedDictionary<TKey, TValue>.Empty.WithComparers(comparers.KeyComparer, comparers.ValueEqualityComparer);
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var dictionaryBuilder = ImmutableSortedDictionary<TKey, TValue>.Empty.ToBuilder();
        var keyComparer = comparers.KeyComparer;
        if (keyComparer != null) dictionaryBuilder.KeyComparer = keyComparer;
        var valueEqualityComparer = comparers.ValueEqualityComparer;
        if (valueEqualityComparer != null) dictionaryBuilder.ValueComparer = valueEqualityComparer;

        builder(item => dictionaryBuilder.TryAdd(item.Key, item.Value));

        return dictionaryBuilder.ToImmutable();
    }

    public ImmutableSortedDictionary<TKey, TValue> New<TKey, TValue, TState>(int capacity, EnumerableBuilder<KeyValuePair<TKey, TValue>, TState> builder, in TState state, in Comparers<TKey, TValue> comparers = default) where TKey : notnull
    {
        if (capacity == 0) return ImmutableSortedDictionary<TKey, TValue>.Empty.WithComparers(comparers.KeyComparer, comparers.ValueEqualityComparer);
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var dictionaryBuilder = ImmutableSortedDictionary<TKey, TValue>.Empty.ToBuilder();
        var keyComparer = comparers.KeyComparer;
        if (keyComparer != null) dictionaryBuilder.KeyComparer = keyComparer;
        var valueEqualityComparer = comparers.ValueEqualityComparer;
        if (valueEqualityComparer != null) dictionaryBuilder.ValueComparer = valueEqualityComparer;

        builder(item => dictionaryBuilder.TryAdd(item.Key, item.Value), in state);

        return dictionaryBuilder.ToImmutable();
    }

    public override int GetHashCode() => HashCode.Combine(GetType());

    public override bool Equals(object? obj) => Equals(obj as ImmutableSortedDictionaryFactory);

    public bool Equals(ImmutableSortedDictionaryFactory? other) => this == other || (other != null && other.GetType() == GetType());

    IDictionary<TKey, TValue> IDictionaryFactory.Empty<TKey, TValue>(in Comparers<TKey, TValue> comparers) => Empty(in comparers);
    IDictionary<TKey, TValue> IDictionaryFactory.New<TKey, TValue>(int capacity, in Comparers<TKey, TValue> comparers) => New(capacity, in comparers);
    IDictionary<TKey, TValue> IDictionaryFactory.New<TKey, TValue>(int capacity, EnumerableBuilder<KeyValuePair<TKey, TValue>> builder, in Comparers<TKey, TValue> comparers) => New(capacity, builder, in comparers);
    IDictionary<TKey, TValue> IDictionaryFactory.New<TKey, TValue, TState>(int capacity, EnumerableBuilder<KeyValuePair<TKey, TValue>, TState> builder, in TState state, in Comparers<TKey, TValue> comparers) => New(capacity, builder, in state, in comparers);
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