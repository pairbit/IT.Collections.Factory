namespace IT.Collections.Factory;

[Flags]
public enum EnumerableType
{
    None = 0,
    ReadOnly = 1,
    Ordered = 2,
    Unordered = 2 << 1,
    Unique = 2 << 2,
    Reverse = 2 << 3,
    Fixed = 2 << 4,
    Proxy = 2 << 5,
    EquatableKey = 2 << 6,
    Equatable = 2 << 7,
    ComparableKey = 2 << 8,
    Comparable = 2 << 9
    //NoCapacity
    //LinkedList
    //ThreadSafe
}