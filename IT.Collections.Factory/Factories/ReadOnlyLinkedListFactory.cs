namespace IT.Collections.Factory.Factories;

using Internal;

public class ReadOnlyLinkedListFactory : IEnumerableFactory
{
    public static readonly ReadOnlyLinkedListFactory Default = new();

    public bool IsReadOnly => true;

    public IEnumerable<T> Empty<T>() => ReadOnlyCollection<T>.Empty;

    public IEnumerable<T> New<T>(int capacity)
    {
        throw new NotSupportedException();
    }

    public IEnumerable<T> New<T>(int capacity, EnumerableBuilder<T> builder)
    {
        if (capacity == 0) return ReadOnlyCollection<T>.Empty;
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var list = new LinkedList<T>();

        builder(((ICollection<T>)list).Add, false);

        return new ReadOnlyCollection<T>(list);
    }

    public IEnumerable<T> New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state)
    {
        if (capacity == 0) return ReadOnlyCollection<T>.Empty;
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var list = new LinkedList<T>();

        builder(((ICollection<T>)list).Add, false, in state);

        return new ReadOnlyCollection<T>(list);
    }
}