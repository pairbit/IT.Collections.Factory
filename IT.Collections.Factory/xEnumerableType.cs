namespace IT.Collections.Factory;

public static class xEnumerableType
{
    public static bool IsReadOnly(this EnumerableType type) => (type & EnumerableType.ReadOnly) == EnumerableType.ReadOnly;

    public static bool IsOrdered(this EnumerableType type) => (type & EnumerableType.Ordered) == EnumerableType.Ordered;

    public static bool IsUnordered(this EnumerableType type) => (type & EnumerableType.Unordered) == EnumerableType.Unordered;

    public static bool IsUnique(this EnumerableType type) => (type & EnumerableType.Unique) == EnumerableType.Unique;

    public static bool IsReverse(this EnumerableType type) => (type & EnumerableType.Reverse) == EnumerableType.Reverse;

    public static bool IsFixed(this EnumerableType type) => (type & EnumerableType.Fixed) == EnumerableType.Fixed;
}