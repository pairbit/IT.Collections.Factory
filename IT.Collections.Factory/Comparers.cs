namespace IT.Collections.Factory;

public static class Comparers
{
    public static Comparers<T> New<T>(
        IEqualityComparer<T>? equalityComparer,
        IComparer<T>? comparer) => new(equalityComparer, comparer);

    public static Comparers<T> NewEqualityComparer<T>(IEqualityComparer<T>? equalityComparer)
        => new(equalityComparer);

    public static Comparers<TKey, TValue> NewEqualityComparer<TKey, TValue>(
        IEqualityComparer<TKey>? keyEqualityComparer,
        IEqualityComparer<TValue>? valueEqualityComparer)
        => new(keyEqualityComparer, null, valueEqualityComparer, null);

    public static Comparers<T> NewComparer<T>(IComparer<T>? comparer) => new(comparer);

    public static Comparers<string?> New(StringComparer comparer)
        => new(comparer, comparer);

    public static Comparers<string?, string?> NewKeyValue(StringComparer comparer)
        => new(comparer, comparer, comparer, comparer);
}