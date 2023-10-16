namespace IT.Collections.Factory;

public static class xEnumerableType
{
    public static bool IsReadOnly(this EnumerableType type) => (type & EnumerableType.ReadOnly) == EnumerableType.ReadOnly;

    public static bool IsOrdered(this EnumerableType type) => (type & EnumerableType.Ordered) == EnumerableType.Ordered;

    public static bool IsUnordered(this EnumerableType type) => (type & EnumerableType.Unordered) == EnumerableType.Unordered;

    public static bool IsUnique(this EnumerableType type) => (type & EnumerableType.Unique) == EnumerableType.Unique;

    public static bool IsReverse(this EnumerableType type) => (type & EnumerableType.Reverse) == EnumerableType.Reverse;

    public static bool IsFixed(this EnumerableType type) => (type & EnumerableType.Fixed) == EnumerableType.Fixed;

    public static bool IsProxy(this EnumerableType type) => (type & EnumerableType.Proxy) == EnumerableType.Proxy;

    public static bool IsEquatableKey(this EnumerableType type) => (type & EnumerableType.EquatableKey) == EnumerableType.EquatableKey;

    public static bool IsEquatableValue(this EnumerableType type) => (type & EnumerableType.EquatableValue) == EnumerableType.EquatableValue;

    public static bool IsEquatable(this EnumerableType type) => (type & EnumerableType.Equatable) == EnumerableType.Equatable;

    public static bool IsComparableKey(this EnumerableType type) => (type & EnumerableType.ComparableKey) == EnumerableType.ComparableKey;

    public static bool IsComparableValue(this EnumerableType type) => (type & EnumerableType.ComparableValue) == EnumerableType.ComparableValue;

    public static bool IsComparable(this EnumerableType type) => (type & EnumerableType.Comparable) == EnumerableType.Comparable;
}