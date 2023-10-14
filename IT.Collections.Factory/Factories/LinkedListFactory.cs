namespace IT.Collections.Factory.Factories;

public class LinkedListFactory : IEnumerableFactory
{
    public static readonly LinkedListFactory Default = new();

    public EnumerableType Type => EnumerableType.None;

    public IEnumerable<T> Empty<T>() => new LinkedList<T>();

    public IEnumerable<T> New<T>(int capacity) => new LinkedList<T>();

    public IEnumerable<T> New<T>(int capacity, EnumerableBuilder<T> builder)
    {
        if (capacity == 0) return new LinkedList<T>();
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var list = new LinkedList<T>();

        builder(item => { list.AddLast(item); return true; });

        return list;
    }

    public IEnumerable<T> New<T, TState>(int capacity, EnumerableBuilder<T, TState> builder, in TState state)
    {
        if (capacity == 0) return new LinkedList<T>();
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var list = new LinkedList<T>();

        builder(item => { list.AddLast(item); return true; }, in state);

        return list;
    }
}