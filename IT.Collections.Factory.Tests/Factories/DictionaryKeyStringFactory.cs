using IT.Collections.Factory.Generic;

namespace IT.Collections.Factory.Tests;

public class DictionaryKeyStringFactory<TValue> : IDictionaryFactory<Dictionary<string, TValue>, string, TValue>,
    IEquatable<DictionaryKeyStringFactory<TValue>>
{
    private readonly IEqualityComparer<string?>? _comparer;

    public Type EnumerableType => typeof(Dictionary<string, TValue>);

    public EnumerableKind Kind => EnumerableKind.None;

    public IEqualityComparer<string?>? Comparer => _comparer;

    public DictionaryKeyStringFactory(IEqualityComparer<string?>? comparer)
    {
        _comparer = comparer;
    }

    public Dictionary<string, TValue> Empty() => new(_comparer);

    public Dictionary<string, TValue> New(int capacity) => new(capacity, _comparer);

    public Dictionary<string, TValue> New(int capacity, EnumerableBuilder<KeyValuePair<string, TValue>> builder)
    {
        if (capacity == 0) return new(_comparer);
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var dictionary = new Dictionary<string, TValue>(capacity, _comparer);

        builder(item => dictionary.TryAdd(item.Key, item.Value));

        return dictionary;
    }

    public Dictionary<string, TValue> New<TState>(int capacity, EnumerableBuilder<KeyValuePair<string, TValue>, TState> builder, in TState state)
    {
        if (capacity == 0) return new(_comparer);
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var dictionary = new Dictionary<string, TValue>(capacity, _comparer);

        builder(item => dictionary.TryAdd(item.Key, item.Value), in state);

        return dictionary;
    }

    public override int GetHashCode() => HashCode.Combine(_comparer, GetType());

    public override bool Equals(object? obj) => Equals(obj as DictionaryKeyStringFactory<TValue>);

    public bool Equals(DictionaryKeyStringFactory<TValue>? other)
        => other != null && _comparer == other._comparer && GetType() == other.GetType();
}