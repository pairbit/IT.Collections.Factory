namespace IT.Collections.Factory;

[Flags]
public enum EnumerableKind
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
    EquatableValue = 2 << 7,
    Equatable = EquatableKey | EquatableValue,
    ComparableKey = 2 << 8,
    ComparableValue = 2 << 9,
    Comparable = ComparableKey | ComparableValue,
    ThreadSafe = 2 << 10,
    //NoCapacity
    //LinkedList
}