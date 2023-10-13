namespace IT.Collections.Factory;

public static class xEnumerableType
{
    public static bool IsSorted(this EnumerableType type) => (type & EnumerableType.Sorted) == EnumerableType.Sorted;

    public static bool IsReadOnly(this EnumerableType type) => (type & EnumerableType.ReadOnly) == EnumerableType.ReadOnly;

    public static bool IsUnique(this EnumerableType type) => (type & EnumerableType.Unique) == EnumerableType.Unique;

    public static bool IsReverse(this EnumerableType type) => (type & EnumerableType.Reverse) == EnumerableType.Reverse;
}