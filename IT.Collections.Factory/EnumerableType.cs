namespace IT.Collections.Factory;

[Flags]
public enum EnumerableType
{
    None = 0,
    ReadOnly = 1,
    Sorted = 2,
    Unique = 4,
    Reverse = 8
}