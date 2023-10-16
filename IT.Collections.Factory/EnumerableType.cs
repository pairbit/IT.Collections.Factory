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
    //NoCapacity
    //Comparable
    //LinkedList
    //ThreadSafe
    //Equatable
    //Comparable
    //EquatableKey
    //ComparableKey
}