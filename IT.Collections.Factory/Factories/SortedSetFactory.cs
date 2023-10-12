namespace IT.Collections.Factory.Factories;

public class SortedSetFactory : IEnumerableFactory
{
    public static readonly SortedSetFactory Default = new();

    public bool IsReadOnly => false;

    public IEnumerable<T> Empty<T>() => new SortedSet<T>();

    public IEnumerable<T> New<T>(int capacity) => new SortedSet<T>();

    public IEnumerable<T> New<T>(int capacity, EnumerableBuilder<T> builder)
    {
        if (capacity == 0) return new SortedSet<T>();
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var sortedSet = new SortedSet<T>();

        builder(((ICollection<T>)sortedSet).Add, false);

        return sortedSet;
    }

    public IEnumerable<T> New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state)
    {
        if (capacity == 0) return new SortedSet<T>();
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var sortedSet = new SortedSet<T>();

        builder(((ICollection<T>)sortedSet).Add, false, in state);

        return sortedSet;
    }
}