namespace IT.Collections.Factory;

public readonly struct Comparers<TKey, TValue>
{
    private readonly IEqualityComparer<TKey>? _equalityComparerKey;
    private readonly IComparer<TKey>? _comparerKey;

    private readonly IEqualityComparer<TValue>? _equalityComparerValue;
    private readonly IComparer<TValue>? _comparerValue;

    public IEqualityComparer<TKey>? EqualityComparerKey => _equalityComparerKey;

    public IComparer<TKey>? ComparerKey => _comparerKey;

    public IEqualityComparer<TValue>? EqualityComparerValue => _equalityComparerValue;

    public IComparer<TValue>? ComparerValue => _comparerValue;

    public Comparers(
        IEqualityComparer<TKey>? equalityComparerKey = null, IComparer<TKey>? comparerKey = null,
        IEqualityComparer<TValue>? equalityComparerValue = null, IComparer<TValue>? comparerValue = null)
    {
        _equalityComparerKey = equalityComparerKey;
        _comparerKey = comparerKey;
        _equalityComparerValue = equalityComparerValue;
        _comparerValue = comparerValue;
    }
}