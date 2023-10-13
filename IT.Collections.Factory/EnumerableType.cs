namespace IT.Collections.Factory;

[Flags]
public enum EnumerableType
{
    None = 0,
    ReadOnly = 1,
    Ordered = 2,
    Unordered = 4,
    Unique = 8,
    Reverse = 16,
    //NoCapacity
    //Fixed
}