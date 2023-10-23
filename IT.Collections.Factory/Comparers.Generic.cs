namespace IT.Collections.Factory;

public readonly struct Comparers<T> : IEquatable<Comparers<T>>
{
    private readonly IEqualityComparer<T>? _equalityComparer;
    private readonly IComparer<T>? _comparer;

    public IEqualityComparer<T>? EqualityComparer => _equalityComparer;

    public IComparer<T>? Comparer => _comparer;

    public Comparers(IEqualityComparer<T>? equalityComparer, IComparer<T>? comparer)
    {
        _equalityComparer = equalityComparer;
        _comparer = comparer;
    }

    public Comparers(IEqualityComparer<T>? equalityComparer)
    {
        _equalityComparer = equalityComparer;
    }

    public Comparers(IComparer<T>? comparer)
    {
        _comparer = comparer;
    }

    public override int GetHashCode() => HashCode.Combine(_equalityComparer, _comparer);

    public override bool Equals(
#if NETSTANDARD2_1 || NETCOREAPP3_1_OR_GREATER
        [System.Diagnostics.CodeAnalysis.NotNullWhen(true)]
#endif
        object? other) => other is Comparers<T> comparers && Equals(comparers);

    public bool Equals(Comparers<T> other)
    {
        var equalityComparer = _equalityComparer;
        var otherEqualityComparer = other._equalityComparer;

        if (equalityComparer != otherEqualityComparer &&
           (equalityComparer == null || !equalityComparer.Equals(otherEqualityComparer))) return false;

        var comparer = _comparer;
        var otherComparer = other._comparer;

        return comparer == otherComparer || 
              (comparer != null && comparer.Equals(otherComparer));
    }
}