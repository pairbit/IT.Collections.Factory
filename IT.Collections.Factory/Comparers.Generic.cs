using System.Runtime.CompilerServices;

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

    public static bool operator ==(Comparers<T> left, Comparers<T> right) => EqualsCore(in left, in right);

    public static bool operator !=(Comparers<T> left, Comparers<T> right) => !EqualsCore(in left, in right);

    public override int GetHashCode() => HashCode.Combine(_equalityComparer, _comparer);

    public override bool Equals(
#if NETSTANDARD2_1 || NETCOREAPP3_1_OR_GREATER
        [System.Diagnostics.CodeAnalysis.NotNullWhen(true)]
#endif
        object? other) => other is Comparers<T> comparers && EqualsCore(in this, in comparers);

    public bool Equals(Comparers<T> other) => EqualsCore(in this, in other);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool EqualsCore(in Comparers<T> left, in Comparers<T> right)
    {
        var lec = left._equalityComparer;
        var rec = right._equalityComparer;
        if (lec != rec && (lec == null || !lec.Equals(rec))) return false;

        var lc = left._comparer;
        var rc = right._comparer;
        return lc == rc || (lc != null && lc.Equals(rc));
    }
}