namespace IT.Collections.Factory;

public readonly struct Comparers<T>
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
}