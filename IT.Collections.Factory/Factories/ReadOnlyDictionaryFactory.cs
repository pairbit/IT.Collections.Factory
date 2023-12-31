﻿namespace IT.Collections.Factory.Factories;

public class ReadOnlyDictionaryFactory : IDictionaryFactory, IReadOnlyDictionaryFactory, IEquatable<ReadOnlyDictionaryFactory>
{
    protected readonly IDictionaryFactory _factory;

    public virtual Type EnumerableType => typeof(ReadOnlyDictionary<,>);

    public virtual EnumerableKind Kind => EnumerableKind.ReadOnly | _factory.Kind;

    public ReadOnlyDictionaryFactory() : this(EnumerableFactoryCache<DictionaryFactory>.Default) { }

    public ReadOnlyDictionaryFactory(IDictionaryFactory factory)
    {
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
    }

    public virtual ReadOnlyDictionary<TKey, TValue> Empty<TKey, TValue>(in Comparers<TKey, TValue> comparers = default) where TKey : notnull =>
#if NET5_0_OR_GREATER
        Cache<TKey, TValue>.Empty;
#else
        NewDictionary(null, in comparers);
#endif

    public virtual ReadOnlyDictionary<TKey, TValue> New<TKey, TValue>(int capacity, in Comparers<TKey, TValue> comparers = default) where TKey : notnull
        => throw new NotSupportedException();

    public virtual ReadOnlyDictionary<TKey, TValue> New<TKey, TValue>(int capacity, EnumerableBuilder<KeyValuePair<TKey, TValue>> builder, in Comparers<TKey, TValue> comparers = default) where TKey : notnull
    {
        if (capacity == 0) return
#if NET5_0_OR_GREATER
        Cache<TKey, TValue>.Empty;
#else
        NewDictionary(null, in comparers);
#endif

        return
#if NET5_0_OR_GREATER
        new(_factory.New(capacity, builder, in comparers));
#else
        NewDictionary(_factory.New(capacity, builder, in comparers), in comparers);
#endif
    }

    public virtual ReadOnlyDictionary<TKey, TValue> New<TKey, TValue, TState>(int capacity, EnumerableBuilder<KeyValuePair<TKey, TValue>, TState> builder, in TState state, in Comparers<TKey, TValue> comparers = default) where TKey : notnull
    {
        if (capacity == 0) return
#if NET5_0_OR_GREATER
        Cache<TKey, TValue>.Empty;
#else
        NewDictionary(null, in comparers);
#endif

        return
#if NET5_0_OR_GREATER
        new(_factory.New(capacity, builder, in state, in comparers));
#else
        NewDictionary(_factory.New(capacity, builder, in state, in comparers), in comparers);
#endif
    }

    public override int GetHashCode() => HashCode.Combine(GetType(), _factory);

    public override bool Equals(object? obj) => Equals(obj as ReadOnlyDictionaryFactory);

    public bool Equals(ReadOnlyDictionaryFactory? other)
    {
        if (other == this) return true;
        if (other == null || other.GetType() != GetType()) return false;

        var factory = _factory;
        var otherFactory = other._factory;

        return factory == otherFactory || (factory != null && factory.Equals(otherFactory));
    }

#if !NET5_0_OR_GREATER
    protected virtual ReadOnlyDictionary<TKey, TValue> NewDictionary<TKey, TValue>(IDictionary<TKey, TValue>? dictionary, in Comparers<TKey, TValue> comparers) where TKey : notnull
        => dictionary == null ? Cache<TKey, TValue>.Empty : new(dictionary);
#endif

    static class Cache<TKey, TValue> where TKey : notnull
    {
        public readonly static ReadOnlyDictionary<TKey, TValue> Empty = new(new Dictionary<TKey, TValue>());
    }

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