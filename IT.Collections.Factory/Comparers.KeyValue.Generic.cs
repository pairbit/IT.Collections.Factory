namespace IT.Collections.Factory;

public readonly struct Comparers<TKey, TValue> : IEquatable<Comparers<TKey, TValue>>
{
    private readonly IEqualityComparer<TKey>? _keyEqualityComparer;
    private readonly IComparer<TKey>? _keyComparer;

    private readonly IEqualityComparer<TValue>? _valueEqualityComparer;
    private readonly IComparer<TValue>? _valueComparer;

    public IEqualityComparer<TKey>? KeyEqualityComparer => _keyEqualityComparer;

    public IComparer<TKey>? KeyComparer => _keyComparer;

    public IEqualityComparer<TValue>? ValueEqualityComparer => _valueEqualityComparer;

    public IComparer<TValue>? ValueComparer => _valueComparer;

    public Comparers(
        IEqualityComparer<TKey>? keyEqualityComparer = null, IComparer<TKey>? keyComparer = null,
        IEqualityComparer<TValue>? valueEqualityComparer = null, IComparer<TValue>? valueComparer = null)
    {
        _keyEqualityComparer = keyEqualityComparer;
        _keyComparer = keyComparer;
        _valueEqualityComparer = valueEqualityComparer;
        _valueComparer = valueComparer;
    }

    public override int GetHashCode()
        => HashCode.Combine(_keyEqualityComparer, _keyComparer, _valueEqualityComparer, _valueComparer);

    public override bool Equals(
#if NETSTANDARD2_1 || NETCOREAPP3_1_OR_GREATER
        [System.Diagnostics.CodeAnalysis.NotNullWhen(true)]
#endif
        object? other) => other is Comparers<TKey, TValue> comparers && Equals(comparers);

    public bool Equals(Comparers<TKey, TValue> other)
    {
        var keyEqualityComparer = _keyEqualityComparer;
        var otherKeyEqualityComparer = other._keyEqualityComparer;

        if (keyEqualityComparer != otherKeyEqualityComparer &&
           (keyEqualityComparer == null || !keyEqualityComparer.Equals(otherKeyEqualityComparer))) return false;

        var keyComparer = _keyComparer;
        var otherKeyComparer = other._keyComparer;

        if (keyComparer != otherKeyComparer &&
           (keyComparer == null || !keyComparer.Equals(otherKeyComparer))) return false;

        var valueEqualityComparer = _valueEqualityComparer;
        var otherValueEqualityComparer = other._valueEqualityComparer;

        if (valueEqualityComparer != otherValueEqualityComparer &&
           (valueEqualityComparer == null || !valueEqualityComparer.Equals(otherValueEqualityComparer))) return false;

        var valueComparer = _valueComparer;
        var otherValueComparer = other._valueComparer;

        return valueComparer == otherValueComparer || 
              (valueComparer != null && valueComparer.Equals(otherValueComparer));
    }
}