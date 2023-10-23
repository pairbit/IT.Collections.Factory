using System.Runtime.CompilerServices;

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

    public static bool operator ==(Comparers<TKey, TValue> left, Comparers<TKey, TValue> right) => EqualsCore(in left, in right);

    public static bool operator !=(Comparers<TKey, TValue> left, Comparers<TKey, TValue> right) => !EqualsCore(in left, in right);

    public override int GetHashCode()
        => HashCode.Combine(_keyEqualityComparer, _keyComparer, _valueEqualityComparer, _valueComparer);

    public override bool Equals(
#if NETSTANDARD2_1 || NETCOREAPP3_1_OR_GREATER
        [System.Diagnostics.CodeAnalysis.NotNullWhen(true)]
#endif
        object? other) => other is Comparers<TKey, TValue> comparers && EqualsCore(in this, in comparers);

    public bool Equals(Comparers<TKey, TValue> other) => EqualsCore(in this, in other);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool EqualsCore(in Comparers<TKey, TValue> left, in Comparers<TKey, TValue> right)
    {
        var lkeq = left._keyEqualityComparer;
        var rkeq = right._keyEqualityComparer;
        if (lkeq != rkeq && (lkeq == null || !lkeq.Equals(rkeq))) return false;

        var lkc = left._keyComparer;
        var rkc = right._keyComparer;
        if (lkc != rkc && (lkc == null || !lkc.Equals(rkc))) return false;

        var lvec = left._valueEqualityComparer;
        var rvec = right._valueEqualityComparer;
        if (lvec != rvec && (lvec == null || !lvec.Equals(rvec))) return false;

        var lvc = left._valueComparer;
        var rvc = right._valueComparer;
        return lvc == rvc || (lvc != null && lvc.Equals(rvc));
    }
}