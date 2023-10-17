namespace IT.Collections.Factory;

public readonly struct Comparers<TKey, TValue>
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
}