#if NETCOREAPP3_1_OR_GREATER

using System.Collections.Immutable;

namespace IT.Collections.Factory.Factories;

public class ImmutableDictionaryFactory : IDictionaryFactory
{
    public static readonly ImmutableDictionaryFactory Default = new();

    public EnumerableType Type => EnumerableType.None;

    public IDictionary<TKey, TValue> Empty<TKey, TValue>() where TKey : notnull => ImmutableDictionary<TKey, TValue>.Empty;

    public IDictionary<TKey, TValue> New<TKey, TValue>(int capacity) where TKey : notnull
        => ImmutableDictionary<TKey, TValue>.Empty;

    public IDictionary<TKey, TValue> New<TKey, TValue>(int capacity, EnumerableBuilder<KeyValuePair<TKey, TValue>> builder) where TKey : notnull
    {
        if (capacity == 0) return ImmutableDictionary<TKey, TValue>.Empty;
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var dictionaryBuilder = ImmutableDictionary<TKey, TValue>.Empty.ToBuilder();

        builder(dictionaryBuilder.Add, false);

        return dictionaryBuilder.ToImmutable();
    }

    public IDictionary<TKey, TValue> New<TKey, TValue, TState>(int capacity, EnumerableBuilder<KeyValuePair<TKey, TValue>, TState> builder, in TState state) where TKey : notnull
    {
        if (capacity == 0) return ImmutableDictionary<TKey, TValue>.Empty;
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var dictionaryBuilder = ImmutableDictionary<TKey, TValue>.Empty.ToBuilder();

        builder(dictionaryBuilder.Add, false, in state);

        return dictionaryBuilder.ToImmutable();
    }
}

#endif