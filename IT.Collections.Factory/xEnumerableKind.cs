namespace IT.Collections.Factory;

public static class xEnumerableKind
{
    public static bool IsReadOnly(this EnumerableKind kind) => (kind & EnumerableKind.ReadOnly) == EnumerableKind.ReadOnly;

    public static bool IsOrdered(this EnumerableKind kind) => (kind & EnumerableKind.Ordered) == EnumerableKind.Ordered;

    public static bool IsUnordered(this EnumerableKind kind) => (kind & EnumerableKind.Unordered) == EnumerableKind.Unordered;

    /// <summary>
    /// kind.IsOrdered() || kind.IsUnordered()
    /// </summary>
    public static bool HasOrdered(this EnumerableKind kind) => kind.IsOrdered() || kind.IsUnordered();

    public static bool IsUnique(this EnumerableKind kind) => (kind & EnumerableKind.Unique) == EnumerableKind.Unique;

    public static bool IsReverse(this EnumerableKind kind) => (kind & EnumerableKind.Reverse) == EnumerableKind.Reverse;

    public static bool IsFixed(this EnumerableKind kind) => (kind & EnumerableKind.Fixed) == EnumerableKind.Fixed;

    public static bool IsProxy(this EnumerableKind kind) => (kind & EnumerableKind.Proxy) == EnumerableKind.Proxy;

    public static bool IsEquatableKey(this EnumerableKind kind) => (kind & EnumerableKind.EquatableKey) == EnumerableKind.EquatableKey;

    public static bool IsEquatableValue(this EnumerableKind kind) => (kind & EnumerableKind.EquatableValue) == EnumerableKind.EquatableValue;

    public static bool IsEquatable(this EnumerableKind kind) => (kind & EnumerableKind.Equatable) == EnumerableKind.Equatable;

    public static bool IsComparableKey(this EnumerableKind kind) => (kind & EnumerableKind.ComparableKey) == EnumerableKind.ComparableKey;

    public static bool IsComparableValue(this EnumerableKind kind) => (kind & EnumerableKind.ComparableValue) == EnumerableKind.ComparableValue;

    public static bool IsComparable(this EnumerableKind kind) => (kind & EnumerableKind.Comparable) == EnumerableKind.Comparable;

    public static bool IsThreadSafe(this EnumerableKind kind) => (kind & EnumerableKind.ThreadSafe) == EnumerableKind.ThreadSafe;
}