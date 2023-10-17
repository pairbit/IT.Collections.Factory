namespace IT.Collections.Factory;

public static class xStringComparer
{
    public static Comparers<string?> ToComparers(this StringComparer comparer)
        => new(comparer, comparer);
}